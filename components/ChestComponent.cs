using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public class ChestComponent : Component
    {
        public bool IsLocked { get; private set; }
        public KeyItem KeyRequired { get; private set; }
        public int ChestID { get; private set; }
        public List<Item> ItemsInside { get; private set; } = new List<Item>();
        Inventory inventory;

        public ChestComponent(Entity _owner) : base(_owner)
        {
            owner.AddComponent<ChestComponent>(this);
            
            inventory = owner.entityManager.Inventory;
        }

        public override void Update(float deltaTime) { }

        public void SetValues(bool _isLocked, int _chestID, string _keyRequired)
        {
            IsLocked = _isLocked;
            ChestID = _chestID;
            KeyRequired = SetKeyRequired(_keyRequired);
        }

        public void SetItemsInside(List<Item> _items)
        {
            ItemsInside = _items;
        }

        public void Open()
        {            
            if (!IsLocked)
            {
                if (ItemsInside.Count > 0)
                {
                    TakeLootBox takeLootBox = new TakeLootBox(ItemsInside, owner.entityManager.Inventory, owner);
                    EntityCreator.CreateUIEntity<TakeLootBox>(takeLootBox);
                }
                else
                {
                    Point textBoxSize = new Point(Screen.Width / 4, Screen.Height / 4);
                    Rectangle textBoxDestination = new Rectangle((Screen.Width / 2) - (textBoxSize.X / 2), (Screen.Height / 2) - (textBoxSize.Y / 2), textBoxSize.X, textBoxSize.Y);
                    TextBox textBox = new TextBox(textBoxDestination, false);
                    EntityCreator.CreateUIEntity<TextBox>(textBox);
                    textBox.CreateGenericTextBox("It's not a bottomless chest, and if it was you couldn't ever get the items from it!");
                }
            }
            else
            {
                // Create a new type of text box, it will be a new class called UnlockDialogBox. It will require different parameters from the normal text box.
                // Firstly it will require the type of key required to open the box , a reference of the players inventory, and finally a reference of this chest.
                // When the dialog box is initialized it does a quick loop thru your key items and checks to see if the required key is there.
                // The message and opitions will be different depending if you have the key or not.
                // if you have the key have it be like this
                /*
                You have the Simple Key.
                        Unlock
                        Close
                */
                // if you don't have the key it be like this
                /*
                The simple key is required.
                        Close
                */
                // When unlock is pressed, check if the key is reusable, if not remove. Then Call the chests Unlock function again and close the dialog box.
                // This box will be used for chests and does, so that last bit will require a if/else statment to see if its a door or a chest.
                UnlockDialogBox unlockDialogBox = new UnlockDialogBox(KeyRequired, owner.entityManager.Inventory, this);
                EntityCreator.CreateUIEntity<UnlockDialogBox>(unlockDialogBox);
            }
        }

        public void Unlock()
        {
            IsLocked = false;
            Open();
        }

        private KeyItem SetKeyRequired(string _keyRequired)
        {
            switch (_keyRequired)
            {
                case "SIMPLE_KEY":
                    return new SimpleKey();
            }

            return null;
        }

        public override void Initialize() { }

        ///<summary>
        /// Checks if all required components are added before initialized, will send error message if not all are there
        ///</summary>
        protected override void CheckRequiredComponents() {}
    }
}