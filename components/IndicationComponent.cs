using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoRPG
{
    public class IndicationComponent : Component
    {
        public IndicationComponent(Entity _owner) : base(_owner)
        {
            owner.AddComponent<IndicationComponent>(this);
        }
    }
}