using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoRPG
{
    public class TextButton : UIComponent
    {
        public enum TextAlignment { LEFT, CENTER, RIGHT }
        TextAlignment textAlignment = TextAlignment.CENTER;

        SpriteFont spriteFont;
        SpriteBatch spriteBatch;

        public Rectangle DestinationRect { get; private set; }
        public Vector2 Position { get; private set; }
        Rectangle clickRect;
        public string Text { get; private set; }
        Color color;

        bool isHovered;

        public TextButton(UIEntity _owner) : base(_owner)
        {
            spriteBatch = owner.SpriteBatch;

            Pressed = new Signal();
            FocusEntered = new Signal();
            FocusExited = new Signal();
        }

        public void Initialize(SpriteFont _spriteFont, string _text, Vector2 _position, Color _color, TextAlignment _alignment = TextAlignment.CENTER)
        {
            spriteFont = _spriteFont;
            Text = _text;
            color = _color;
            textAlignment = _alignment;

            float alignmentAdjuster = 0;

            //TODO: Right now this is simply centers the label at its orgin, have an alignment enum that aligns it based up the width of destination rect
            Vector2 textWidth = spriteFont.MeasureString(Text);

            if (textAlignment == TextAlignment.LEFT)
            {
                alignmentAdjuster = 0;
            }
            else if (textAlignment == TextAlignment.CENTER)
            {
                alignmentAdjuster = textWidth.X / 2;
            }
            
            Position = new Vector2(((_position.X + owner.DestinationRect.X) * Screen.Scale) - alignmentAdjuster, (_position.Y + owner.DestinationRect.Y) * Screen.Scale);
            clickRect = new Rectangle((int)Position.X / Screen.Scale, (int)Position.Y / Screen.Scale, (int)textWidth.X / Screen.Scale, (int)textWidth.Y / Screen.Scale);
        }

        public override void Update(float deltaTime)
        {
            HandleMouseInput();
            //HandleKeyboardInput();
        }

        public override void Draw(float deltaTime)
        {
            if (!IsVisible)
                return;

            spriteBatch.DrawString(spriteFont, Text, Position, color);
        }

        private void HandleMouseInput()
        {
            Vector2 mousePosition = new Vector2();

            // If camera effects position of parent then it would effect where the mouse click positions ends up at
            if (owner.IsScrollable)
                mousePosition = Input.GetMouseWorldPosition();
            else
                mousePosition = Input.GetMousePosition();
            
            // Check to see if mouse click is within the clickRect
            if ((mousePosition.X > clickRect.X) && (mousePosition.X < clickRect.X + clickRect.Width) &&
                (mousePosition.Y > clickRect.Y) && (mousePosition.Y < clickRect.Y + clickRect.Height))
            {
                if (!isHovered)
                {
                    onFocusEntered();
                    isHovered = true;
                }
            }
            else
            {
                if (isHovered)
                {
                    isHovered = false;
                    //onFocusExited();
                }
            }

            // Once button is hovered and mouse button is pressed then emit its pressed signal
            if (Input.IsMouseButtonJustPressed(Input.MouseButton.LEFT) && IsFocused)
            {
                Pressed.Emit();
            }
        }

        private void HandleKeyboardInput()
        {
            if (!IsFocused)
                return;
            
            if (FocusNeighborUp != null && Input.IsKeyJustPressed(Keys.W))
            {
                onFocusExited();
                FocusNeighborUp.onFocusEntered();
            }
            else if (FocusNeighborDown != null && Input.IsKeyJustPressed(Keys.S))
            {
                onFocusExited();
                FocusNeighborDown.onFocusEntered();
            }
            else if (FocusNeighborLeft != null && Input.IsKeyJustPressed(Keys.A))
            {
                onFocusExited();
                FocusNeighborLeft.onFocusEntered();
            }
            else if (FocusNeighborRight != null && Input.IsKeyJustPressed(Keys.D))
            {
                onFocusExited();
                FocusNeighborRight.onFocusEntered();
            }

            if (Input.IsKeyJustPressed(Keys.Enter))
            {
                Pressed.Emit();
            }
        }

        public override void onFocusEntered()
        {
            if (owner.CurrentFocused != null)
                owner.CurrentFocused.onFocusExited();

            IsFocused = true;
            color = Color.Yellow;
            owner.CurrentFocused = this;
        }

        public override void onFocusExited()
        {
            if (IsFocused) // if the mouse was over the button as it left the click rect
            {
                // emit mouse exited signal if useful
                color = Color.White;
                IsFocused = false;
            }
        }
    }
}