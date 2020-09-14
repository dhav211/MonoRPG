using Microsoft.Xna.Framework;
using System;

namespace MonoRPG
{
    public class Transform : Component
    {
        /*
        public Vector2 Position 
        {
            get
            {
                return position;
            }
            set
            {
                if (GridPosition.X % 1 == 0 && GridPosition.Y % 1 == 0 && isInstanced)
                    owner.Grid.RemoveEntityFromGridNode(GridPosition.X, GridPosition.Y);

                position = value;
                GridPosition = new Point((int)Math.Floor(Position.X) / 16, (int)Math.Floor(Position.Y / 16));

                if (GridPosition.X % 1 == 0 && GridPosition.Y % 1 == 0)
                    owner.Grid.SetEntityInGridNode(GridPosition.X, GridPosition.Y, owner);
                
                // A saftety net so if by chance there is an entity at 0,0 it won't be removed
                if (!isInstanced)
                    isInstanced = true;
            }
        }
        */

        private Vector2 position = new Vector2();
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                owner.ClickRect = new Rectangle((int)Position.X, (int)Position.Y, owner.Size.X, owner.Size.Y);
            }
        }
        public Point GridPosition { get; set; }
        //private bool isInstanced = false;

        public Transform (Entity _owner) : base(_owner)
        {
            owner.AddComponent<Transform>(this);
        }

        public override void Update(float deltaTime) { }

        public override void Draw(float deltaTime) { }

        public override void Initialize() { }
    }
}