using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    class Molotov : Entity
    {
        Transform transform;
        SpriteRenderer sprite;
        AnimationController animation;

        public override void Initialize(EntityManager _entityManager, Vector2 _position = default, Level.LevelEntityValues _entityValues = null)
        {
            base.Initialize(_entityManager);

            transform = new Transform(this);
            sprite = new SpriteRenderer(this, entityManager.SpriteBatch, entityManager.ContentManager.Load<Texture2D>("sprites/fire_spread"), new Point(16,16));
            animation = new AnimationController(this);

            foreach (Component component in Components)
                component.Initialize();

            transform.Position = _position;
            SetGridPosition(transform, new Point((int)Math.Floor(transform.Position.X) / 16, (int)Math.Floor(transform.Position.Y / 16)));

            animation.Add("burn", new int[] {0,1,2,3,4,3,2,1,0}, 6, false);
            Action _onBurnAnimationComplete = onBurnAnimationComplete;
            animation.Animations["burn"].OnComplete.Add("molotov", _onBurnAnimationComplete);

            IsWalkable = true;
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
            Destroy.Emit();
            entityManager.RemoveEntity(this);
        }

        public void onBurnAnimationComplete()
        {
            Kill();
        }
    }
}