using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class TakeLootBox : UIEntity
    {
        public Inventory inventory;
        public List<ItemField> itemFields { get; private set; } = new List<ItemField>();
        List<LootItem> lootItems = new List<LootItem>();
        List<Item> itemsToLoot;
        Entity entityLooted = null;

        public SpriteFont SpriteFont { get; private set; } 
        NineSpliceSprite nineSpliceSprite;
        TextButton takeAllButton;
        TextButton closeButton;

        Rectangle menuDestinationRect = new Rectangle();
        public Rectangle FieldDestinationRect = new Rectangle();

        public TakeLootBox(List<Item> _itemsToLoot, Inventory _inventory, Entity _entityLooted)
        {
            inventory = _inventory;
            itemsToLoot = _itemsToLoot;
            entityLooted = _entityLooted;

            GameState.OpenMenu();
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            SpriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");

            FillItemFields();
            SetNineSpliceSprite();
            InitializeItemFields();
            InitializeTakeAllAndCloseButtons();
            SetFocusNeighbors();

            CurrentFocused = itemFields[0].ItemButton;
            CurrentFocused.onFocusEntered();
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            for (int i = 0; i < itemFields.Count; ++i)
            {
                if (i >= itemFields.Count)
                    break;
                
                itemFields[i].Update(deltaTime);
            }

            takeAllButton.Update(deltaTime);
            closeButton.Update(deltaTime);
        }

        public override void Draw(float deltaTime)
        {
            nineSpliceSprite.Draw(deltaTime);
        }

        public override void DrawText(float deltaTime)
        {
            for (int i = 0; i < itemFields.Count; ++i)
            {
                if (i >= itemFields.Count)
                    break;
                
                itemFields[i].Draw(deltaTime);
            }

            takeAllButton.Draw(deltaTime);
            closeButton.Draw(deltaTime);
        }

        private void SetNineSpliceSprite()
        {
            nineSpliceSprite = new NineSpliceSprite(this);

            int spriteWidth = 0;
            int spriteHeight = 0;
            int posX = 0;
            int posY = 0;

            float longestName = itemFields[0].NameWidth.X;
            float nameHeight = itemFields[0].NameWidth.Y / Screen.Scale;
            foreach (ItemField itemField in itemFields)
            {
                if (itemField.NameWidth.X > longestName)
                    longestName = itemField.NameWidth.X;
            }
            longestName /= Screen.Scale;

            spriteWidth = (int)longestName + 32;
            spriteHeight = (int)(itemFields.Count * nameHeight) + 16 + (int)(nameHeight * 2) + 8;
            posX = (Screen.Width / 2) - (spriteWidth / 2);
            posY = (Screen.Height / 2) - (spriteHeight / 2);

            menuDestinationRect = new Rectangle(posX, posY, spriteWidth, spriteHeight);
            FieldDestinationRect = new Rectangle(posX + 8, posY + 8, spriteWidth - 16, spriteHeight - 16);

            nineSpliceSprite.Initialize(uiEntityManager.ContentManager.Load<Texture2D>("ui/9splicesprite"), menuDestinationRect);
        }

        private void InitializeTakeAllAndCloseButtons()
        {
            takeAllButton = new TextButton(this);
            closeButton = new TextButton(this);

            float buttonHeight = itemFields[0].NameWidth.Y / Screen.Scale;
            float initalButtonSpawnPosY = FieldDestinationRect.Y + FieldDestinationRect.Height;

            takeAllButton.Initialize(SpriteFont, "Take All", new Vector2(FieldDestinationRect.X, initalButtonSpawnPosY - (buttonHeight * 2)), Color.White, TextButton.TextAlignment.LEFT);
            closeButton.Initialize(SpriteFont, "Close", new Vector2(FieldDestinationRect.X, initalButtonSpawnPosY - buttonHeight), Color.White, TextButton.TextAlignment.LEFT);
        
            Action closeAction = Close;
            
            Action takeAllAction = delegate 
            {
                for (int i = itemsToLoot.Count - 1; i >= 0; --i)
                {
                    inventory.AddItem(itemsToLoot[i]);
                    itemsToLoot.RemoveAt(i);
                }

                Close();
            };

            closeButton.Pressed.Add("take_loot_box", closeAction);
            takeAllButton.Pressed.Add("take_loot_box", takeAllAction);
        }

        public void FillItemFields()
        {
            List<LootItem> lootItems = new List<LootItem>();
            LootItem currentItem = null;
            int itemFieldIndex = 0;

            foreach (Item item in itemsToLoot)
                lootItems.Add(new LootItem(item, false));
            
            for (int i = 0; i < lootItems.Count; ++i)
            {
                if (!lootItems[i].IsAdded)
                {
                    ItemField itemField = new ItemField(this, lootItems[i].Item, itemFieldIndex);
                    itemFields.Add(itemField);
                    currentItem = lootItems[i];
                    lootItems[i].IsAdded = true;
                    itemFieldIndex++;
                    itemField.Amount++;

                    for (int j = 0; j < lootItems.Count; ++j)
                    {
                        if (lootItems[j].IsAdded)
                            continue;
                        
                        if (lootItems[j].Item.GetType() == currentItem.Item.GetType())
                        {
                            itemField.ItemsContained.Add(lootItems[j].Item);
                            itemField.Amount++;
                            lootItems[j].IsAdded = true;
                        }
                    }
                }
            }
        }

        public void InitializeItemFields() 
        {
            foreach (ItemField itemField in itemFields)
            {
                itemField.Initialize();
            }
        }

        public void SetFocusNeighbors()
        {
            for (int i = 0; i < itemFields.Count; ++i)
            {
                if (i == 0)
                {
                    itemFields[i].ItemButton.FocusNeighborUp = closeButton;
                    
                    if (itemFields.Count > 1)
                    {
                        itemFields[i].ItemButton.FocusNeighborDown = itemFields[i + 1].ItemButton;
                    }
                    else
                    {
                        itemFields[i].ItemButton.FocusNeighborDown = takeAllButton;
                    }
                }
                else if (i == itemFields.Count - 1)
                {
                    itemFields[i].ItemButton.FocusNeighborUp = itemFields[i - 1].ItemButton;
                    itemFields[i].ItemButton.FocusNeighborDown = takeAllButton;
                }
                else
                {
                    itemFields[i].ItemButton.FocusNeighborUp = itemFields[i - 1].ItemButton;
                    itemFields[i].ItemButton.FocusNeighborDown = itemFields[i + 1].ItemButton;
                }
            }

            takeAllButton.FocusNeighborUp = itemFields[itemFields.Count - 1].ItemButton;
            takeAllButton.FocusNeighborDown = closeButton;

            closeButton.FocusNeighborUp = takeAllButton;
            closeButton.FocusNeighborDown = itemFields[0].ItemButton;
        }

        public void ClearItemFields()
        {
            lootItems.Clear();
            itemFields.Clear();
        }

        public void Close()
        {
            if (itemsToLoot.Count == 0 && entityLooted is Enemy)
            {
                InteractionComponent entityInteraction = entityLooted.GetComponent<InteractionComponent>();
                entityInteraction.MainInteraction = InteractionComponent.InteractionType.NONE;
                entityInteraction.Interactions.Clear();
            }

            GameState.CloseMenu();
            uiEntityManager.RemoveEntity<TakeLootBox>(this);
        }

        public class ItemField
        {
            public TextButton ItemButton { get; private set; }
            public Label AmountLabel { get; private set; }
            public int Amount { get; set; }
            public Vector2 NameWidth { get; private set; }
            public Vector2 Position { get; private set; }
            public List<Item> ItemsContained { get; private set; } = new List<Item>();
            public int ItemFieldIndex { get; private set; }
            Item currentItem;
            TakeLootBox owner;

            public ItemField(TakeLootBox _owner, Item _item, int _itemFieldIndex)
            {
                owner = _owner;
                ItemsContained.Add(_item);
                currentItem = _item;
                ItemFieldIndex = _itemFieldIndex;
                NameWidth = owner.SpriteFont.MeasureString(currentItem.Name);
            }

            public void Initialize()
            {
                ItemButton = new TextButton(owner);
                AmountLabel = new Label(owner);

                Position = new Vector2(owner.FieldDestinationRect.X, owner.FieldDestinationRect.Y + ((NameWidth.Y / Screen.Scale) * ItemFieldIndex));
                ItemButton.Initialize(owner.SpriteFont, currentItem.Name, Position, Color.White, TextButton.TextAlignment.LEFT);
                Vector2 amountPosition = new Vector2(Position.X + (NameWidth.X / Screen.Scale) + 8, Position.Y);
                AmountLabel.Initialize(owner.SpriteFont, amountPosition, Amount.ToString(), Color.White);

                Action _onPressed = onPressed;
                ItemButton.Pressed.Add("item_field", _onPressed);
            }

            public void Update(float deltaTime)
            {
                ItemButton.Update(deltaTime);
            }

            public void Draw(float deltaTime)
            {
                ItemButton.Draw(deltaTime);

                if (Amount > 1)
                    AmountLabel.Draw(deltaTime);
            }

            public void onPressed()
            {
                owner.inventory.AddItem(currentItem);
                owner.itemsToLoot.Remove(currentItem);
                ItemsContained.Remove(currentItem);
                Amount--;
                AmountLabel.Text = Amount.ToString();

                if (ItemsContained.Count > 0)
                {
                    currentItem = ItemsContained[0];
                }
                else // an item is depleted, so reform the list
                {
                    owner.ClearItemFields();
                    owner.FillItemFields();
                    owner.InitializeItemFields();

                    if (owner.itemsToLoot.Count == 0)
                    {
                        owner.Close();
                    }
                    else // this is seperated to prevent crash when all items are cleared
                    {
                        owner.SetFocusNeighbors();

                        if (ItemFieldIndex < owner.itemFields.Count) // not the last item, so simply focus the next item
                            owner.CurrentFocused = owner.itemFields[ItemFieldIndex].ItemButton;
                        else // the last item, so focus the previous item
                            owner.CurrentFocused = owner.itemFields[ItemFieldIndex - 1].ItemButton;
                        
                        owner.CurrentFocused.onFocusEntered();
                    }
                }
            }
        }

        class LootItem
        {
            public Item Item { get; private set; }
            public bool IsAdded { get; set; } = false;

            public LootItem(Item _item, bool _isAdded)
            {
                Item = _item;
            }
        }
    }
}