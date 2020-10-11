using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class MagicBar : UIEntity
    {
        ProgressBar progressBar;
        Label currentMagicAmount;
        Label divider;
        Label maxMagicAmount;
        Label magicTitle;

        Vector2 originPosition;

        Player player;
        Stats playerStats;
        SpriteFont spriteFont;

        public MagicBar(Player _player, Vector2 _position)
        {
            player = _player;
            originPosition = _position;

            // Connect signals
            SkillsComponent playerSkills = player.GetComponent<SkillsComponent>();
            System.Action _onUpdateMagicBar = onUpdateMagicBar;
            playerSkills.SkillUsed.Add("magic_bar", _onUpdateMagicBar);
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            spriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");

            Texture2D insideTexture = uiEntityManager.ContentManager.Load<Texture2D>("ui/magic_bar");
            Texture2D outsideTexture = uiEntityManager.ContentManager.Load<Texture2D>("ui/bar_background");

            progressBar = new ProgressBar(this);
            progressBar.Initialize(insideTexture, outsideTexture, originPosition);

            DestinationRect = new Rectangle((int)originPosition.X, (int)originPosition.Y, outsideTexture.Width, outsideTexture.Height);

            playerStats = player.GetComponent<Stats>();

            magicTitle = new Label(this);
            magicTitle.Initialize(spriteFont, new Vector2(-(spriteFont.MeasureString("Magic").X / Screen.Scale) - 2, 0), "Magic", Color.White);

            divider = new Label(this);
            divider.Initialize(spriteFont, new Vector2(DestinationRect.Width / 2, 0), "/", Color.White);

            currentMagicAmount = new Label(this);
            currentMagicAmount.Initialize(spriteFont, new Vector2((DestinationRect.Width / 2) - 0, 0), "", Color.White);

            maxMagicAmount = new Label(this);
            maxMagicAmount.Initialize(spriteFont, new Vector2((DestinationRect.Width / 2) + 4, 0), "", Color.White);

            onUpdateMagicBar();
        }

        public override void Draw(float deltaTime)
        {
            progressBar.Draw(deltaTime);
        }

        public override void DrawText(float deltaTime)
        {
            magicTitle.Draw(deltaTime);
            divider.Draw(deltaTime);
            currentMagicAmount.Draw(deltaTime);
            maxMagicAmount.Draw(deltaTime);
        }

        public void onUpdateMagicBar()
        {
            float valueToSet = (float)playerStats.MP / (float)playerStats.MaxMP;
            progressBar.SetValue(valueToSet);

            currentMagicAmount.Text = playerStats.MP.ToString();
            maxMagicAmount.Text = playerStats.MaxMP.ToString();

            currentMagicAmount.SetPosition(new Vector2((DestinationRect.Width / 2) - (spriteFont.MeasureString(currentMagicAmount.Text).X / Screen.Scale) - 2, 0));
        }
    }
}