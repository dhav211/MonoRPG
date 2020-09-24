using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class TextBox : UIEntity
    {
        NineSpliceSprite nineSpliceSprite;
        Label label;
        TextButton button;

        Action _onButtonPressed;

        public TextBox(Rectangle _destinationRect, bool _isScrollable = false) : base(_isScrollable)
        {
            DestinationRect = _destinationRect;
            _onButtonPressed = onButtonPressed;
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            nineSpliceSprite = new NineSpliceSprite(this);
            label = new Label(this);
            button = new TextButton(this);

            if (!IsScrollable)
                GameState.OpenMenu();
        }

        public override void Update(float deltaTime)
        {
            button.Update(deltaTime);
        }

        public override void Draw(float deltaTime)
        {
            nineSpliceSprite.Draw(deltaTime);
        }

        public override void DrawText(float deltaTime)
        {
            label.Draw(deltaTime);
            button.Draw(deltaTime);
        }

        public void CreateGenericTextBox(string _text)
        {
            Texture2D texture = uiEntityManager.ContentManager.Load<Texture2D>("ui/9splicesprite");
            SpriteFont spriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");
            Rectangle labelDestinationRect = new Rectangle(8, 8, DestinationRect.Width - 16, DestinationRect.Height - 16);
            Vector2 textSize = spriteFont.MeasureString(_text);
            Vector2 textButtonPosition = new Vector2(labelDestinationRect.X + (labelDestinationRect.Width / 2), labelDestinationRect.Y + (labelDestinationRect.Height - (textSize.Y / Screen.Scale)));


            CreateNineSpliceSprite(texture, new Rectangle());
            CreateLabel(spriteFont, labelDestinationRect, _text, Color.White);
            CreateButton(spriteFont, "Close", textButtonPosition, Color.White);
        }

        public void CreateNineSpliceSprite(Texture2D _texture, Rectangle _destination)
        {
            nineSpliceSprite.Initialize(_texture, _destination);
        }

        public void CreateLabel(SpriteFont _spriteFont, Rectangle _destinationRect, string _text, Color _color)
        {
            label.Initialize(_spriteFont, _destinationRect, _text, _color);
        }

        public void CreateButton(SpriteFont _spriteFont, string _text, Vector2 _position, Color _color)
        {
            button.Initialize(_spriteFont, _text, _position, _color);
            button.Pressed.Add("text_box", _onButtonPressed);
        }

        public void onButtonPressed()
        {
            Close();
        }

        public void Close()
        {
            if (!IsScrollable)
                GameState.CloseMenu();
            
            uiEntityManager.RemoveEntity<TextBox>(this);
        }
    }
}