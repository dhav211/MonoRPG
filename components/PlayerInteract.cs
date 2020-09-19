using Microsoft.Xna.Framework;
using System;

namespace MonoRPG
{
    public class PlayerInteract : Component
    {
        PlayerController playerController;

        public Command command { get; set; } = new Command();

        public PlayerInteract(Entity _owner) : base(_owner)
        {
            owner.AddComponent<PlayerInteract>(this);
        }
        public override void Initialize()
        {
            playerController = owner.GetComponent<PlayerController>();
        }

        public override void Update(float deltaTime)
        {
            if (Input.IsMouseButtonJustPressed(Input.MouseButton.LEFT) && GameState.CanPlayerMove())
            {
                // TODO: refactor this so it is not only used here but in the player controller script
                Entity entityClicked = null;
                Point gridPositionClicked = Input.GetMouseGridPosition();

                if (!owner.Grid.IsOutOfBounds(gridPositionClicked.X, gridPositionClicked.Y))
                    entityClicked = owner.Grid.GetEntityInGridPosition(gridPositionClicked);
                
                if (entityClicked != null)
                    playerController.TargetToFollow = entityClicked;
                else
                    return; // Exit this function because no entity was clicked

                command = SetCommand(entityClicked);

                if (command.IsSet)
                {
                    if (owner.Grid.IsEntityNearby(owner, entityClicked))
                    {
                        command.Execute();
                        playerController.TargetToFollow = null; // No longer needed to track because command has been executed
                        owner.TurnEnded.Emit(); // TODO: Temporary solution, this signal will emit when the command has fully finished.
                    }
                    else
                    {
                        Transform entityTransform = entityClicked.GetComponent<Transform>();
                        playerController.SetPathToFollow(entityTransform.GridPosition);
                        playerController.CurrentState = PlayerController.State.FOLLOW_PATH;
                    }
                }
            }
        }

        public Command SetCommand(Entity _entityToInteract)
        {
            InteractionComponent entityInteraction = null;

            if (_entityToInteract != null)
                entityInteraction = _entityToInteract.GetComponent<InteractionComponent>() as InteractionComponent;

            Command commandToReturn = new Command();

            if (_entityToInteract != null && entityInteraction != null)
            {
                if (entityInteraction.MainInteraction == InteractionComponent.InteractionType.ATTACK)
                {
                    Attack attack = owner.GetComponent<Attack>();
                    Action<Stats, Stats, TakeDamage> attackAction = attack.DealPhysicalDamage;
                    commandToReturn.SetCommand(attackAction, owner.GetComponent<Stats>(), _entityToInteract.GetComponent<Stats>(), _entityToInteract.GetComponent<TakeDamage>());
                }

                else if (entityInteraction.MainInteraction == InteractionComponent.InteractionType.OPEN_CHEST)
                {
                    ChestComponent chest = _entityToInteract.GetComponent<ChestComponent>();
                    Action openChestAction = chest.Open;
                    commandToReturn.SetCommand(openChestAction);
                }
            }

            return commandToReturn;
        }
    }
}