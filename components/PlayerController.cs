using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoRPG
{
    public class PlayerController : Component
    {
        Transform transform;
        PlayerInteract playerInteract;
        AnimationController animation;
        Tween tween;
        Camera camera;
        Pathfinder pathfinder;

        float speed = 100;

        public enum State { STANDING, MOVING, FOLLOW_PATH, INTERACTING, WAITING_FOR_TURN, USING_SKILL }
        public State CurrentState { get; set; } = State.STANDING;

        Point currentMoveDirection = new Point();
        Point[] previousGridPositions = new Point[2] { new Point(-1,-1), new Point(-1,-1) };

        public List<Point> PathToFollow { get; private set; } = new List<Point>();
        public Entity TargetToFollow { get; set; } = null;

        public Action _onFinishedMoving { get; private set; }
        public Action _onTurnEnded { get; private set; }
        public Action _onTurnStarted { get; private set; }

        public PlayerController(Entity _owner) : base(_owner)
        {
            owner.AddComponent<PlayerController>(this);

            tween = new Tween();
            pathfinder = new Pathfinder(owner.Grid);

            Player player = owner as Player;
            camera = player.Camera;

            _onFinishedMoving = onFinishedMoving;
            _onTurnEnded = onTurnEnded;
            _onTurnStarted = onTurnStarted;

            tween.OnComplete.Add("player_controller", _onFinishedMoving);
        }

        public override void Initialize()
        {
            CheckRequiredComponents();
            transform = owner.GetComponent<Transform>() as Transform;
            playerInteract = owner.GetComponent<PlayerInteract>() as PlayerInteract;
            animation = owner.GetComponent<AnimationController>() as AnimationController;

            owner.TurnEnded.Add("player_controller", _onTurnEnded);
            owner.TurnStarted.Add("player_controller", _onTurnStarted);
        }

        public override void Update(float deltaTime)
        {   
            if (CurrentState == State.STANDING && owner.IsEntitiesTurn())
            {
                animation.Play("idle");

                if (GameState.CanPlayerMove())
                {
                    MouseMove();
                    KeyboardInteract();
                    KeyboardMove();
                }
            }
            else if (CurrentState == State.MOVING)
            {
                animation.Play("walk");
                if (GameState.CanPlayerMove())
                {
                    transform.Position = tween.TweenVector2(transform.Position, deltaTime);
                }

                // TODO create a cancel path to follow fucntion
            }
            else if (CurrentState == State.FOLLOW_PATH && owner.IsEntitiesTurn())
            {
                if (GameState.CanPlayerMove())
                {
                    if (PathToFollow.Count == 0)
                    {
                        CurrentState = State.STANDING;
                        return;
                    }
                    else
                    {
                        if (TargetToFollow == null)
                        {
                            SetPathToFollow(PathToFollow[PathToFollow.Count - 1]); // Keep following in same path
                        }
                        else  // Recacluate the path based upon the targets position and not old target
                        {
                            Transform targetTransform = TargetToFollow.GetComponent<Transform>() as Transform;
                            SetPathToFollow(targetTransform.GridPosition);
                        }

                        // There exists a chance an enemy can block the players path, so they will keep moving out of each others way creating an infinite
                        // loop. This checks to see if the path isn't the same as it was two moves ago. if it was, then it means it's stuck in the same path
                        if (PathToFollow[0] == previousGridPositions[1])
                        {
                            CurrentState = State.STANDING;
                            return;
                        }

                        currentMoveDirection = PathToFollow[0] - transform.GridPosition;
                        PathToFollow.RemoveAt(0);
                        MoveInDirection(new Vector2(currentMoveDirection.X, currentMoveDirection.Y));
                    }
                }
            }
        }

        private void MouseMove()
        {
            if (Input.IsMouseButtonJustPressed(Input.MouseButton.LEFT) && owner.Grid.IsNodeWalkable(Input.GetMouseGridPosition().X, Input.GetMouseGridPosition().Y))
            {
                if (!Input.IsMouseInClickRange())
                    return;

                ClearPreviousPositions();
                
                // Exit from this function if an entity is clicked in the PlayerInteract component.
                // The player interact should have priority over this command
                if (owner.Grid.IsEntityOcuppyingGridPosition(Input.GetMouseGridPosition()))
                    return;

                SetPathToFollow(Input.GetMouseGridPosition());
                currentMoveDirection = PathToFollow[0] - transform.GridPosition;
                PathToFollow.RemoveAt(0);
                MoveInDirection(new Vector2(currentMoveDirection.X, currentMoveDirection.Y));
            }
        }

        ///<summary>
        /// Gets player directional input and sets player in motion.
        ///</summary>
        private void KeyboardMove()
        {
            bool up = Input.IsKeyPressed(Keys.W) || Input.IsKeyPressed(Keys.Up);
            bool down = Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.Down);
            bool left = Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.Left);
            bool right = Input.IsKeyPressed(Keys.D) || Input.IsKeyPressed(Keys.Right);

            if (up && down)
                up = down = false;
            if (left && right)
                left = right = false;

            if (up && owner.Grid.IsNodeWalkable(transform.GridPosition.X, transform.GridPosition.Y - 1))
            {
                currentMoveDirection = new Point(0, -1);
                MoveInDirection(new Vector2(0, -1));
            }
            if (down && owner.Grid.IsNodeWalkable(transform.GridPosition.X, transform.GridPosition.Y + 1))
            {
                currentMoveDirection = new Point(0, 1);
                MoveInDirection(new Vector2(0, 1));
            }
            if (left && owner.Grid.IsNodeWalkable(transform.GridPosition.X - 1, transform.GridPosition.Y))
            {
                currentMoveDirection = new Point(-1, 0);
                MoveInDirection(new Vector2(-1, 0));
            }
            if (right && owner.Grid.IsNodeWalkable(transform.GridPosition.X + 1, transform.GridPosition.Y))
            {
                currentMoveDirection = new Point(1, 0);
                MoveInDirection(new Vector2(1, 0));
            }
        }

        private void KeyboardInteract()
        {
            Entity entityToInteract = null;
            bool up = Input.IsKeyJustPressed(Keys.W) || Input.IsKeyJustPressed(Keys.Up);
            bool down = Input.IsKeyJustPressed(Keys.S) || Input.IsKeyJustPressed(Keys.Down);
            bool left = Input.IsKeyJustPressed(Keys.A) || Input.IsKeyJustPressed(Keys.Left);
            bool right = Input.IsKeyJustPressed(Keys.D) || Input.IsKeyJustPressed(Keys.Right);

            if (up && down)
                up = down = false;
            if (left && right)
                left = right = false;

            if (up && owner.Grid.IsEntityOcuppyingGridPosition(new Point(transform.GridPosition.X, transform.GridPosition.Y - 1)))
            {
                entityToInteract = owner.Grid.GetEntityInGridPosition(new Point(transform.GridPosition.X, transform.GridPosition.Y - 1));
            }
            if (down && owner.Grid.IsEntityOcuppyingGridPosition(new Point(transform.GridPosition.X, transform.GridPosition.Y + 1)))
            {
                entityToInteract = owner.Grid.GetEntityInGridPosition(new Point(transform.GridPosition.X, transform.GridPosition.Y + 1));
            }
            if (left && owner.Grid.IsEntityOcuppyingGridPosition(new Point(transform.GridPosition.X - 1, transform.GridPosition.Y)))
            {
                entityToInteract = owner.Grid.GetEntityInGridPosition(new Point(transform.GridPosition.X - 1, transform.GridPosition.Y));
            }
            if (right && owner.Grid.IsEntityOcuppyingGridPosition(new Point(transform.GridPosition.X + 1, transform.GridPosition.Y)))
            {
                entityToInteract = owner.Grid.GetEntityInGridPosition(new Point(transform.GridPosition.X + 1, transform.GridPosition.Y));
            }

            if (entityToInteract != null)
                playerInteract.command = playerInteract.SetCommand(entityToInteract);

            if (playerInteract.command.IsSet)
            {
                playerInteract.command.Execute();
                owner.TurnEnded.Emit();
            }
        }

        ///<summary>
        /// Sets the list of points as move positions for player to put based upon destination given
        ///</summary>
        public void SetPathToFollow(Point _destination)
        {
            PathToFollow = pathfinder.GetPath(transform.GridPosition, _destination);
            if (PathToFollow.Count == 0)
                return;
        }

        ///<summary>
        /// Sets the tween to move player by using a direction Vector
        ///</summary>
        private void MoveInDirection(Vector2 _direction)
        {
            owner.SetGridPosition(transform, transform.GridPosition + new Point((int)_direction.X, (int)_direction.Y));
            
            // This helps prevent the player getting stuck in infinite pathfinding loops by keeping track of previous spaces
            previousGridPositions[1] = previousGridPositions[0];
            previousGridPositions[0] = transform.GridPosition;

            _direction *= 16;
            tween.SetTween(transform.Position, transform.Position + _direction, 16 / speed, Tween.EaseType.LINEAR);
            tween.Start();
            CurrentState = State.MOVING;
        }

        ///<summary>
        /// Handles camera movement based on players position on map. Also calls camera back to player if player is out of bounds.
        ///</summary>
        private void MoveCamera()
        {
            if (camera.CurrentState == Camera.State.SCROLLING)
                return;
            
            // Check if player is off screen. If player is off screen then tween the camera to position of player
            if (camera.IsEntityOutOfBounds(owner))
            {
                camera.ScrollToPosition(new Vector2(transform.Position.X - (Screen.Width / 2), transform.Position.Y - (Screen.Height / 2)), 150f);
            }

            float scrollDistance = 192;
            Vector2 scrollDirection = new Vector2();
            Vector2 playerScreenPosition = camera.WorldToScreen(transform.Position);

            if (playerScreenPosition.X > Screen.Width * .85 && currentMoveDirection == new Point(1,0))
            {
                scrollDirection.X = 1;
            }
            else if (playerScreenPosition.X < Screen.Width * .15 && currentMoveDirection == new Point(-1,0))
            {
                scrollDirection.X = -1;
            }

            if (playerScreenPosition.Y > Screen.Height * .75 && currentMoveDirection == new Point(0,1))
            {
                scrollDirection.Y = 1;
            }
            else if (playerScreenPosition.Y < Screen.Height * .15  && currentMoveDirection == new Point(0,-1))
            {
                scrollDirection.Y = -1;
            }

            if (scrollDirection.X > 0 || scrollDirection.X < 0)
            {
                camera.Scroll(scrollDirection, Screen.Width * .65f, .35f);
            }
            else if (scrollDirection.Y > 0 || scrollDirection.Y < 0)
            {
                camera.Scroll(scrollDirection, Screen.Height * .65f, .35f);
            }
        }
        
        public void onFinishedMoving()
        {
            MoveCamera();  // Once player is finished moving then check to see if the camera can move
            currentMoveDirection = Point.Zero;
            owner.TurnEnded.Emit();
        }

        public void onTurnStarted()
        {
            if (PathToFollow.Count > 0)
            {
                if (TargetToFollow != null && owner.Grid.IsEntityInNearbySquare(owner, TargetToFollow) && playerInteract.command.IsSet)
                {
                    playerInteract.command.Execute();
                    PathToFollow.Clear();
                    TargetToFollow = null; // No longer needed to track because command has been executed
                    owner.TurnEnded.Emit(); // TODO: Temporary solution, this signal will emit when the command has fully finished.
                }
                else
                {
                    CurrentState = State.FOLLOW_PATH;
                }
            }
            /*
            TODO: I don't think this bit here is needed, remove if proven useless
            else if(PathToFollow.Count == 0 && playerInteract.command.IsSet) 
            {
                playerInteract.command.Execute();
                TargetToFollow = null; // No longer needed to track because command has been executed
                owner.TurnEnded.Emit(); // TODO: Temporary solution, this signal will emit when the command has fully finished.
                //CurrentState = State.INTERACTING;  This state will come in play once commands have actual animations and what not
            }
            */
            else
            {
                CurrentState = State.STANDING;
            }
        }

        public void onTurnEnded()
        {
            CurrentState = State.WAITING_FOR_TURN;
        }

        public void ClearPreviousPositions()
        {
            previousGridPositions[0] = new Point(-1,-1);
            previousGridPositions[1] = new Point(-1,-1);
        }

        protected override void CheckRequiredComponents()
        {
            List<Type> requiredComponents = new List<Type>();
            requiredComponents.Add(typeof(Transform));

            foreach(Type requiredComponent in requiredComponents)
            {
                // TODO finish this later
            }
            //Console.Error.WriteLine(requiredTransform.Name + " was not found!");
        }
    }
}