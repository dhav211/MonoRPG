using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class MolotovSkill : Skill
    {
        FloodFill floodFill;
        Point currentGridPosition = new Point();

        List<IndicationArea> indicationAreas = new List<IndicationArea>();
        List<Point> attackPositions = new List<Point>();

        public MolotovSkill(Entity _entity) : base(_entity)
        {
            Name = "Molotov";
            Cost = 5;
            CooldownPeriod = 8;
            State = SkillState.NOT_IN_USE;

            Icon = _entity.entityManager.ContentManager.Load<Texture2D>("ui/fire_spread_icon");

            floodFill = new FloodFill(Owner.Grid);
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
            Point mousePosition = Input.GetMouseGridPosition();

            if (mousePosition != currentGridPosition)
            {
                currentGridPosition = mousePosition;
                // if there are indication areas already, remove them from the game

                if (indicationAreas.Count > 0)
                {
                    foreach(IndicationArea area in indicationAreas)
                        area.Kill();
                    
                    indicationAreas.Clear();
                }

                // Check to see if line is clear from caster to destination.
                // if there is a collision then set the currentGridPosition to the point before collision
                if (!lineOfSight.IsLineClear(currentGridPosition))
                {
                    currentGridPosition = lineOfSight.GetPointBeforeCollision(currentGridPosition);
                }

                floodFill.Start(currentGridPosition, 3);

                // The flood fill will pick up walls what not, this makes sure that only the walkable areas are gathered
                attackPositions = floodFill.GetAllWalkablePositions();

                // create red area indicators where flood fill positions are given
                foreach(Point point in attackPositions)
                {
                    IndicationArea area = new IndicationArea();
                    EntityCreator.CreateEntity<IndicationArea>(area, new Vector2(point.X * 16, point.Y * 16));
                    indicationAreas.Add(area);
                }
            }

            if (Input.IsMouseButtonJustPressed(Input.MouseButton.LEFT) && attackPositions.Count > 0)
            {
                Molotov molotovToWait = null;
                List<Entity> entitiesToDamage = new List<Entity>();
                
                foreach(Point position in attackPositions)
                {
                    Molotov molotov = new Molotov();
                    EntityCreator.CreateEntity<Molotov>(molotov, new Vector2(position.X * 16, position.Y * 16));

                    // Check to see if there are any entities in the molotovs attack range
                    if (Owner.Grid.IsEntityOcuppyingGridPosition(position))
                    {
                        List<Entity> entities = Owner.Grid.GetEntitiesInGridPosition(position);

                        // if any of the entities can be damaged, then add them to the list
                        foreach (Entity entity in entities)
                        {
                            if (entity.HasComponent<TakeDamage>())
                            {
                                entitiesToDamage.Add(entity);
                            }
                        }
                    }

                    // only one molotov instance needs to be sent to be waited on for the DamageTarget method
                    if (molotovToWait == null)
                        molotovToWait = molotov;
                }

                foreach (IndicationArea area in indicationAreas)
                    area.Kill();

                indicationAreas.Clear();

                Stats stats = Owner.GetComponent<Stats>();
                stats.MP -= Cost;
                DamageTarget(molotovToWait, entitiesToDamage);
                CurrentCooldown = CooldownPeriod;
                wasJustUsed = true;
                OnUsed.Emit();
                State = SkillState.EXECUTING;
            }
        }

        private async void DamageTarget(Molotov _molotov, List<Entity> _entitiesToDamage)
        {
            await _molotov.Destroy.Wait();

            Attack ownerAttack = Owner.GetComponent<Attack>();

            for(int i = 0; i < _entitiesToDamage.Count; ++i)
            {
                ownerAttack.DealMagicalDamage(Owner.GetComponent<Stats>(), _entitiesToDamage[i].GetComponent<Stats>(), _entitiesToDamage[i].GetComponent<TakeDamage>());
            }

            State = SkillState.NOT_IN_USE;
            Owner.TurnEnded.Emit();
        }
    }
}