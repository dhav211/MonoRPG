using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoRPG
{
    public class UITexture : UIComponent
    {
        SpriteBatch spriteBatch;
        Texture2D texture;
        Rectangle destination;

        public UITexture(UIEntity _owner) : base(_owner)
        {
            spriteBatch = owner.SpriteBatch;
        }

        public void Initialize(Texture2D _texture, Vector2 _position)
        {
            texture = _texture;
            destination = new Rectangle((int)_position.X, (int)_position.Y, texture.Width, texture.Height);
        }

        public override void Draw(float deltaTime)
        {
            spriteBatch.Draw(texture, destination, Color.White);
        }
    }
}