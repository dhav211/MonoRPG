using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class InventoryMenu : UIEntity
    {
        public enum SectionOpen { CONSUMABLE, EQUIPMENT, KEY, BATTLE }
        public SectionOpen currentSectionOpen { get; private set; } = SectionOpen.CONSUMABLE;

        List<ItemField> currentItemFields = new List<ItemField>();

        public Inventory inventory { get; private set; }
        public SpriteFont SpriteFont {get; private set; }
        NineSpliceSprite nineSpliceSprite;
        TextButton consumableButton;
        TextButton equipmentButton;
        TextButton keyButton;
        TextButton battleButton;
        Label nameLabel;
        Label amountLabel;
        Label weightLabel;
        Label emptyLabel;

        Rectangle menuDestinationRect;
        public Rectangle fieldDestinationRect { get; private set; }

        public Player Player { get; private set; }

        public InventoryMenu(Inventory _inventory, EntityManager _entityManager)
        {
            IsScrollable = false;
            inventory = _inventory;
            GameState.OpenMenu();
            
            Player = _entityManager.GetEntityOfType<Player>();

            menuDestinationRect = new Rectangle(Screen.Width / 4, Screen.Height / 4, Screen.Width / 2, Screen.Height / 2);
            fieldDestinationRect = new Rectangle(menuDestinationRect.X + 8, menuDestinationRect.Y + 24, menuDestinationRect.Width - 16, menuDestinationRect.Height - 16);
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            SpriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");
            nineSpliceSprite = new NineSpliceSprite(this);
            nineSpliceSprite.Initialize(uiEntityManager.ContentManager.Load<Texture2D>("ui/9splicesprite"), menuDestinationRect);

            InitializeHeaderLabels();
            InitializeTabButtons();

            currentSectionOpen = SectionOpen.CONSUMABLE;
            OpenSection(SectionOpen.CONSUMABLE);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            consumableButton.Update(deltaTime);
            equipmentButton.Update(deltaTime);
            keyButton.Update(deltaTime);
            battleButton.Update(deltaTime);

            for (int i = 0; i < currentItemFields.Count; ++i)
            {
                if (i >= currentItemFields.Count)
                    break;
                
                currentItemFields[i].Update(deltaTime);
            }
        }

        public override void Draw(float deltaTime)
        {
            nineSpliceSprite.Draw(deltaTime);
        }

        public override void DrawText(float deltaTime)
        {
            nameLabel.Draw(deltaTime);
            amountLabel.Draw(deltaTime);
            weightLabel.Draw(deltaTime);
            emptyLabel.Draw(deltaTime);
            consumableButton.Draw(deltaTime);
            equipmentButton.Draw(deltaTime);
            keyButton.Draw(deltaTime);
            battleButton.Draw(deltaTime);

            foreach(ItemField itemField in currentItemFields)
            {
                itemField.Draw(deltaTime);
            }
        }

        ///<summary>
        /// Sends the correct item list from inventory, based on parameter, to the FillItemFields function. If list is empty, sets the empty label.
        ///</summary>
        public void OpenSection(SectionOpen _sectionToOpen)
        {
            currentSectionOpen = _sectionToOpen;
            currentItemFields.Clear();

            switch (_sectionToOpen)
            {
                case SectionOpen.CONSUMABLE:
                    {
                        if (inventory.ConsumableItems.Count > 0)
                        {
                            FillItemFields<ConsumableItem>(inventory.ConsumableItems);
                            emptyLabel.IsVisible = false;
                        }
                        else
                            SetEmptyText(_sectionToOpen);

                        break;
                    }

                case SectionOpen.EQUIPMENT:
                    {
                        if (inventory.Equipment.Count > 0)
                        {
                            FillItemFields<Equipment>(inventory.Equipment);
                            emptyLabel.IsVisible = false;
                        }
                        else
                            SetEmptyText(_sectionToOpen);

                        break;
                    }

                case SectionOpen.KEY:
                    {
                        if (inventory.KeyItems.Count > 0)
                        {
                            FillItemFields<KeyItem>(inventory.KeyItems);
                            emptyLabel.IsVisible = false;
                        }
                        else
                            SetEmptyText(_sectionToOpen);

                        break;
                    }

                case SectionOpen.BATTLE:
                    {
                        if (inventory.BattleItems.Count > 0)
                        {
                            FillItemFields<BattleItem>(inventory.BattleItems);
                            emptyLabel.IsVisible = false;
                        }
                        else
                            SetEmptyText(_sectionToOpen);
                    }
                    break;
            }
        }

        ///<summary>
        /// Emptys the list that contains all item fields.
        ///</summary>
        public void ClearSection()
        {
            currentItemFields.Clear();
        }

        ///<summary>
        /// Sets the text in the Empty label based upon the parameter given
        ///</summary>
        private void SetEmptyText(SectionOpen _sectionToOpen)
        {
            emptyLabel.IsVisible = true;

            switch(_sectionToOpen)
            {
                case SectionOpen.CONSUMABLE:
                    emptyLabel.Text = "No consumable items!";
                    break;
                case SectionOpen.EQUIPMENT:
                    emptyLabel.Text = "No equipment!";
                    break;
                case SectionOpen.KEY:
                    emptyLabel.Text = "No key items!";
                    break;
                case SectionOpen.BATTLE:
                    emptyLabel.Text = "No battle items!";
                    break;
            }
        }

        ///<summary>
        /// Creates all the item fields based upon the given list parameter.
        ///</summary>
        private void FillItemFields<T>(List<T> _items) where T : Item
        {
            List<InventoryItem> newItemList = new List<InventoryItem>();
            InventoryItem currentItem = null;
            int currentItemOrder = 0;

            foreach(T item in _items) // Create an InventoryItem object for each item in list
                newItemList.Add(new InventoryItem(item));
            
            for(int i = 0; i < newItemList.Count; ++i)
            {
                if (!newItemList[i].IsAdded) // Current item in list hasn't been added, so begin creating a new ItemField
                {
                    ItemField itemField = new ItemField(this, newItemList[i].Item, currentItemOrder);
                    currentItemFields.Add(itemField);
                    currentItem = newItemList[i];  // this variable aids in checking for repeating items and setting weights
                    newItemList[i].IsAdded = true;
                    currentItemOrder++;
                    itemField.SetAmount(1);
                    itemField.SetWeight(currentItem.Item.Weight);
                    itemField.ItemsContained.Add(newItemList[i].Item);

                    for (int j = 0; j < newItemList.Count; ++j)
                    {
                        if (newItemList[j].IsAdded)  // Item has already been added so don't add it again
                            continue;
                        
                        if (newItemList[j].Item.GetType() == currentItem.Item.GetType())  // The same type of item, so add it to the current item field
                        {
                            newItemList[j].IsAdded = true;
                            itemField.SetAmount(1);
                            itemField.SetWeight(currentItem.Item.Weight);
                            itemField.ItemsContained.Add(newItemList[j].Item);
                        }
                    }
                }
            }

            SetFocusNeighborsForItems();
        }

        private void SetFocusNeighborsForItems()
        {
            if (currentItemFields.Count == 0)
                return;
            
            for (int i = 0; i < currentItemFields.Count; ++i)
            {
                if (i == 0)  // For the first item
                {
                    // The first item in the list, when pressed up will return to the tab which the item belongs in.
                    // example: a potion, up will lead you to the consumables button
                    switch(currentItemFields[i].ItemType)
                    {
                        case Item.ItemType.CONSUMABLE:
                        {
                            currentItemFields[i].ItemButton.FocusNeighborUp = consumableButton;
                            break;
                        }
                        case Item.ItemType.EQUIPMENT:
                        {
                            currentItemFields[i].ItemButton.FocusNeighborUp = equipmentButton;
                            break;
                        }
                        case Item.ItemType.BATTLE:
                        {
                            currentItemFields[i].ItemButton.FocusNeighborUp = battleButton;
                            break;
                        }
                        case Item.ItemType.KEY:
                        {
                            currentItemFields[i].ItemButton.FocusNeighborUp = keyButton;
                            break;
                        }
                    }
                    
                    if (currentItemFields.Count > 1) // Go to the next button if there is more than one item
                        currentItemFields[i].ItemButton.FocusNeighborDown = currentItemFields[i + 1].ItemButton;
                }
                else if (i == currentItemFields.Count - 1)  // For the last item
                {
                    currentItemFields[i].ItemButton.FocusNeighborUp = currentItemFields[i - 1].ItemButton;
                    currentItemFields[i].ItemButton.FocusNeighborDown = currentItemFields[0].ItemButton;
                }
                else   // for all the items in between
                {
                    currentItemFields[i].ItemButton.FocusNeighborUp = currentItemFields[i - 1].ItemButton;
                    currentItemFields[i].ItemButton.FocusNeighborDown = currentItemFields[i + 1].ItemButton;
                }
            }

            // Set all the tab buttons to the first item field in the list
            consumableButton.FocusNeighborDown = currentItemFields[0].ItemButton;
            equipmentButton.FocusNeighborDown = currentItemFields[0].ItemButton;
            keyButton.FocusNeighborDown = currentItemFields[0].ItemButton;
            battleButton.FocusNeighborDown = currentItemFields[0].ItemButton;
        }

        ///<summary>
        /// Remove from UI Entity Manager so it will no longer be updated or drawn
        ///</summary>
        public void Close()
        {
            GameState.CloseMenu();
            uiEntityManager.RemoveEntity<InventoryMenu>(this);
        }

        private void InitializeHeaderLabels()
        {
            nameLabel = new Label(this);
            amountLabel = new Label(this);
            weightLabel = new Label(this);
            emptyLabel = new Label(this);

            float amountXPosAdjuster = (fieldDestinationRect.Width * .2f) * 3;
            float weightXPosAdjust = (fieldDestinationRect.Width * .2f) * 4;

            nameLabel.Initialize(SpriteFont, new Vector2(fieldDestinationRect.X, fieldDestinationRect.Y), "Item Name", Color.White);
            amountLabel.Initialize(SpriteFont, new Vector2(fieldDestinationRect.X + amountXPosAdjuster, fieldDestinationRect.Y), "Amount", Color.White);
            weightLabel.Initialize(SpriteFont, new Vector2(fieldDestinationRect.X + weightXPosAdjust, fieldDestinationRect.Y), "Weight", Color.White);
            emptyLabel.Initialize(SpriteFont, new Vector2(fieldDestinationRect.X, fieldDestinationRect.Y + 16), "Text", Color.White);
        }

        private void InitializeTabButtons()
        {
            consumableButton = new TextButton(this);
            equipmentButton = new TextButton(this);
            keyButton = new TextButton(this);
            battleButton = new TextButton(this);

            consumableButton.Initialize(SpriteFont, "Consumable", new Vector2(menuDestinationRect.X + (menuDestinationRect.Width * .2f), menuDestinationRect.Y + 8), Color.White, TextButton.TextAlignment.LEFT);
            equipmentButton.Initialize(SpriteFont, "Equipment", new Vector2(menuDestinationRect.X + (menuDestinationRect.Width * .2f) * 2, menuDestinationRect.Y + 8), Color.White, TextButton.TextAlignment.LEFT);
            keyButton.Initialize(SpriteFont, "Key", new Vector2(menuDestinationRect.X + (menuDestinationRect.Width * .2f) * 3, menuDestinationRect.Y + 8), Color.White, TextButton.TextAlignment.LEFT);
            battleButton.Initialize(SpriteFont, "Battle", new Vector2(menuDestinationRect.X + (menuDestinationRect.Width * .2f) * 4, menuDestinationRect.Y + 8), Color.White, TextButton.TextAlignment.LEFT);

            // The the signals for pressing the buttons
            Action consumableButtonAction = delegate { OpenSection(SectionOpen.CONSUMABLE); };
            Action equipmentButtonAction = delegate { OpenSection(SectionOpen.EQUIPMENT); };
            Action keyButtonAction = delegate { OpenSection(SectionOpen.KEY); };
            Action battleButtonAction = delegate { OpenSection(SectionOpen.BATTLE); };

            consumableButton.Pressed.Add("inventory_menu", consumableButtonAction);
            equipmentButton.Pressed.Add("inventory_menu", equipmentButtonAction);
            keyButton.Pressed.Add("inventory_menu", keyButtonAction);
            battleButton.Pressed.Add("inventory_menu", battleButtonAction);

            // Set the left and right focus neighors for Tab buttons. Down focus neighbors will be set in Set Item Fields function
            consumableButton.SetFocusNeighbors(null, null, battleButton, equipmentButton);
            equipmentButton.SetFocusNeighbors(null, null, consumableButton, keyButton);
            keyButton.SetFocusNeighbors(null, null, equipmentButton, battleButton);
            battleButton.SetFocusNeighbors(null, null, keyButton, consumableButton);

            CurrentFocused = consumableButton;
            CurrentFocused.onFocusEntered();
        }

        class ItemField
        // These are the actual representations of items in the inventory. Contains a button, amount, weight and ways to use the item.
        {
            public TextButton ItemButton { get; private set; }
            public Label AmountLabel { get; private set; }
            public Label WeightLabel { get; private set; }
            public int Amount { get; set; }
            public float Weight { get; set; }
            public Vector2 Position { get; set; }
            public List<Item> ItemsContained { get; set; } = new List<Item>();
            public Item.ItemType ItemType { get; private set; }
            int itemIndex;
            Item CurrentItem;
            Action _onPressed;
            Action<Entity> _onUseConsumable;
            InventoryMenu inventoryMenu;

            Signal Use = new Signal();

            public ItemField(InventoryMenu _owner, Item _item, int _itemOrderNumber)
            {
                inventoryMenu = _owner;

                ItemButton = new TextButton(_owner);
                AmountLabel = new Label(_owner);
                WeightLabel = new Label(_owner);

                ItemType = _item.Item_Type;
                CurrentItem = _item;
                itemIndex = _itemOrderNumber;

                // Set the onPressed signal in the Text Button for the item
                _onPressed = OnPressed;
                ItemButton.Pressed.Add("item_button", _onPressed);

                SetUseAction(_item);

                Vector2 nameWidth = _owner.SpriteFont.MeasureString(_item.Name);
                Position = new Vector2(_owner.fieldDestinationRect.X, _owner.fieldDestinationRect.Y + (_itemOrderNumber * (nameWidth.Y / Screen.Scale) + (nameWidth.Y / Screen.Scale)));
                ItemButton.Initialize(_owner.SpriteFont, _item.Name, Position, Color.White, TextButton.TextAlignment.LEFT);

                float amountXPosAdjuster = (_owner.fieldDestinationRect.Width * .2f) * 3;
                float weightXPosAdjust = (_owner.fieldDestinationRect.Width * .2f) * 4;

                AmountLabel.Initialize(_owner.SpriteFont, new Vector2(Position.X + amountXPosAdjuster, Position.Y), Amount.ToString(), Color.White);
                WeightLabel.Initialize(_owner.SpriteFont, new Vector2(Position.X + weightXPosAdjust, Position.Y), Weight.ToString(), Color.White);
            }

            public void SetUseAction(Item _item)
            {
                // Set the signal for using a consumable item. This simply links the use function in the Consumable Item class
                if (_item.Item_Type == Item.ItemType.CONSUMABLE)
                {
                    ConsumableItem consumableItem = _item as ConsumableItem;
                    _onUseConsumable = consumableItem.Use;
                    Use.Add("item_field", _onUseConsumable);
                }
            }

            public void Update(float deltaTime)
            {
                ItemButton.Update(deltaTime);
            }

            public void Draw(float deltaTime)
            {
                ItemButton.Draw(deltaTime);
                WeightLabel.Draw(deltaTime);
                AmountLabel.Draw(deltaTime);
            }

            public void SetAmount(int _amount)
            {
                Amount += _amount;
                AmountLabel.Text = Amount.ToString();
            }

            public void SetWeight(float _amount)
            {
                Weight += _amount;
                WeightLabel.Text = Weight.ToString();
            }

            public void OnPressed()
            {
                // TODO: This really shouldn't be called straight away. In time the button will be clicked and a list will pop up.
                // this list will Give multiple opitions. USE, DISPOSE, INSPECT
                // USE - this will call the rest of the function shown below.
                // DISPOSE - this will open a menu with a slider letting you choose how many to throw away.
                // INSPECT - this will open a text box showing stats on the item.
                // These opitions will change depending on item type.

                if (ItemType == Item.ItemType.CONSUMABLE)
                {
                    Use.Emit(inventoryMenu.Player);
                    inventoryMenu.inventory.RemoveItem(CurrentItem);
                    ItemsContained.Remove(CurrentItem);

                    if (ItemsContained.Count > 0)  // If there are more of this type of items left, then just weight and amount variables and labels.
                    {
                        CurrentItem = ItemsContained[0];
                        Weight -= CurrentItem.Weight;
                        Amount--;
                        AmountLabel.Text = Amount.ToString();
                        WeightLabel.Text = Weight.ToString();
                    }
                    else  // There are no more left, so refresh the list to remove the depleted item
                    {
                        inventoryMenu.ClearSection();
                        inventoryMenu.OpenSection(SectionOpen.CONSUMABLE);

                        if (inventoryMenu.currentItemFields.Count > 0) // there are still items left
                        {
                            if (itemIndex < inventoryMenu.currentItemFields.Count) // not the last item, so simply focus the next item
                                inventoryMenu.CurrentFocused = inventoryMenu.currentItemFields[itemIndex].ItemButton;
                            else // the last item, so focus the previous item
                                inventoryMenu.CurrentFocused = inventoryMenu.currentItemFields[itemIndex - 1].ItemButton;
                        }
                        else // there are no items left, focus the consumable tab
                        {
                            inventoryMenu.CurrentFocused = inventoryMenu.consumableButton;
                        }

                        inventoryMenu.CurrentFocused.onFocusEntered();
                    }
                }
            }
        }

        // This is used for forming the list for item fields. It exists so the item lists within the inventory do not have to be altered.
        class InventoryItem
        {
            public Item Item { get; private set; }
            public bool IsAdded { get; set; }

            public InventoryItem(Item _item)
            {
                Item = _item;
            }
        }
    }
}