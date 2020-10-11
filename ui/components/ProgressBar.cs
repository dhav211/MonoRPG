using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class ProgressBar : UIComponent
    {
        SpriteBatch spriteBatch;
        Texture2D insideTexture;
        Texture2D outsideTexture;
        float currentProgress;
        int maxProgressWidth;
        Rectangle insideDestination;
        Rectangle insideSource;
        Rectangle outsideDestination;

        public ProgressBar(UIEntity _owner) : base(_owner)
        {
            spriteBatch = owner.SpriteBatch;
        }

        public void Initialize(Texture2D _insideTexture, Texture2D _outsideTexture, Vector2 _position)
        {
            outsideTexture = _outsideTexture;
            outsideDestination = new Rectangle((int)_position.X, (int)_position.Y, outsideTexture.Width, outsideTexture.Height);

            insideTexture = _insideTexture;
            insideDestination = new Rectangle((int)_position.X, (int)_position.Y, insideTexture.Width, insideTexture.Height);
            insideSource = new Rectangle(0, 0, insideTexture.Width, insideTexture.Height);

            maxProgressWidth = insideDestination.Width;
        }

        public override void Update(float deltaTime)
        {
            // update the tween here when you have that feature in place
        }

        public override void Draw(float deltaTime)
        {
            spriteBatch.Draw(outsideTexture, outsideDestination, Color.White);
            spriteBatch.Draw(insideTexture, insideDestination, insideSource, Color.White);

        }

        public void SetValue(float _value)
        {
            currentProgress = System.Math.Clamp(_value, 0f, 1f);

            insideSource = new Rectangle(insideSource.X, insideSource.Y, (int)(maxProgressWidth * currentProgress), insideTexture.Height);
            insideDestination = new Rectangle(insideDestination.X, insideDestination.Y, insideSource.Width, insideDestination.Height);

            // TODO eventually this value will be tweened to make a more visually appealing progress bar. At this point it just instantly changes
        }
    }
}