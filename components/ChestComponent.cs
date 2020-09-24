using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public class ChestComponent : Component
    {
        public bool IsLocked { get; private set; }
        public string KeyRequired { get; private set; }
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
            KeyRequired = _keyRequired;
        }

        public void SetItemsInside(List<Item> _items)
        {
            ItemsInside = _items;
        }

        public void Open()
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

        public override void Initialize() { }

        ///<summary>
        /// Checks if all required components are added before initialized, will send error message if not all are there
        ///</summary>
        protected override void CheckRequiredComponents() {}
    }
}