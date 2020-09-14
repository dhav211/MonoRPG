using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class TextButton : UIComponent
    {
        SpriteFont spriteFont;
        SpriteBatch spriteBatch;
        Input input;

        public Rectangle DestinationRect { get; private set; }
        public Vector2 Position { get; private set; }
        Rectangle clickRect;
        public string Text { get; private set; }
        Color color;

        bool isHovered = false;

        public Signal Pressed { get; private set; }
        public Signal MouseEntered { get; private set; }
        public Signal MouseExited { get; private set; }

        public TextButton(UIEntity _owner) : base(_owner)
        {
            spriteBatch = owner.SpriteBatch;

            Pressed = new Signal();
            MouseEntered = new Signal();
            MouseExited = new Signal();
        }

        public void Initialize(SpriteFont _spriteFont, string _text, Vector2 _position, Color _color)
        {
            spriteFont = _spriteFont;
            Text = _text;
            color = _color;
            input = new Input();

            //TODO: Right now this is simply centers the label at its orgin, have an alignment enum that aligns it based up the width of destination rect
            Vector2 textWidth = spriteFont.MeasureString(Text);
            
            Position = new Vector2(((_position.X + owner.DestinationRect.X) * Screen.Scale) - (int)(textWidth.X / 2), (_position.Y + owner.DestinationRect.Y) * Screen.Scale);
            clickRect = new Rectangle((int)Position.X / Screen.Scale, (int)Position.Y / Screen.Scale, (int)textWidth.X, (int)textWidth.Y);
        }

        public override void Update(float deltaTime)
        {
            Vector2 mousePosition = new Vector2();

            // If camera effects position of parent then it would effect where the mouse click positions ends up at
            if (owner.IsScrollable)
                mousePosition = input.GetMouseWorldPosition();
            else
                mousePosition = input.GetMousePosition();
            
            // Check to see if mouse click is within the clickRect
            if ((mousePosition.X > clickRect.X) && (mousePosition.X < clickRect.X + clickRect.Width) &&
                (mousePosition.Y > clickRect.Y) && (mousePosition.Y < clickRect.Y + clickRect.Height))
            {
                isHovered = true;
                // emit mouse entered signal if useful
            }
            else
            {
                if (isHovered) // if the mouse was over the button as it left the click rect
                {
                    // emit mouse exited signal if useful
                }

                isHovered = false;
            }

            // Once button is hovered and mouse button is pressed then emit its pressed signal
            if (input.IsMouseButtonJustPressed(Input.MouseButton.LEFT) && isHovered)
            {
                Pressed.Emit();
            }
        }

        public override void Draw(float deltaTime)
        {
            spriteBatch.DrawString(spriteFont, Text, Position, color);
        }
    }
}