using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoRPG
{
    public class Enemy : Entity
    {
        Transform transform;
        SpriteRenderer spriteRenderer;
        Stats stats;
        Attack attack;
        TakeDamage takeDamage;
        InteractionComponent interactionComponent;
        AnimationController animation;
        EnemyController enemyController;
        EnemyAI enemyAI;
        EnemyStatPopupController enemyStatPopupController;
        public Player Player { get; private set; }

        public Action _onTurnEnded { get; private set; }

        public override void Initialize(EntityManager _entityManager, Vector2 _position, Level.LevelEntityValues _entityValues = null)
        {
            base.Initialize(_entityManager);

            transform = new Transform(this);
            spriteRenderer = new SpriteRenderer(this, entityManager.SpriteBatch, entityManager.ContentManager.Load<Texture2D>("sprites/red_box"), new Point(16,16));
            stats = new Stats(this);
            attack = new Attack(this);
            takeDamage = new TakeDamage(this);
            interactionComponent = new InteractionComponent(this);
            animation = new AnimationController(this);
            enemyController = new EnemyController(this);
            enemyAI = new EnemyAI(this);
            enemyStatPopupController = new EnemyStatPopupController(this);

            TurnManager.Entities.Add(this);

            foreach (Component component in Components)
                component.Initialize();
            
            transform.Position = _position;
            transform.GridPosition = new Point((int)Math.Floor(_position.X) / 16, (int)Math.Floor(_position.Y / 16));
            Grid.SetEntityInGridNode(transform.GridPosition.X, transform.GridPosition.Y, this);

            Name = "Enemy";
            stats.SetStats(Name, 1, 10, 5, 3, 3, 3, 3);
            interactionComponent.MainInteraction = InteractionComponent.InteractionType.ATTACK;
            interactionComponent.Interactions.Add(InteractionComponent.InteractionType.ATTACK);

            _onTurnEnded = onTurnEnded;
            TurnEnded.Add("enemy", _onTurnEnded);

            animation.Add("idle", new int[] { 0 });
            animation.Add("dead", new int[] { 1 });
            animation.Play("idle");
        }

        public override void PostInitialize()
        {
            Player = entityManager.GetEntityOfType<Player>() as Player;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw(float deltaTime)
        {
            spriteRenderer.Draw(deltaTime);
        }

        public void onTurnEnded()
        {
            TurnManager.TurnEnded();
        }

        public override void Kill()
        {
            IsAlive = false;

            animation.Play("dead");

            interactionComponent.Interactions.Remove(InteractionComponent.InteractionType.ATTACK);
            interactionComponent.MainInteraction = InteractionComponent.InteractionType.LOOT;
            interactionComponent.Interactions.Add(InteractionComponent.InteractionType.LOOT);
        }
    }
}