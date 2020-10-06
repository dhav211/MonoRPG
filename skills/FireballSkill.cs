using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class FireballSkill : Skill
    {
        public FireballSkill(Entity _entity) : base(_entity)
        {
            Name = "Fireball";
            Cost = 5;
            CooldownPeriod = 3;
            State = SkillState.NOT_IN_USE;

            Icon = _entity.entityManager.ContentManager.Load<Texture2D>("ui/fireball_icon");
            
            OnUsed = new Signal();
            OnCoolDownFinished = new Signal();
        }

        public override void Update(float deltaTime)
        {
            if (State == SkillState.SELECT_TARGET)
            {
                SelectTarget();
            }
        }

        public override void Execute()
        {
            State = SkillState.SELECT_TARGET;

            if (Owner is Player)
            {
                PlayerController playerController = Owner.GetComponent<PlayerController>();

                playerController.CurrentState = PlayerController.State.USING_SKILL;
            }
        }

        public override void Initiate()
        {
            State = SkillState.SELECT_TARGET;
        }

        private void SelectTarget()
        {
            Point mouseGridPosition = Input.GetMouseGridPosition();

            if (Input.IsMouseButtonJustPressed(Input.MouseButton.LEFT))
            {
                // Spawn fire ball and send it to target.
                Fireball fireball = new Fireball();
                Transform ownerTransform = Owner.GetComponent<Transform>();
                Entity target = GetEntityClicked(mouseGridPosition);
                EntityCreator.CreateEntity<Fireball>(fireball, ownerTransform.Position);
                ProjectileComponent projectile = fireball.GetComponent<ProjectileComponent>();

                if (target != null)
                {
                    if (lineOfSight.IsLineToTargetClear(target))
                    {
                        Transform targetTransform = target.GetComponent<Transform>();
                        projectile.Fire(ownerTransform.Position, targetTransform.Position, 120);
                    }
                    else
                    {
                        target = null;
                        Point collisionPoint = lineOfSight.GetCollisionPoint(mouseGridPosition);
                        projectile.Fire(ownerTransform.Position, new Vector2(collisionPoint.X * 16, collisionPoint.Y * 16), 120);
                    }
                }
                else
                {
                    if (lineOfSight.IsLineClear(mouseGridPosition))
                    {
                        projectile.Fire(ownerTransform.Position, new Vector2(mouseGridPosition.X * 16, mouseGridPosition.Y * 16), 120);
                    }
                    else
                    {
                        Point collisionPoint = lineOfSight.GetCollisionPoint(mouseGridPosition);
                        projectile.Fire(ownerTransform.Position, new Vector2(collisionPoint.X * 16, collisionPoint.Y * 16), 120);
                    }
                }
                
                
                DamageTarget(target, projectile);
                CurrentCooldown = CooldownPeriod;
                wasJustUsed = true;
                OnUsed.Emit();
                State = SkillState.EXECUTING;
            }
        }

        private async void DamageTarget(Entity _target, ProjectileComponent _projectileComponent)
        {
            await _projectileComponent.ReachedTarget.Wait();  // TODO this may be better set as not an await, but this function will be called when the projectiles'
                                                            // ReachedTarget signal is emitted. The parameters will just be stored as headers, which will be
                                                            // cleared once this function has ran
            
            if (_target != null)
            {
                Attack ownerAttack = Owner.GetComponent<Attack>();
                ownerAttack.DealMagicalDamage(Owner.GetComponent<Stats>(), _target.GetComponent<Stats>(), _target.GetComponent<TakeDamage>());
            }

            State = SkillState.NOT_IN_USE;
            Owner.TurnEnded.Emit();
        }
    }
}