using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
            if (Input.IsMouseButtonJustPressed(Input.MouseButton.LEFT) && GameState.CanPlayerMove() && playerController.CurrentState == PlayerController.State.STANDING)
            {
                if (!Input.IsMouseInClickRange())
                    return;
                    
                // TODO: refactor this so it is not only used here but in the player controller script
                Entity entityClicked = null;
                Point gridPositionClicked = Input.GetMouseGridPosition();

                if (!owner.Grid.IsOutOfBounds(gridPositionClicked.X, gridPositionClicked.Y))
                { // Get the entity in the position clicked, if there are multiple entities it chooses the only one that has an interaction component
                    List<Entity> entitiesInPosition = owner.Grid.GetEntitiesInGridPosition(gridPositionClicked);
                    
                    foreach (Entity e in entitiesInPosition)
                    {
                        if (e.HasComponent<InteractionComponent>())
                        {
                            entityClicked = e;
                        }
                    }
                }
                
                if (entityClicked != null)
                {
                    // There can be times where the player will be stuck on a dead enemy, this makes sure the player won't try following a dead enemy.
                    //TODO this may cause problems later one. Possibly entity.IsWalkable variable is more suitable.
                    if (entityClicked.IsAlive)
                        playerController.TargetToFollow = entityClicked;
                    else
                        playerController.TargetToFollow = null;
                }
                else
                {
                    playerController.TargetToFollow = null;
                    return; // Exit this function because no entity was clicked
                }

                command = SetCommand(entityClicked);

                if (command.IsSet)
                {
                    if (owner.Grid.IsEntityInNearbySquare(owner, entityClicked))
                    {
                        command.Execute();
                        playerController.TargetToFollow = null; // No longer needed to track because command has been executed
                        owner.TurnEnded.Emit(); // TODO: Temporary solution, this signal will emit when the command has fully finished.
                    }
                    else // Entity is not nearby so start the path following
                    {
                        MovePlayerToPosition(gridPositionClicked);
                    }
                }
                else  // The player clicked on an entity with no interactions, just walk to it without the command set
                {
                    MovePlayerToPosition(gridPositionClicked);
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

                else if (entityInteraction.MainInteraction == InteractionComponent.InteractionType.LOOT)
                {
                    EnemyLoot loot = _entityToInteract.GetComponent<EnemyLoot>();
                    Action lootEnemyAction = loot.LootEnemy;
                    commandToReturn.SetCommand(lootEnemyAction);
                }
            }

            return commandToReturn;
        }

        private void MovePlayerToPosition(Point _gridPositionToMove)
        {
            playerController.SetPathToFollow(_gridPositionToMove);
            playerController.CurrentState = PlayerController.State.FOLLOW_PATH;
        }
    }
}