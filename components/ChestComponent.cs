using System.Collections.Generic;

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
            // TODO: eventually this will open a menu that will let you choose which items you want to put in inventory. Now just put them all in there.
            
            if (ItemsInside.Count == 0)  // have a text box pop up that says the chest is empty
                return;

            foreach (Item item in ItemsInside)
            {
                inventory.AddItem(item);
            }

            ItemsInside.Clear();
        }

        public override void Initialize() { }

        ///<summary>
        /// Checks if all required components are added before initialized, will send error message if not all are there
        ///</summary>
        protected override void CheckRequiredComponents() {}
    }
}