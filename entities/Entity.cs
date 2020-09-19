using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoRPG
{
    public class Entity
    {
        public List<Component> Components { get; private set; } = new List<Component>();
        public EntityManager entityManager { get; private set; }
        public MapGrid Grid { get; private set; }
        public TurnManager TurnManager { get; private set; }
        public string Name { get; set; }
        public bool IsAlive { get; set; } = true;
        public Point Size { get; set; } = new Point(16,16);
        public Rectangle ClickRect { get; set; }
        public bool IsMouseHovered { get; set; }
        public bool IsInteractable { get; private set; } = true;
        public Signal TurnStarted { get; private set; } = new Signal();
        public Signal TurnEnded { get; private set; } = new Signal();
        public Signal MouseEntered { get; private set; } = new Signal();
        public Signal MouseExited { get; private set; } = new Signal();
        public Signal MousePressed { get; private set; } = new Signal();


        public virtual void Update(float deltaTime) 
        {
            foreach (Component component in Components)
            {
                component.Update(deltaTime);
            }

            HandleMouseClick();
        }

        public virtual void Draw(float deltaTime) { }

        public virtual void Initialize(EntityManager _entityManager, Vector2 _position = new Vector2(), Level.LevelEntityValues _entityValues = null) 
        { 
            entityManager = _entityManager;
            Grid = entityManager.Grid;
            TurnManager = entityManager.TurnManager;
        }

        ///<summary>
        /// Perform any initialization that would require all entities to be instantiated first
        ///</summary>
        public virtual void PostInitialize() { }

        ///<summary>
        /// Returns the component of given type from entity
        ///</summary>
        public T GetComponent<T>() where T : Component
        {
            Type componentType = typeof(T);

            foreach (Component component in Components)
            {
                if (component.GetType() == componentType)
                {
                    return component as T;
                }
            }

            return null;
        }

        ///<summary>
        /// Returns a bool stating wether the entity has the given component or not
        ///</summary>
        public bool HasComponent<T>() where T : Component
        {
            Type componentType = typeof(T);

            foreach (Component component in Components)
            {
                if (component.GetType() == componentType)
                {
                    return true;
                }
            }

            return false;
        }

        public void AddComponent<T>(T _component) where T : Component
        {
            Components.Add(_component);
        }

        ///<summary>
        /// Sets entity's grid position in it's transform component, based on it's current Position, then updates the Grid filling the space with the entity.
        ///</summary>
        public void SetGridPosition(Transform _transform)
        {
            Grid.RemoveEntityFromGridNode(_transform.GridPosition.X, _transform.GridPosition.Y);
            _transform.GridPosition = new Point((int)Math.Floor(_transform.Position.X) / 16, (int)Math.Floor(_transform.Position.Y / 16));
            Grid.SetEntityInGridNode(_transform.GridPosition.X, _transform.GridPosition.Y, this);
        }

        ///<summary>
        /// Sets entity's grid position in it's transform component, then updates the Grid filling the space with the entity.
        ///</summary>
        public void SetGridPosition(Transform _transform, Point _gridPositionToSet)
        {
            Grid.RemoveEntityFromGridNode(_transform.GridPosition.X, _transform.GridPosition.Y);
            _transform.GridPosition = _gridPositionToSet;
            Grid.SetEntityInGridNode(_transform.GridPosition.X, _transform.GridPosition.Y, this);
        }

        ///<summary>
        /// Checks if mouse is in clicking range, checks for mouse input, and emits all mouse related signals
        ///</summary>
        private void HandleMouseClick()
        {
            Vector2 mousePosition = Input.GetMouseWorldPosition();
            bool mousePressed = Input.IsMouseButtonJustPressed(Input.MouseButton.LEFT) || Input.IsMouseButtonJustPressed(Input.MouseButton.RIGHT); 

            if ((mousePosition.X > ClickRect.X) && (mousePosition.X < ClickRect.X + ClickRect.Width) &&
                (mousePosition.Y > ClickRect.Y) && (mousePosition.Y < ClickRect.Y + ClickRect.Height))
            {
                if (!IsMouseHovered)
                    MouseEntered.Emit();
        
                IsMouseHovered = true;
            }
            else
            {
                if (IsMouseHovered)
                    MouseExited.Emit();

                IsMouseHovered = false;
            }

            if (IsMouseHovered && mousePressed)
            {
                MousePressed.Emit();
            }
        }

        public virtual void Kill() { }

        public virtual void Destroy() { }

        public bool IsEntitiesTurn()
        {
            if (TurnManager.CurrentEntity == this)
                return true;
            
            return false;
        }
    }
}