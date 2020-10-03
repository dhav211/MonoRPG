using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoRPG
{
    public class TextureButton : UIComponent
    {
        SpriteBatch spriteBatch;
        public Texture2D Texture { get; set; }
        public Rectangle Destination { get; set; }

        public TextureButton(UIEntity _owner) : base(_owner)
        {
            spriteBatch = owner.SpriteBatch;
        }

        public void Initialize(Texture2D _texture, Vector2 _position)
        {
            Texture = _texture;
            Pressed = new Signal();
            Destination = new Rectangle((int)_position.X, (int)_position.Y, Texture.Width, Texture.Height);
        }

        public override void Update(float deltaTime)
        {
            Vector2 mousePosition = Input.GetMousePosition();

            if ((mousePosition.X > Destination.X) && (mousePosition.X < Destination.X + Destination.Width) &&
                (mousePosition.Y > Destination.Y) && (mousePosition.Y < Destination.Y + Destination.Height))
            {
                if (Input.IsMouseButtonJustPressed(Input.MouseButton.LEFT))
                {
                    Pressed.Emit();
                }
            }
        }

        public override void Draw(float deltaTime)
        {
            spriteBatch.Draw(Texture, Destination, Color.White);
        }
    }
}