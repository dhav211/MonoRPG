using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class TakeLootBox : UIEntity
    {
        public Inventory inventory;
        List<ItemField> itemFields = new List<ItemField>();
        List<LootItem> lootItems = new List<LootItem>();
        List<Item> itemsToLoot;

        public SpriteFont SpriteFont { get; private set; } 
        NineSpliceSprite nineSpliceSprite;
        TextButton takeAllButton;
        TextButton closeButton;

        Rectangle menuDestinationRect = new Rectangle();
        public Rectangle FieldDestinationRect = new Rectangle();

        public TakeLootBox(List<Item> _itemsToLoot, Inventory _inventory)
        {
            inventory = _inventory;
            itemsToLoot = _itemsToLoot;

            GameState.OpenMenu();
        }

        /*
            The nine splice sprite will be the last thing initialized because it's size and location is based upon how many item fields will be created, and 
                width of longest item name. Unless Take All length is longer that will be it.
            As with InventoryMenu itself, create all the item fields by creating a new list of loot items from itemsToLoot list. The big difference here is the 
                text buttons will not be initially initialized.
            When each ItemField is created, run a tally of the total height of item fields, this will add the height of each item field when created.
            Once all item fields are spawned, find the item name with the longest name, this will help determine width of menu.
            Now we can initialize the NineSpliceSprite. Calculate the height of item field, plus the two buttons. Then width of longest item name plus amount
                number and space between. Finally center the the menu so it displays in center of screen.
            Now get the field rect, which will be the size of the ninesplice rect but smaller to factor in buffer on sides.
            Initialize text buttons in item fields
            Initalize Close and Take All buttons.

        */

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            SpriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");
            FillItemFields();

            SetNineSpliceSprite();

            InitializeItemFields();

            InitializeTakeAllAndCloseButtons();
        }

        public override void Update(float deltaTime)
        {
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
            FieldDestinationRect = new Rectangle(posX + 8, posY + 8, spriteWidth - 16, spriteWidth - 16);

            nineSpliceSprite.Initialize(uiEntityManager.ContentManager.Load<Texture2D>("ui/9splicesprite"), menuDestinationRect);
        }

        private void InitializeTakeAllAndCloseButtons()
        {
            takeAllButton = new TextButton(this);
            closeButton = new TextButton(this);

            float buttonHeight = itemFields[0].NameWidth.Y / Screen.Scale;
            float initalButtonSpawnPosY = FieldDestinationRect.Y + FieldDestinationRect.Height - buttonHeight;

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

        public void ClearItemFields()
        {
            lootItems.Clear();
            itemFields.Clear();
        }

        public void Close()
        {
            GameState.CloseMenu();
            uiEntityManager.RemoveEntity<TakeLootBox>(this);
        }

        class ItemField
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
                else
                {
                    owner.ClearItemFields();
                    owner.FillItemFields();
                    owner.InitializeItemFields();

                    if (owner.itemsToLoot.Count == 0)
                    {
                        owner.Close();
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