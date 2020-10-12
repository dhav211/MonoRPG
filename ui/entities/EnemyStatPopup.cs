using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class EnemyStatPopup : UIEntity
    {
        NineSpliceSprite nineSpliceSprite;
        Label nameLabel;
        Label hpLabel;
        Label mpLabel;
        Stats enemyStats;
        SpriteFont spriteFont;
        Vector2 position;

        public EnemyStatPopup(Vector2 _position, Stats _stats)
        {
            position = _position;
            enemyStats = _stats;
            IsScrollable = true;
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            spriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");

            Vector2 nameWidth = spriteFont.MeasureString(enemyStats.Name);
            // this variable will increase the width of the box so it will it be even 16 pixel tiles
            int widthIncreaseAmount = (int)nameWidth.X % 16; // TODO magic number should be replaced with splice size

            // Adjust the width of the name string to be proportional to the screen scale
            int adjustedWidth = (int)nameWidth.X / Screen.Scale;
            int adjustHeight = (int)nameWidth.Y / Screen.Scale;

            // As name indicates, these are the final measurements so more complicated calculations won't have to be done in the forming of the rects
            int finalWidth = adjustedWidth + widthIncreaseAmount + 16;
            int finalHeight = adjustHeight * 3 + 16;
            int xSpawnAdjustment = (finalWidth / 2) - 8;

            DestinationRect = new Rectangle((int)position.X - xSpawnAdjustment, (int)position.Y - finalHeight, finalWidth, finalHeight);

            nineSpliceSprite = new NineSpliceSprite(this);
            nineSpliceSprite.Initialize(uiEntityManager.ContentManager.Load<Texture2D>("ui/9splicesprite"), new Rectangle(0,0, DestinationRect.Width, DestinationRect.Height));

            nameLabel = new Label(this);
            nameLabel.Initialize(spriteFont, new Rectangle(8,8, adjustedWidth, adjustHeight), enemyStats.Name, Color.White, false);

            hpLabel = new Label(this);
            hpLabel.Initialize(spriteFont, new Rectangle(8, 8 + adjustHeight, adjustedWidth, adjustHeight), enemyStats.HP.ToString(), Color.White, false);
        
            mpLabel = new Label(this);
            mpLabel.Initialize(spriteFont, new Rectangle(8, 8 + (adjustHeight * 2), adjustedWidth, adjustHeight), enemyStats.MP.ToString(), Color.White, false);
        }

        public override void Draw(float deltaTime)
        {
            nineSpliceSprite.Draw(deltaTime);
        }

        public override void DrawText(float deltaTime)
        {
            nameLabel.Draw(deltaTime);
            hpLabel.Draw(deltaTime);
            mpLabel.Draw(deltaTime);
        }

        public void SetCurrentHP(int _value)
        {
            hpLabel.Text = _value.ToString();
        }
    }
}