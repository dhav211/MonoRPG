using System;
using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public class EnemyAI : Component
    {
        public EnemyAI(Entity _owner) : base(_owner)
        {
            owner.AddComponent<EnemyAI>(this);
        }
    }
}