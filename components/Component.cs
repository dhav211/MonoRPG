namespace MonoRPG
{
    public class Component
    {
        protected Entity owner;

        public Component(Entity _owner)
        {
            owner = _owner;
        }

        public virtual void Update(float deltaTime) { }

        public virtual void Draw(float deltaTime) { }

        public virtual void Initialize() { }

        public virtual void PostInitialize() { }

        ///<summary>
        /// Checks if all required components are added before initialized, will send error message if not all are there
        ///</summary>
        protected virtual void CheckRequiredComponents() {}
    }
}