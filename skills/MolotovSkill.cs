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

        public MolotovSkill(Entity _entity) : base(_entity)
        {
            Name = "Molotov";
            Cost = 5;
            CooldownPeriod = 3;
            State = SkillState.NOT_IN_USE;

            Icon = _entity.entityManager.ContentManager.Load<Texture2D>("ui/fire_spread_icon");

            floodFill = new FloodFill(Owner.Grid);
            
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
            Point mousePosition = Input.GetMouseGridPosition();

            if (mousePosition != currentGridPosition)
            {
                currentGridPosition = mousePosition;
                // if there are indication areas already, remove them from the game
                // do a flood fill
                // create red area indicators where flood fill positions are given

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

                List<Point> floodFillPoints = floodFill.GetAllWalkablePositions();

                foreach(Point point in floodFillPoints)
                {
                    IndicationArea area = new IndicationArea();
                    EntityCreator.CreateEntity<IndicationArea>(area, new Vector2(point.X * 16, point.Y * 16));
                    indicationAreas.Add(area);
                }
            }
        }
    }
}