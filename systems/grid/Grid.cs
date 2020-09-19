using Microsoft.Xna.Framework;
using System;

namespace MonoRPG
{
    public class MapGrid
    {
        public GridNode[,] Grid { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public void FormGrid (int _width, int _height)
        {
            Grid = new GridNode[_width, _height];
            Width = _width;
            Height = _height;

            for (int y = 0; y < _height; ++y)
            {
                for (int x = 0; x < _width; ++x)
                {
                    Grid[x, y] = new GridNode();
                }
            }
        }

        public void SetGridNodeWalkablity(int _x, int _y, bool _isWalkable)
        {
            Grid[_x, _y].IsWalkable = _isWalkable;
        }

        public void SetEntityInGridNode(int _x, int _y, Entity _entity)
        {
            Grid[_x, _y].OccupyingEntity = _entity;
            Grid[_x, _y].IsOccupied = true;
        }

        public void RemoveEntityFromGridNode(int _x, int _y)
        {
            Grid[_x, _y].OccupyingEntity = null;
            Grid[_x, _y].IsOccupied = false;
        }

        public bool IsNodeWalkable(int _x, int _y)
        {
            if (!IsOutOfBounds(_x, _y) && Grid[_x, _y].IsWalkable)
            {
                if (!Grid[_x, _y].IsOccupied)
                    return true;
                else if (Grid[_x, _y].IsOccupied && !Grid[_x,_y].OccupyingEntity.IsAlive)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public bool IsOutOfBounds(int _x, int _y)
        {
            if (_x < 0)
                return true;
            if (_y < 0)
                return true;
            if (_x >= Width)
                return true;
            if (_y >= Height)
                return true;
            
            return false;
        }

        public Entity GetEntityInGridPosition(Point _gridPosition)
        {
            return Grid[_gridPosition.X,_gridPosition.Y].OccupyingEntity;
        }

        public bool IsEntityOcuppyingGridPosition(Point _gridPosition)
        {
            if (Grid[_gridPosition.X, _gridPosition.Y].OccupyingEntity != null)
            {
                if (!Grid[_gridPosition.X, _gridPosition.Y].OccupyingEntity.IsAlive)
                {
                    return false;
                }
                return true;
            }
            else
                return false;
        }

        public bool IsEntityNearby(Entity _self, Entity _target)
        {
            Transform selfTransform = _self.GetComponent<Transform>() as Transform;
            Transform targetTransform = _target.GetComponent<Transform>() as Transform;
            Point distance = selfTransform.GridPosition - targetTransform.GridPosition;

            if (Math.Abs(distance.X) <= 1 && Math.Abs(distance.Y) <= 1)
                return true;
            else
                return false;
        }

        public class GridNode
        {
            public Entity OccupyingEntity { get; set; } = null;
            public bool IsOccupied { get; set; } = false;
            public bool IsWalkable { get; set; } = false;
        }
    }
}