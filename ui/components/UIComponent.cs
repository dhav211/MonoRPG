namespace MonoRPG
{
    public class UIComponent
    {
        protected UIEntity owner;
        public bool IsVisible { get; set; } = true;
        public bool IsFocused { get; set; } = false;

        public Signal Pressed { get; set; }
        public Signal FocusEntered { get; set; }
        public Signal FocusExited { get; set; }

        public UIComponent FocusNeighborUp { get; set; }
        public UIComponent FocusNeighborDown { get; set; }
        public UIComponent FocusNeighborLeft { get; set; }
        public UIComponent FocusNeighborRight { get; set; }

        public UIComponent(UIEntity _owner)
        {
            owner = _owner;
        }

        public virtual void Update(float deltaTime) { }
        public virtual void Draw(float deltaTime) { }
        public virtual void onFocusEntered() { }
        public virtual void onFocusExited() { }

        public void SetFocusNeighbors(UIComponent _focusNeighborUp, UIComponent _focusNeighborDown, UIComponent _focusNeighborLeft, UIComponent _focusNeighborRight)
        {
            FocusNeighborUp = _focusNeighborUp;
            FocusNeighborDown = _focusNeighborDown;
            FocusNeighborLeft = _focusNeighborLeft;
            FocusNeighborRight = _focusNeighborRight;
        }
    }
}