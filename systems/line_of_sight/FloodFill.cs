using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace MonoRPG
{
    public class FloodFill
    {
        MapGrid grid;
        List<MapGrid.GridNode> gridSpaces = new List<MapGrid.GridNode>();

        public FloodFill(MapGrid _grid)
        {
            grid = _grid;
        }

        public void Start(Point _startingPoint, int _distance)
        {
            gridSpaces.Clear();
            if (grid.IsOutOfBounds(_startingPoint.X, _startingPoint.Y) || !grid.Grid[_startingPoint.X, _startingPoint.Y].IsWalkable)
                return;


            List<FloodFillNode> nodes = new List<FloodFillNode>();
            List<FloodFillNode> openNodes = new List<FloodFillNode>();
            int currentDistance = 0;

            FloodFillNode startingNode = new FloodFillNode(grid.Grid[_startingPoint.X, _startingPoint.Y].IsWalkable, true, _startingPoint);
            nodes.Add(startingNode);
            openNodes.Add(startingNode);


            while (currentDistance < _distance)
            {
                List<FloodFillNode> discoveredNodes = new List<FloodFillNode>();

                for (int i = 0; i < openNodes.Count; ++i)
                {
                    // check in four directions if there is a vaild grid space there.
                    // if is check to see if it is walkable
                    // if is walkable, check to see if node has already been discovered.
                    // if not create a new flood fill node, if not walkable, then it set bool canContinue as false
                    if (!openNodes[i].CanContinue)
                    {
                        continue;
                    }

                    List<Point> surroundingPositions = GetSurroundingPositions(openNodes[i].GridPosition);

                    foreach(Point surroundingPosition in surroundingPositions)
                    {
                        bool isDiscoveredNode = false;

                        foreach(FloodFillNode node in nodes)
                        {
                            if (node.GridPosition == surroundingPosition)
                            {
                                isDiscoveredNode = true;
                            }
                        }

                        if (!isDiscoveredNode)
                        {
                            // create a new node
                            // add it to nodes and discovered nodes
                            FloodFillNode discoveredNode = new FloodFillNode(grid.Grid[surroundingPosition.X, surroundingPosition.Y].IsWalkable, 
                                                                            grid.Grid[surroundingPosition.X, surroundingPosition.Y].IsWalkable, 
                                                                            surroundingPosition);
                            discoveredNodes.Add(discoveredNode);
                            nodes.Add(discoveredNode);
                        }
                    }
                }

                openNodes.Clear();
                currentDistance++;
                
                foreach(FloodFillNode node in discoveredNodes)
                {
                    openNodes.Add(node);
                }
            }

            foreach(FloodFillNode node in nodes)
            {
                gridSpaces.Add(grid.Grid[node.GridPosition.X, node.GridPosition.Y]);
            }
        }

        public List<Point> GetAllWalkablePositions()
        {
            List<Point> walkablePositions = new List<Point>();

            foreach(MapGrid.GridNode node in gridSpaces)
            {
                if (node.IsWalkable)
                    walkablePositions.Add(new Point(node.Position.X, node.Position.Y));
            }

            return walkablePositions;
        }

        private List<Point> GetSurroundingPositions(Point _initalPoint)
        {
            List<Point> points = new List<Point>();

            if (!grid.IsOutOfBounds(_initalPoint.X - 1, _initalPoint.Y) && grid.Grid[_initalPoint.X - 1, _initalPoint.Y] != null)
            {
                points.Add(new Point(_initalPoint.X - 1, _initalPoint.Y));
            }
            if (!grid.IsOutOfBounds(_initalPoint.X + 1, _initalPoint.Y) && grid.Grid[_initalPoint.X + 1, _initalPoint.Y] != null)
            {
                points.Add(new Point(_initalPoint.X + 1, _initalPoint.Y));
            }
            if (!grid.IsOutOfBounds(_initalPoint.X, _initalPoint.Y + 1) && grid.Grid[_initalPoint.X, _initalPoint.Y + 1] != null)
            {
                points.Add(new Point(_initalPoint.X, _initalPoint.Y + 1));
            }
            if (!grid.IsOutOfBounds(_initalPoint.X, _initalPoint.Y - 1) && grid.Grid[_initalPoint.X, _initalPoint.Y - 1] != null)
            {
                points.Add(new Point(_initalPoint.X, _initalPoint.Y - 1));
            }

            return points;
        }

        class FloodFillNode
        {
            public bool IsWalkable { get; private set; }
            public bool CanContinue { get; private set; }
            public Point GridPosition { get; private set; }

            public FloodFillNode(bool _isWalkable, bool _canContinue, Point _gridPosition)
            {
                IsWalkable = _isWalkable;
                CanContinue = _canContinue;
                GridPosition = _gridPosition;
            }
        }
    }
}