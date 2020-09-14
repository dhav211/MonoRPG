using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class Chest : Entity
    {
        Transform transform;
        SpriteRenderer spriteRenderer;
        ChestComponent chestComponent; // TODO: later on this might need to be a public get

        public override void Initialize(EntityManager _entityManager, Vector2 _position, Level.LevelEntityValues _entityValues = null)
        {
            base.Initialize(_entityManager);

            transform = new Transform(this);
            spriteRenderer = new SpriteRenderer(this, entityManager.SpriteBatch, entityManager.ContentManager.Load<Texture2D>("sprites/temp_chest"), new Point(16,16));
            chestComponent = new ChestComponent(this);

            foreach (Component component in Components)
                component.Initialize();

            transform.Position = _position;
            chestComponent.SetValues(_entityValues.isLocked, _entityValues.itemInside, _entityValues.keyRequired);

            IsAlive = true;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw(float deltaTime)
        {
            spriteRenderer.Draw(deltaTime);
        }
    }
}