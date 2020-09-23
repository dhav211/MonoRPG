using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MonoRPG
{
    public class EnemyController : Component
    {
        public enum State { WANDER, CHASE, UNDISCOVERED }
        public State CurrentState { get; set; } = State.WANDER;

        EnemyAI enemyAI;
        Transform transform;
        Tween tween;
        LineOfSight lineOfSight;
        Player player;
        Transform playerTransform;
        Pathfinder pathfinder;
        Wait wait;

        float viewRange = 128f;
        Command command = new Command();
        public List<Point> PathToFollow { get; private set; }
        

        public Action _onTurnStarted { get; private set; }
        public Action _onTurnEnded { get; private set; }
        public Action _onFinishedMoving { get; private set; }

        public EnemyController(Entity _owner) : base(_owner)
        {
            owner.AddComponent<EnemyController>(this);

            tween = new Tween();
            lineOfSight = new LineOfSight(owner, owner.Grid);
            pathfinder = new Pathfinder(owner.Grid);
            wait = new Wait();

            _onTurnStarted = onTurnStarted;
            _onTurnEnded = onTurnEnded;
            _onFinishedMoving = onFinishedMoving;

            tween.OnComplete.Add("enemy_controller", _onFinishedMoving);
        }

        public override void Initialize()
        {
            enemyAI = owner.GetComponent<EnemyAI>() as EnemyAI;
            transform = owner.GetComponent<Transform>() as Transform;

            owner.TurnStarted.Add("enemy_controller", _onTurnStarted);
        }

        public override void PostInitialize()
        {
                Enemy self = owner as Enemy;
                player = self.Player;
                playerTransform = self.Player.GetComponent<Transform>() as Transform;
        }

        public override void Update(float deltaTime)
        {
            if (tween.IsRunning())
            {
                transform.Position = tween.TweenVector2(transform.Position, deltaTime);
            }
        }

        public async void onTurnStarted()
        {
            if (CurrentState == State.WANDER && owner.IsEntitiesTurn())
            {
                float distanceFromPlayer = Vector2.Distance(transform.Position, playerTransform.Position);

                if (distanceFromPlayer < viewRange)
                // TODO: Right now this only is based off a distance, in the future I want to establish fog of war.
                // Once enemy enters the fog of war, then it will enter this state
                {
                    if (lineOfSight.IsLineToTargetClear(player))
                    {
                        CurrentState = State.CHASE;
                        SetCommand();
                        PathToFollow = pathfinder.GetPath(transform.GridPosition, playerTransform.GridPosition);

                        // set command to attack player
                        if (owner.Grid.IsEntityInNearbySquare(owner, player)) // if player nearby then attack the asshole
                        {
                            command.Execute();
                        }
                        else // player is not nearby start moving towards him
                        {
                            StartMovementOnPath();
                        }
                    }
                    else // Not in line of sight, so skip turn
                    {
                        owner.TurnEnded.Emit();
                    }
                }
                else // Not in viewing range, so skip turn.
                {
                    owner.TurnEnded.Emit();
                }
            }
            else if (CurrentState == State.CHASE && owner.IsEntitiesTurn())
            {
                if (owner.Grid.IsEntityInNearbySquare(owner, player)) // if player nearby then attack the asshole
                {
                    await wait.WaitUntil(.5f);

                    if (command.IsSet)
                        command.Execute();
                    else
                    {
                        SetCommand();
                        command.Execute();
                    }
                    owner.TurnEnded.Emit();
                }
                else // player is not nearby start moving towards him
                {
                    PathToFollow = pathfinder.GetPath(transform.GridPosition, playerTransform.GridPosition);
                    StartMovementOnPath();
                }
            }
        }

        private void SetCommand()
        {
            // TODO: Right now this is super simple, it only gets the physical attack.
            // Eventually this will be handled by the enemyAI script. The enemy will choose various skills, like distance attacks, attack magic,
            // healing self and others, and fleeing.
            Attack attack = owner.GetComponent<Attack>() as Attack;
            command = new Command();
            Action<Stats, Stats, TakeDamage> attackAction = attack.DealPhysicalDamage;
            command.SetCommand(attackAction, owner.GetComponent<Stats>(), player.GetComponent<Stats>(), player.GetComponent<TakeDamage>());
        }

        private void StartMovementOnPath()
        {
            Point direction = PathToFollow[0] - transform.GridPosition;
            owner.SetGridPosition(transform, PathToFollow[0]);

            tween.SetTween(transform.Position, transform.Position + new Vector2(direction.X * 16, direction.Y * 16), .25f, Tween.EaseType.LINEAR);
            tween.Start();
        }

        public void onTurnEnded()
        {

        }

        public void onFinishedMoving()
        {
            owner.TurnEnded.Emit();
        }
    }
}