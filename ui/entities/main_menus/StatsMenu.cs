using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class StatsMenu : UIEntity
    {
        NineSpliceSprite nineSpliceSprite;
        Label name;
        Label hpText;
        Label hpAmount;
        Label mpText;
        Label mpAmount;
        Label lvlText;
        Label lvlAmount;
        Label expText;
        Label expAmount;
        Label nextText;
        Label nextAmount;
        Label atkText;
        Label atkAmount;
        Label defText;
        Label defAmount;
        Label intText;
        Label intAmount;
        Label resText;
        Label resAmount;
        Label lukText;
        Label lukAmount;
        Label spdText;
        Label spdAmount;

        Rectangle fieldDestinationRect;
        Rectangle leftSideRect;
        Rectangle rightSideRect;
        Rectangle firstPanel;
        Rectangle secondPanel;
        Rectangle thirdPanel;
        Rectangle fourthPanel;

        Stats playerStats;
        SpriteFont spriteFont;

        public StatsMenu(EntityManager _entityManager)
        {
            IsScrollable = false;
            GameState.OpenMenu();

            Player player = _entityManager.GetEntityOfType<Player>();
            playerStats = player.GetComponent<Stats>();

            int menuHeight = 90;
            DestinationRect = new Rectangle((Screen.Width / 2) - ((Screen.Width / 4) / 2), (Screen.Height / 2) - (menuHeight / 2), Screen.Width / 4, menuHeight);
            fieldDestinationRect = new Rectangle(DestinationRect.X + 8, DestinationRect.Y + 24, DestinationRect.Width - 16, DestinationRect.Height - 16);
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            spriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");

            // set panel rectangles
            float stringHeight = spriteFont.MeasureString("MeasureThis").Y / Screen.Scale;
            float firstPanelWidth = spriteFont.MeasureString("NEXT ").X / Screen.Scale;
            float secondPanelWidth = spriteFont.MeasureString("6969/6969     ").X / Screen.Scale;
            float thirdPanelWidth = spriteFont.MeasureString("ATK ").X / Screen.Scale;
            float fourthPanelWidth = (fieldDestinationRect.Width / 2) - secondPanelWidth;

            int panelXPosition = fieldDestinationRect.X - DestinationRect.X;
            int initalYPostion = (int)stringHeight * 2 + 8;

            firstPanel = new Rectangle(panelXPosition, initalYPostion, (int)firstPanelWidth, fieldDestinationRect.Height);
            panelXPosition += (int)firstPanelWidth;

            secondPanel = new Rectangle(panelXPosition, initalYPostion, (int)secondPanelWidth, fieldDestinationRect.Height);
            panelXPosition += (int)secondPanelWidth;

            thirdPanel = new Rectangle(panelXPosition, initalYPostion, (int)thirdPanelWidth, fieldDestinationRect.Y);
            panelXPosition += (int)thirdPanelWidth;

            fourthPanel = new Rectangle(panelXPosition, initalYPostion, (int)fourthPanelWidth, fieldDestinationRect.Y);

            // create nine splice sprite
            nineSpliceSprite = new NineSpliceSprite(this);
            nineSpliceSprite.Initialize(uiEntityManager.ContentManager.Load<Texture2D>("ui/9splicesprite"), new Rectangle());


            // initalize all labels
            float nameWidth = (spriteFont.MeasureString("Player").X / Screen.Scale) / 2;
            float currentYPos = 4;

            name = new Label(this);
            name.Initialize(spriteFont, new Vector2(DestinationRect.Width / 2 - nameWidth, currentYPos), "Player", Color.White);

            lvlText = new Label(this);
            string lvlString = "LVL " + "1";
            float lvlWidth = (spriteFont.MeasureString(lvlString).X / Screen.Scale) / 2;
            currentYPos += stringHeight;
            lvlText.Initialize(spriteFont, new Vector2(DestinationRect.Width / 2 - lvlWidth, currentYPos), lvlString, Color.White);

            // first row stats
            currentYPos = firstPanel.Y;
            hpText = new Label(this);
            hpText.Initialize(spriteFont, new Vector2(firstPanel.X, currentYPos), "HP", Color.White);

            hpAmount = new Label(this);
            hpAmount.Initialize(spriteFont, new Vector2(secondPanel.X, currentYPos), playerStats.HP + " / " + playerStats.MaxHP, Color.White);

            atkText = new Label(this);
            atkText.Initialize(spriteFont, new Vector2(thirdPanel.X, currentYPos), "ATK", Color.White);

            atkAmount = new Label(this);
            atkAmount.Initialize(spriteFont, new Vector2(fourthPanel.X, currentYPos), playerStats.ATK.ToString(), Color.White);

            currentYPos += stringHeight;

            // second row stats
            mpText = new Label(this);
            mpText.Initialize(spriteFont, new Vector2(firstPanel.X, currentYPos), "MP", Color.White);

            mpAmount = new Label(this);
            mpAmount.Initialize(spriteFont, new Vector2(secondPanel.X, currentYPos), playerStats.MP + " / " + playerStats.MaxMP, Color.White);

            defText = new Label(this);
            defText.Initialize(spriteFont, new Vector2(thirdPanel.X, currentYPos), "DEF", Color.White);

            defAmount = new Label(this);
            defAmount.Initialize(spriteFont, new Vector2(fourthPanel.X, currentYPos), playerStats.DEF.ToString(), Color.White);

            currentYPos += stringHeight;

            // third row stats
            
            // TODO add status icons here. like for poision and even buffs and debuffs
            // The icons should actually be centered between the two rows to kinda fill in the blanks space and make all look more centered

            intText = new Label(this);
            intText.Initialize(spriteFont, new Vector2(thirdPanel.X, currentYPos), "INT", Color.White);

            intAmount = new Label(this);
            intAmount.Initialize(spriteFont, new Vector2(fourthPanel.X, currentYPos), playerStats.INT.ToString(), Color.White);

            currentYPos += stringHeight;

            // fourth row stats
            resText = new Label(this);
            resText.Initialize(spriteFont, new Vector2(thirdPanel.X, currentYPos), "RES", Color.White);

            resAmount = new Label(this);
            resAmount.Initialize(spriteFont, new Vector2(fourthPanel.X, currentYPos), playerStats.RES.ToString(), Color.White);

            currentYPos += stringHeight;

            // fifth row stats
            expText = new Label(this);
            expText.Initialize(spriteFont, new Vector2(firstPanel.X, currentYPos), "EXP", Color.White);

            expAmount = new Label(this);
            expAmount.Initialize(spriteFont, new Vector2(secondPanel.X, currentYPos), playerStats.EXP.ToString(), Color.White);

            spdText = new Label(this);
            spdText.Initialize(spriteFont, new Vector2(thirdPanel.X, currentYPos), "SPD", Color.White);

            spdAmount = new Label(this);
            spdAmount.Initialize(spriteFont, new Vector2(fourthPanel.X, currentYPos), playerStats.SPD.ToString(), Color.White);

            currentYPos += stringHeight;

            // fifth row stats
            nextText = new Label(this);
            nextText.Initialize(spriteFont, new Vector2(firstPanel.X, currentYPos), "NEXT", Color.White);

            // TODO get next lvl amount by subtracting total experience required for next lvl by current experience
            nextAmount = new Label(this);
            nextAmount.Initialize(spriteFont, new Vector2(secondPanel.X, currentYPos), "69", Color.White);

            lukText = new Label(this);
            lukText.Initialize(spriteFont, new Vector2(thirdPanel.X, currentYPos), "LUK", Color.White);

            lukAmount = new Label(this);
            lukAmount.Initialize(spriteFont, new Vector2(fourthPanel.X, currentYPos), playerStats.LUK.ToString(), Color.White);

            currentYPos += stringHeight; // 83
        }

        public override void Draw(float deltaTime)
        {
            nineSpliceSprite.Draw(deltaTime);
        }

        public override void DrawText(float deltaTime)
        {
            name.Draw(deltaTime);
            lvlText.Draw(deltaTime);
            hpText.Draw(deltaTime);
            hpAmount.Draw(deltaTime);
            mpText.Draw(deltaTime);
            mpAmount.Draw(deltaTime);
            expText.Draw(deltaTime);
            expAmount.Draw(deltaTime);
            nextText.Draw(deltaTime);
            nextAmount.Draw(deltaTime);
            atkText.Draw(deltaTime);
            atkAmount.Draw(deltaTime);
            defText.Draw(deltaTime);
            defAmount.Draw(deltaTime);
            intText.Draw(deltaTime);
            intAmount.Draw(deltaTime);
            resText.Draw(deltaTime);
            resAmount.Draw(deltaTime);
            spdText.Draw(deltaTime);
            spdAmount.Draw(deltaTime);
            lukText.Draw(deltaTime);
            lukAmount.Draw(deltaTime);
        }

        ///<summary>
        /// Remove from UI Entity Manager so it will no longer be updated or drawn
        ///</summary>
        public void Close()
        {
            GameState.CloseMenu();
            uiEntityManager.RemoveEntity<StatsMenu>(this);
        }
    }
}