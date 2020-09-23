using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

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
            Grid[_x, _y].OccupyingEntities.Add(_entity);

            if (Grid[_x, _y].OccupyingEntities.Count > 1)
                Grid[_x, _y].OccupyingEntity = Grid[_x, _y].OccupyingEntities[0];
            else
                Grid[_x, _y].OccupyingEntity = _entity;
        }

        public void RemoveEntityFromGridNode(int _x, int _y, Entity _entity)
        {
            Grid[_x, _y].OccupyingEntities.Remove(_entity);
            if (Grid[_x, _y].OccupyingEntities.Count > 0)
            {
                Grid[_x, _y].OccupyingEntity = Grid[_x, _y].OccupyingEntities[0];
            }
            else
            {
                Grid[_x, _y].OccupyingEntity = null;
            }
        }

        public bool IsNodeWalkable(int _x, int _y)
        {
            if (!IsOutOfBounds(_x, _y) && Grid[_x, _y].IsWalkable)
            {
                if (Grid[_x, _y].OccupyingEntity == null)
                    return true;

                if (Grid[_x, _y].OccupyingEntities.Count > 1)
                {
                    foreach (Entity e in Grid[_x, _y].OccupyingEntities)
                    {
                        if (!e.IsWalkable)
                            return false;
                    }
                    return true;
                }
                else
                {
                    if (!Grid[_x, _y].OccupyingEntity.IsWalkable)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
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

        public List<Entity> GetEntitiesInGridPosition(Point _gridPosition)
        {
            return Grid[_gridPosition.X, _gridPosition.Y].OccupyingEntities;
        }

        public bool IsEntityOcuppyingGridPosition(Point _gridPosition)
        {
            if (Grid[_gridPosition.X, _gridPosition.Y].OccupyingEntity == null)
                return false;
            else
                return true;
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

        public bool IsEntityInNearbySquare(Entity _self, Entity _target)
        {
            Transform selfTransform = _self.GetComponent<Transform>() as Transform;

            Point[] directions = new Point[4];
            directions[0] = new Point(1,0);
            directions[1] = new Point(-1,0);
            directions[2] = new Point(0,1);
            directions[3] = new Point(0,-1);

            foreach(Point direction in directions)
            {
                Point nodeToCheck = selfTransform.GridPosition + direction;
                foreach(Entity entity in Grid[nodeToCheck.X, nodeToCheck.Y].OccupyingEntities)
                {
                    if (entity == _target)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public class GridNode
        {
            public Entity OccupyingEntity { get; set; } = null;
            public List<Entity> OccupyingEntities { get; private set; } = new List<Entity>();
            public bool IsWalkable { get; set; } = false;
        }
    }
}