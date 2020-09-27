using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoRPG
{
    public class UIEntity
    {
        protected UIEntityManager uiEntityManager;
        public Rectangle DestinationRect { get; set; }
        public bool IsScrollable { get; set; } = false;  // this bool affects with it moves with the camera or stays static on the screen
        public SpriteBatch SpriteBatch { get; private set; }
        public UIComponent CurrentFocused { get; set; }

        public UIEntity(bool _isScrollable = false)
        {
            IsScrollable = _isScrollable;
        }

        public virtual void Update(float deltaTime) 
        { 
            HandleKeyboardFocusInput();
        }
        public virtual void Initialize(UIEntityManager _uiEntityManager) 
        { 
            uiEntityManager = _uiEntityManager;
            SpriteBatch = uiEntityManager.SpriteBatch;
        }
        public virtual void Draw(float deltaTime) { }
        public virtual void DrawText(float deltaTime) { }
        public void Destroy()
        {
            uiEntityManager.RemoveEntity<UIEntity>(this);
        }

        private void HandleKeyboardFocusInput()
        {
            if (CurrentFocused == null)
                return;

            bool up = Input.IsKeyJustPressed(Keys.W) || Input.IsKeyJustPressed(Keys.Up);
            bool down = Input.IsKeyJustPressed(Keys.S) || Input.IsKeyJustPressed(Keys.Down);
            bool left = Input.IsKeyJustPressed(Keys.A) || Input.IsKeyJustPressed(Keys.Left);
            bool right = Input.IsKeyJustPressed(Keys.D) || Input.IsKeyJustPressed(Keys.Right);
            
            if (CurrentFocused.FocusNeighborUp != null && up)
            {
                CurrentFocused.onFocusExited();
                CurrentFocused.FocusNeighborUp.onFocusEntered();
            }
            else if (CurrentFocused.FocusNeighborDown != null && down)
            {
                CurrentFocused.onFocusExited();
                CurrentFocused.FocusNeighborDown.onFocusEntered();
            }
            else if (CurrentFocused.FocusNeighborLeft != null && left)
            {
                CurrentFocused.onFocusExited();
                CurrentFocused.FocusNeighborLeft.onFocusEntered();
            }
            else if (CurrentFocused.FocusNeighborRight != null && right)
            {
                CurrentFocused.onFocusExited();
                CurrentFocused.FocusNeighborRight.onFocusEntered();
            }

            if (Input.IsKeyJustPressed(Keys.Enter))
            {
                CurrentFocused.Pressed.Emit();
            }
        }
    }
}