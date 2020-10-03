using Microsoft.Xna.Framework;
using System;

namespace MonoRPG
{
    public class ProjectileComponent : Component
    {
        Tween tween;
        Vector2 destination;
        SpriteRenderer sprite;
        Transform transform;

        public Signal ReachedTarget { get; private set; }

        public ProjectileComponent(Entity _owner) : base(_owner)
        {
            owner.AddComponent<ProjectileComponent>(this);

            tween = new Tween();
        }

        public override void Initialize()
        {
            sprite = owner.GetComponent<SpriteRenderer>();
            transform = owner.GetComponent<Transform>();

            ReachedTarget = new Signal();

            Action tweenOnCompleteAction = delegate { ReachedTarget.Emit(); };
            tween.OnComplete.Add("projectile_component", tweenOnCompleteAction);

            Action onReachedAction = onReachedTarget;
            ReachedTarget.Add("projectile_component", onReachedAction);
        }

        public override void Update(float deltaTime)
        {
            if (tween.IsRunning())
            {
                transform.Position = tween.TweenVector2(transform.Position, deltaTime);
            }
        }

        public void Fire(Vector2 _start, Vector2 _destination, float _speed)
        {
            destination = _destination;
            transform.Position = _start;
            float distance = Vector2.Distance(_start, _destination);
            float time = distance / _speed;

            tween.SetTween(transform.Position, destination, time, Tween.EaseType.LINEAR);
            tween.Start();
        }

        public void onReachedTarget()
        {
            owner.Kill();
        }
    }
}