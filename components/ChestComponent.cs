using System.Collections.Generic;

namespace MonoRPG
{
    public class ChestComponent : Component
    {
        public bool IsLocked { get; private set; }

        // TODO: Changes these to an enum value when inventory is created
        public string KeyRequired { get; private set; }
        public int ChestID { get; private set; }
        public List<Item> ItemsInside { get; private set; } = new List<Item>();

        public ChestComponent(Entity _owner) : base(_owner)
        {
            owner.AddComponent<ChestComponent>(this);
        }

        public override void Update(float deltaTime) { }

        public void SetValues(bool _isLocked, int _chestID, string _keyRequired)
        {
            IsLocked = _isLocked;
            ChestID = _chestID;
            KeyRequired = _keyRequired;
        }

        public override void Initialize() { }

        ///<summary>
        /// Checks if all required components are added before initialized, will send error message if not all are there
        ///</summary>
        protected override void CheckRequiredComponents() {}
    }
}