namespace MonoRPG
{
    public class ChestComponent : Component
    {
        public bool IsLocked { get; private set; }

        // TODO: Changes these to an enum value when inventory is created
        public string ItemInside { get; private set; }
        public string KeyRequired { get; private set; }

        public ChestComponent(Entity _owner) : base(_owner)
        {
            owner.AddComponent<ChestComponent>(this);
        }

        public override void Update(float deltaTime) { }

        public void SetValues(bool _isLocked, string _itemInside, string _keyRequired)
        {
            IsLocked = _isLocked;
            ItemInside = _itemInside;
            KeyRequired = _keyRequired;
        }

        public override void Initialize() { }

        ///<summary>
        /// Checks if all required components are added before initialized, will send error message if not all are there
        ///</summary>
        protected override void CheckRequiredComponents() {}
    }
}