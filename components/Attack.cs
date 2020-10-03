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

        public void DealMagicalDamage(Stats _attackerStats, Stats _targetStats, TakeDamage _targetTakeDamage)
        {
            int attackAmount = (int)Math.Pow(_attackerStats.INT, 2) / _targetStats.RES;
            _targetTakeDamage.DealDamage(attackAmount);
        }
    }
}