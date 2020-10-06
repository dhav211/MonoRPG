using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class IndicationArea : Entity
    {
        Transform transform;
        SpriteRenderer spriteRenderer;
        IndicationComponent indicationComponent;

        public override void Initialize(EntityManager _entityManager, Vector2 _position = default, Level.LevelEntityValues _entityValues = null)
        {
            base.Initialize(_entityManager);

            transform = new Transform(this);
            spriteRenderer = new SpriteRenderer(this, entityManager.SpriteBatch, entityManager.ContentManager.Load<Texture2D>("sprites/damage_square"), new Point(16,16));
            indicationComponent = new IndicationComponent(this);

            foreach (Component component in Components)
                component.Initialize();
            
            transform.Position = _position;
            SetGridPosition(transform, new Point((int)Math.Floor(transform.Position.X) / 16, (int)Math.Floor(transform.Position.Y / 16)));

            IsWalkable = true;
        }

        public override void Draw(float deltaTime)
        {
            spriteRenderer.Draw(deltaTime);
        }

        public override void Kill()
        {
            entityManager.RemoveEntity(this);
        }
    }
}