namespace MonoRPG
{
    public class UIComponent
    {
        protected UIEntity owner;

        public UIComponent(UIEntity _owner)
        {
            owner = _owner;
        }

        public virtual void Update(float deltaTime) { }
        public virtual void Draw(float deltaTime) { }
    }
}