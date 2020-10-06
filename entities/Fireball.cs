using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoRPG
{
    public class Fireball : Entity
    {
        ProjectileComponent projectileComponent;
        Transform transform;
        SpriteRenderer sprite;
        AnimationController animation;

        public override void Initialize(EntityManager _entityManager, Vector2 _position = default, Level.LevelEntityValues _entityValues = null)
        {
            base.Initialize(_entityManager);

            projectileComponent = new ProjectileComponent(this);
            transform = new Transform(this);
            sprite = new SpriteRenderer(this, entityManager.SpriteBatch, entityManager.ContentManager.Load<Texture2D>("sprites/fireball"), new Point(16,16));
            animation = new AnimationController(this);

            foreach (Component component in Components)
                component.Initialize();
            
            transform.Position = _position;

            animation.Add("moving", new int[] {0,1,2}, 4);
            animation.Add("explode", new int[] {3,4,5}, 4, false);

            System.Action _onExplodeAnimationComplete = onExplodeAnimationComplete;
            animation.animations["explode"].OnComplete.Add("fireball", _onExplodeAnimationComplete);
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
            animation.Play("explode");
        }

        public void onExplodeAnimationComplete()
        {
            entityManager.RemoveEntity(this);
        }
    }
}