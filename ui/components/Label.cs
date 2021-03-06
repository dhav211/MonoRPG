using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class Label : UIComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        public Rectangle DestinationRect { get; private set; }
        public Vector2 Position { get; set; }
        private string text;
        public string Text
        {
            get { return text; }
            set
            {
                text = value;

                if (isWordWrapEnabled)
                    text = WrapText();
            }
        }
        Color color;

        bool isWordWrapEnabled = false;

        public Label(UIEntity _owner) : base(_owner)
        {
            spriteBatch = owner.SpriteBatch;
        }

        public void Initialize(SpriteFont _spriteFont, Rectangle _destinationRect, string _text, Color _color, bool _wordWrap = true)
        {
            // This is multipled by screen scale to remain continunity between the upscaled textures and unscaled fonts
            DestinationRect = new Rectangle((_destinationRect.X + owner.DestinationRect.X) * Screen.Scale,
                                            (_destinationRect.Y + owner.DestinationRect.Y) * Screen.Scale, 
                                            _destinationRect.Width * Screen.Scale, 
                                            _destinationRect.Height * Screen.Scale);
            Position = new Vector2(DestinationRect.X, DestinationRect.Y);
            isWordWrapEnabled = _wordWrap;
            spriteFont = _spriteFont;
            Text = _text;
            color = _color;
        }

        public void Initialize(SpriteFont _spriteFont, Vector2 _position, string _text, Color _color)
        {
            Position = new Vector2((_position.X + owner.DestinationRect.X) * Screen.Scale, (_position.Y + owner.DestinationRect.Y) * Screen.Scale);
            spriteFont = _spriteFont;
            Text = _text;
            color = _color;

            Vector2 labelSize = spriteFont.MeasureString(Text);
            DestinationRect = new Rectangle((int)Position.X, (int)Position.Y, (int)labelSize.X, (int)labelSize.Y);
        }
        
        public override void Draw(float deltaTime)
        {
            if (!IsVisible)
                return;

            spriteBatch.DrawString(spriteFont, Text, new Vector2(Position.X, Position.Y), color);
        }

        ///<summary>
        /// Wraps the text of the label to fit into the width of the rectangle's bounds
        ///</summary>
        private string WrapText()
        {
            string wrappedText = "";
            string[] words = Text.Split(' ');
            Vector2 spaceWidth = spriteFont.MeasureString(" ");
            float lineWidth = DestinationRect.Width;
            float currentLineWidth = 0;

            for (int i = 0; i < words.Length; ++i)
            {
                Vector2 wordWidth = spriteFont.MeasureString(words[i]);
                float actualWidth = wordWidth.X + spaceWidth.X;

                if (currentLineWidth + actualWidth <= lineWidth)
                {
                    wrappedText += words[i] + " ";
                    currentLineWidth += actualWidth;
                }
                else
                {
                    wrappedText += "\n" + words[i] + " ";
                    currentLineWidth = actualWidth;
                }
            }

            return wrappedText;
        }

        public void SetPosition(Vector2 _position)
        {
            Position = new Vector2((_position.X + owner.DestinationRect.X) * Screen.Scale, (_position.Y + owner.DestinationRect.Y) * Screen.Scale);
        }
    }
}