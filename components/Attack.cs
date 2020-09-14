using System;

namespace MonoRPG
{
    public class Attack : Component
    {
        public Attack(Entity _owner) : base(_owner)
        {
            owner.AddComponent<Attack>(this);
        }

        public void DealPhysicalDamage(Stats _attackerStats, Stats _targetStats, TakeDamage _targetTakeDamage)
        {
            int attackAmount = (int)Math.Pow(_attackerStats.ATK, 2) / _targetStats.DEF;
            _targetTakeDamage.DealDamage(attackAmount);
        }
    }
}