using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class Pathfinder
    {
        MapGrid grid;

        public Pathfinder(MapGrid _grid)
        {
            grid = _grid;
        }

        public List<Point> GetPath(Point _start, Point _end)
        {
            List<PathfinderNode> openList = new List<PathfinderNode>();
            List<PathfinderNode> closedList = new List<PathfinderNode>();

            PathfinderNode startNode = new PathfinderNode(PathfinderNode.NodeType.START, null, _start);
            PathfinderNode endNode = new PathfinderNode(PathfinderNode.NodeType.END, null, _end);

            openList.Add(startNode);

            while (openList.Count > 0)
            {
                PathfinderNode currentNode = openList[0];

                foreach (PathfinderNode node in openList)
                {
                    if (node.Cost < currentNode.Cost)
                        currentNode = node;
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                if (currentNode.SetNodeType == PathfinderNode.NodeType.END)
                {
                    return TracePath(endNode, startNode);
                }

                List<PathfinderNode> children = GetChildrenNodes(currentNode, endNode);
                foreach (PathfinderNode child in children)
                {
                    bool childIsAlreadyFound = false;

                    foreach (PathfinderNode node in closedList)
                    {
                        if (child.GridPosition == node.GridPosition)
                            childIsAlreadyFound = true;
                    }

                    foreach (PathfinderNode node in openList)
                    {
                        if (child.GridPosition == node.GridPosition)
                            childIsAlreadyFound = true;
                    }

                    if (!childIsAlreadyFound)
                        openList.Add(child);
                }
            }

            return new List<Point>();  // No path was found
        }

        private List<Point> TracePath(PathfinderNode _endNode, PathfinderNode _startNode)
        {
            List<Point> path = new List<Point>();
            PathfinderNode currentNode = _endNode;

            while (currentNode != _startNode)
            {
                if (!grid.IsEntityOcuppyingGridPosition(currentNode.GridPosition))
                    path.Add(currentNode.GridPosition);
                currentNode = currentNode.ParentNode;
            }

            path.Reverse();

            return path;
        }

        private List<PathfinderNode> GetChildrenNodes(PathfinderNode _currentNode, PathfinderNode _endNode)
        {
            List<PathfinderNode> children = new List<PathfinderNode>();

            foreach (Point direction in GetDirections())
            {
                Point currentPosition = _currentNode.GridPosition + direction;
                
                if (grid.IsNodeWalkable(currentPosition.X, currentPosition.Y) || _endNode.GridPosition == currentPosition)
                {
                    if (currentPosition == _endNode.GridPosition)
                    {
                        _endNode.SetParent(_currentNode);
                        children.Add(_endNode);
                    }
                    else
                    {
                        PathfinderNode newNode = new PathfinderNode(PathfinderNode.NodeType.REGULAR, _currentNode, currentPosition);
                        newNode.SetCost(_endNode);
                        children.Add(newNode);
                    }

                }
            }

            return children;
        }

        private List<Point> GetDirections()
        {
            List<Point> directions = new List<Point>();

            directions.Add(new Point(1,0));
            directions.Add(new Point(-1,0));
            directions.Add(new Point(0,1));
            directions.Add(new Point(0,-1));

            return directions;
        }

        class PathfinderNode
        {
            public enum NodeType { START, END, REGULAR }
            public NodeType SetNodeType { get; private set; }
            public int Cost { get; private set; }
            public int GCost { get; private set; }
            public int HCost { get; private set; }
            public PathfinderNode ParentNode { get; private set; }
            public Point GridPosition { get; private set; }

            public PathfinderNode(NodeType _setNodeType, PathfinderNode _parentNode, Point _gridPosition)
            {
                SetNodeType = _setNodeType;
                ParentNode = _parentNode;
                GridPosition = _gridPosition;
            }

            public void SetCost(PathfinderNode _endNode) 
            {
                GCost = ParentNode.GCost + 1;
                HCost = (int)Math.Pow(Math.Abs(GridPosition.X - _endNode.GridPosition.X), 2) + (int)Math.Pow(Math.Abs(GridPosition.Y - _endNode.GridPosition.Y), 2);
                Cost = GCost + HCost;
            }

            public void SetParent(PathfinderNode _parentNode)
            {
                ParentNode = _parentNode;
            }
        }
    }
}