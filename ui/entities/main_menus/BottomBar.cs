using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class BottomBar : UIEntity
    {
        UITexture bottomBar;
        Texture2D bottomBarTexture;

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            bottomBarTexture = uiEntityManager.ContentManager.Load<Texture2D>("ui/bottom_bar");
            DestinationRect = new Rectangle(0, Screen.Height - bottomBarTexture.Height, Screen.Width, bottomBarTexture.Width);

            bottomBar = new UITexture(this);
            bottomBar.Initialize(bottomBarTexture, new Vector2(DestinationRect.X, DestinationRect.Y));
        }

        public override void Draw(float deltaTime)
        {
            bottomBar.Draw(deltaTime);
        }
    }    
}