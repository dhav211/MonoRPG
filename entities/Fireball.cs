using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class Fireball : Entity
    {
        ProjectileComponent projectileComponent;
        Transform transform;
        SpriteRenderer sprite;

        public override void Initialize(EntityManager _entityManager, Vector2 _position = default, Level.LevelEntityValues _entityValues = null)
        {
            base.Initialize(_entityManager);

            projectileComponent = new ProjectileComponent(this);
            transform = new Transform(this);
            sprite = new SpriteRenderer(this, entityManager.SpriteBatch, entityManager.ContentManager.Load<Texture2D>("ui/fireball_icon"), new Point(16,16));

            foreach (Component component in Components)
                component.Initialize();
            
            transform.Position = _position;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw(float deltaTime)
        {
            sprite.Draw(deltaTime);
        }

        public override void Kill()
        {
            // Play explosion animation here when you got one
            // this could also be an async deal, so when animation is done, then kill
            entityManager.RemoveEntity(this);
        }
    }
}