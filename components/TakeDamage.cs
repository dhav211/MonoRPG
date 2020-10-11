using Microsoft.Xna.Framework;
using System;

namespace MonoRPG
{
    public class TakeDamage : Component
    {
        Stats stats;
        public Signal TookDamage { get; private set; } = new Signal();

        public TakeDamage (Entity _owner) : base(_owner)
        {
            owner.AddComponent<TakeDamage>(this);
        }

        public void DealDamage(int _amount)
        {
            // TODO: spawn a damage amount text that floats up in the air
            stats.HP -= _amount;

            Transform ownerTransform = owner.GetComponent<Transform>() as Transform;
            DamageText damageText = new DamageText(_amount, new Vector2(ownerTransform.Position.X + (owner.Size.X / 2), ownerTransform.Position.Y));
            EntityCreator.CreateUIEntity(damageText);

            TookDamage.Emit();
            
            if (stats.HP <= 0)
            {
                owner.Kill();
            }
        }

        public override void Initialize() 
        {
            // TODO: Check for required components
            stats = owner.GetComponent<Stats>() as Stats;
        }
    }
}