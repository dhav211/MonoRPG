using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace MonoRPG
{
    public class DamageText : UIEntity
    {
        SpriteFont spriteFont;
        Label label;

        int amount = 0;
        Vector2 position;
        float currentLifetime = 1;

        public DamageText(int _amount, Vector2 _position)
        {
            IsScrollable = true;
            amount = _amount;
            position = _position;
            DestinationRect = new Rectangle((int)position.X, (int)position.Y, 0, 0);
        }

        public override void Initialize(UIEntityManager _uiEntityManager)
        {
            base.Initialize(_uiEntityManager);

            spriteFont = uiEntityManager.ContentManager.Load<SpriteFont>("fonts/m5x7_16");

            Vector2 labelWidth = spriteFont.MeasureString(amount.ToString());

            label = new Label(this);
            label.Initialize(spriteFont, new Rectangle((int)-((labelWidth.X / 2) / Screen.Scale),(int)-(labelWidth.Y / Screen.Scale),(int)labelWidth.X,(int)labelWidth.Y), amount.ToString(), Color.Red, false);
        }

        public override void Update(float deltaTime)
        {
            currentLifetime -= deltaTime;

            if (currentLifetime <= 0)
            {
                uiEntityManager.RemoveEntity<DamageText>(this);
            }
        }

        public override void DrawText(float deltaTime)
        {
            label.Draw(deltaTime);
        }
    }
}