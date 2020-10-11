using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MonoRPG
{

    public class Skill
    {
        public enum SkillState { SELECT_TARGET, EXECUTING, FINISHED, NOT_IN_USE }

        public string Name { get; set; }
        public Texture2D Icon { get; set; }
        public int Cost { get; set; }
        public int CooldownPeriod { get; set; }
        public int CurrentCooldown { get; set; }
        public SkillState State { get; set; }
        public Entity Owner { get; set; }
        protected bool wasJustUsed = false;
        protected LineOfSight lineOfSight;

        public Signal OnComplete { get; set; } = new Signal();
        public Signal OnUsed { get; set; } = new Signal();
        public Signal OnCoolDownFinished { get; set; } = new Signal();

        public Skill(Entity _entity)
        {
            Owner = _entity;
            lineOfSight = new LineOfSight(Owner, Owner.Grid);

            Action _onTurnStarted = onTurnStarted;
            Owner.TurnStarted.Add("skill", _onTurnStarted);

            SkillsComponent ownerSkills = Owner.GetComponent<SkillsComponent>();
            Action _onSkillUsed = delegate { ownerSkills.SkillUsed.Emit(); };
            OnUsed.Add("skill", _onSkillUsed);
        }

        public virtual void Update(float deltaTime) { }
        public virtual void Execute() { }
        public virtual void Initiate() { }

        public Entity GetEntityClicked(Point _mouseGridPosition)
        {
            if (Owner.Grid.IsEntityOcuppyingGridPosition(_mouseGridPosition))
            {
                List<Entity> entitiesInPosition = Owner.Grid.GetEntitiesInGridPosition(_mouseGridPosition);

                foreach (Entity entity in entitiesInPosition)
                {
                    if (entity.IsAlive && entity.HasComponent<TakeDamage>())
                    {
                        return entity;
                    }
                }
            }

            return null;
        }

        public void onTurnStarted()
        {
            if (wasJustUsed)
            {
                wasJustUsed = false;
                return;
            }
            
            if (CurrentCooldown > 0)
            {
                CurrentCooldown--;

                if (CurrentCooldown == 0)
                    OnCoolDownFinished.Emit();
            }
        }
    }
}