using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoRPG
{
    public class Player : Entity
    {
        Transform transform;
        SpriteRenderer spriteRenderer;
        AnimationController animation;
        PlayerController playerController;
        PlayerInteract playerInteract;
        Stats stats;
        TakeDamage takeDamage;
        Attack attack;
        SkillsComponent skills;
        public Camera Camera { get; set; }

        public Action _onTurnEnded;

        public override void Initialize(EntityManager _entityManager, Vector2 _position, Level.LevelEntityValues _entityValues = null)
        {
            base.Initialize(_entityManager);

            Camera = entityManager.Camera;
            transform = new Transform(this);
            spriteRenderer = new SpriteRenderer(this, entityManager.SpriteBatch, entityManager.ContentManager.Load<Texture2D>("sprites/white_box"), new Point(16,16));
            animation = new AnimationController(this);
            playerController = new PlayerController(this);
            playerInteract = new PlayerInteract(this);
            stats = new Stats(this);
            takeDamage = new TakeDamage(this);
            attack = new Attack(this);
            skills = new SkillsComponent(this);

            TurnManager.Entities.Add(this);

            foreach (Component component in Components)
                component.Initialize();
            
            transform.Position = _position;
            transform.GridPosition = new Point((int)Math.Floor(_position.X) / 16, (int)Math.Floor(_position.Y / 16));
            Grid.SetEntityInGridNode(transform.GridPosition.X, transform.GridPosition.Y, this);

            Name = "Player";
            stats.SetStats("Player", 1, 15, 5, 4, 4, 4, 4);

            _onTurnEnded = onTurnEnded;

            TurnEnded.Add("player", _onTurnEnded);

            animation.Add("idle", new int[] {0, 1, 2, 1}, 4);
            animation.Add("walk", new int[] {0, 3, 4, 3}, 4);

            TurnManager.SetCurrentEntity(this);
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
    }
}