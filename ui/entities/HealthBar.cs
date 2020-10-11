using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class HealthBar : UIEntity
    {
        ProgressBar progressBar;
        Label currentHealthAmount;
        Label divider;
        Label maxHealthAmount;
        Label healthTitle;

        Vector2 originPosition;

        Player player;
        Stats playerStats;
        SpriteFont spriteFont;

        public HealthBar(Player _player, Vector2 _position)
        {
            player = _player;
            originPosition = _position;

            // Connect signals
            TakeDamage takeDamage = player.GetComponent<TakeDamage>();
            System.Action _onUpdateHealthBar = onUpdateHealthBar;
            takeDamage.TookDamage.Add("health_bar", _onUpdateHealthBar);
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            spriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");

            Texture2D insideTexture = uiEntityManager.ContentManager.Load<Texture2D>("ui/health_bar");
            Texture2D outsideTexture = uiEntityManager.ContentManager.Load<Texture2D>("ui/bar_background");

            progressBar = new ProgressBar(this);
            progressBar.Initialize(insideTexture, outsideTexture, originPosition);

            DestinationRect = new Rectangle((int)originPosition.X, (int)originPosition.Y, outsideTexture.Width, outsideTexture.Height);

            playerStats = player.GetComponent<Stats>();

            healthTitle = new Label(this);
            healthTitle.Initialize(spriteFont, new Vector2(DestinationRect.Width + 2, 0), "Health", Color.White);

            divider = new Label(this);
            divider.Initialize(spriteFont, new Vector2(DestinationRect.Width / 2, 0), "/", Color.White);

            currentHealthAmount = new Label(this);
            currentHealthAmount.Initialize(spriteFont, new Vector2((DestinationRect.Width / 2) - 0, 0), "", Color.White);

            maxHealthAmount = new Label(this);
            maxHealthAmount.Initialize(spriteFont, new Vector2((DestinationRect.Width / 2) + 4, 0), "", Color.White);

            onUpdateHealthBar();
        }

        public override void Draw(float deltaTime)
        {
            progressBar.Draw(deltaTime);
        }

        public override void DrawText(float deltaTime)
        {
            healthTitle.Draw(deltaTime);
            divider.Draw(deltaTime);
            currentHealthAmount.Draw(deltaTime);
            maxHealthAmount.Draw(deltaTime);
        }

        public void onUpdateHealthBar()
        {
            float valueToSet = (float)playerStats.HP / (float)playerStats.MaxHP;
            progressBar.SetValue(valueToSet);

            currentHealthAmount.Text = playerStats.HP.ToString();
            maxHealthAmount.Text = playerStats.MaxHP.ToString();

            currentHealthAmount.SetPosition(new Vector2((DestinationRect.Width / 2) - (spriteFont.MeasureString(currentHealthAmount.Text).X / Screen.Scale) - 2, 0));
        }
    }
}