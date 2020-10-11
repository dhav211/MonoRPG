using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace MonoRPG
{
    public class SkillBar : UIEntity
    {
        Player player;
        SkillsComponent playerSkills;
        Stats playerStats;

        public SpriteFont SpriteFont {get ; private set; }
        public SkillButton[] SkillButtons { get; private set; } = new SkillButton[10];

        public SkillBar(Entity _player)
        {
            player = _player as Player;
            playerSkills = player.GetComponent<SkillsComponent>();
            playerStats = player.GetComponent<Stats>();
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            SpriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");

            SpawnSkillButtons();
            SetInitalSkills();
        }

        public override void Draw(float deltaTime)
        {
            for (int i = 0; i < SkillButtons.Length; ++i)
            {
                SkillButtons[i].Draw(deltaTime);
            }
        }

        public override void DrawText(float deltaTime)
        {
            for (int i = 0; i < SkillButtons.Length; ++i)
            {
                SkillButtons[i].DrawText(deltaTime);
            }
        }

        public override void Update(float deltaTime)
        {
            for (int i = 0; i < SkillButtons.Length; ++i)
            {
                SkillButtons[i].Update(deltaTime);
            }
        }

        private void SpawnSkillButtons()
        {
            Texture2D skillButtonTexture = uiEntityManager.ContentManager.Load<Texture2D>("ui/skill_ui_box");

            int skillButtonSpacing = 8;
            int numberOfSkillButtons = 10;
            int totalSkillBarsWidth = (skillButtonTexture.Width * numberOfSkillButtons) + (skillButtonSpacing * (numberOfSkillButtons - 1));
            int sideBufferWidth = (Screen.Width - totalSkillBarsWidth) / 2;
            int ySpawnPos = Screen.Height - skillButtonTexture.Height;
            int currentXSpawnPos = sideBufferWidth;
            int xSpawnIncreaseAmount = skillButtonTexture.Width + skillButtonSpacing;

            for (int i = 0; i < SkillButtons.Length; ++i)
            {
                Rectangle skillButtonDestinationRect = new Rectangle(currentXSpawnPos, ySpawnPos, skillButtonTexture.Width, skillButtonTexture.Height);
                SkillButton skillButton = new SkillButton(skillButtonDestinationRect, skillButtonTexture, this);
                SkillButtons[i] = skillButton;

                Action _onTurnStarted = skillButton.onTurnStarted;
                player.TurnStarted.Add("skill_button", _onTurnStarted);

                currentXSpawnPos += xSpawnIncreaseAmount;
            }
        }

        private void SetInitalSkills()
        {
            for (int i = 0; i < SkillButtons.Length; ++i)
            {
                if (playerSkills.HotkeySkills[i] != null)
                {
                    SkillButtons[i].SetSkillButton(playerSkills.HotkeySkills[i]);
                }
            }
        }

        public void SetUsableWithCurrentMP()
        {
            foreach(SkillButton skillButton in SkillButtons)
            {
                if (skillButton.SetSkill == null)
                    continue;

                if (skillButton.SetSkill.Cost <= playerStats.MP)
                {
                    skillButton.isNotEnoughMP = false;
                }
                else
                {
                    skillButton.isNotEnoughMP = true;
                }
            }
        }

        public class SkillButton
        {
            public TextureButton Button { get; set; }
            public Skill SetSkill { get; set; }
            public UITexture SkillBoxTexture { get; set; }
            public Rectangle Destination { get; set; }
            public SkillBar SkillBar { get; set; }
            Label cooldownPeriod;
            UITexture cooldownCover;
            Texture2D cooldownCoverTexture;
            bool isButtonInitialized = false;
            bool isCoolingDown = false;
            public bool isNotEnoughMP { get; set; } = false;

            public SkillButton(Rectangle _destination, Texture2D _textureOfSkillBox, SkillBar _skillBar)
            {
                Destination = _destination;
                SkillBar = _skillBar;

                Button = new TextureButton(SkillBar);
                SkillBoxTexture = new UITexture(SkillBar);

                SkillBoxTexture.Initialize(_textureOfSkillBox, new Vector2(Destination.X, Destination.Y));

                cooldownCoverTexture = _skillBar.uiEntityManager.ContentManager.Load<Texture2D>("ui/skill_cooldown_cover");

                cooldownPeriod = new Label(SkillBar);
                cooldownPeriod.Initialize(SkillBar.SpriteFont, new Rectangle(Destination.X + 10, Destination.Y + 8, 16, 16), "", Color.White, false);

                cooldownCover = new UITexture(SkillBar);
                cooldownCover.Initialize(cooldownCoverTexture, new Vector2(Destination.X  + 4, Destination.Y + 4));
            }

            public void SetSkillButton(Skill _skill)
            {
                SetSkill = _skill;

                if (!isButtonInitialized)
                {
                    Button.Initialize(SetSkill.Icon, new Vector2(Destination.X + 4, Destination.Y + 4));
                    isButtonInitialized = true;
                }
                else
                {
                    Button.Texture = SetSkill.Icon;
                }

                Action skillButtonPressedAction = SetSkill.Execute;
                Button.Pressed.Add("skill_button", skillButtonPressedAction);

                SetSkill.OnUsed.RemoveListener("skill_button");
                Action _onUsed = onSkillUsed;
                SetSkill.OnUsed.Add("skill_button", _onUsed);

                SetSkill.OnCoolDownFinished.RemoveListener("skill_button");
                Action _onCoolDownFinished = onCooldownFinished;
                SetSkill.OnCoolDownFinished.Add("skill_button", _onCoolDownFinished);
            }

            public void Update(float deltaTime)
            {
                if (isButtonInitialized && !isCoolingDown && !isNotEnoughMP)
                    Button.Update(deltaTime);
            }

            public void Draw(float deltaTime)
            {
                SkillBoxTexture.Draw(deltaTime);

                if (isButtonInitialized)
                    Button.Draw(deltaTime);
                
                if (isCoolingDown || isNotEnoughMP)
                    cooldownCover.Draw(deltaTime);
            }

            public void DrawText(float deltaTime)
            {
                if (isCoolingDown)
                    cooldownPeriod.Draw(deltaTime);
            }

            public void onSkillUsed()
            {
                if (SetSkill.CurrentCooldown > 0)
                {
                    isCoolingDown = true;
                    cooldownPeriod.Text = SetSkill.CurrentCooldown.ToString();
                }

                SkillBar.SetUsableWithCurrentMP();
            }

            public void onCooldownFinished()
            {
                isCoolingDown = false;
            }

            public void onTurnStarted()
            {
                if (isCoolingDown)
                {
                    cooldownPeriod.Text = SetSkill.CurrentCooldown.ToString();
                }
            }
        }
    }
}