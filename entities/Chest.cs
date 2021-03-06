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
        InteractionComponent interactionComponent;

        public override void Initialize(EntityManager _entityManager, Vector2 _position, Level.LevelEntityValues _entityValues = null)
        {
            base.Initialize(_entityManager);

            transform = new Transform(this);
            spriteRenderer = new SpriteRenderer(this, entityManager.SpriteBatch, entityManager.ContentManager.Load<Texture2D>("sprites/temp_chest"), new Point(16,16));
            chestComponent = new ChestComponent(this);
            interactionComponent = new InteractionComponent(this);

            foreach (Component component in Components)
                component.Initialize();

            transform.Position = _position;
            SetGridPosition(transform, new Point((int)Math.Floor(transform.Position.X) / 16, (int)Math.Floor(transform.Position.Y / 16)));
            chestComponent.SetValues(_entityValues.isLocked, _entityValues.chest_id, _entityValues.keyRequired);

            interactionComponent.MainInteraction = InteractionComponent.InteractionType.OPEN_CHEST;
            interactionComponent.Interactions.Add(InteractionComponent.InteractionType.OPEN_CHEST);
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