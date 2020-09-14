using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace MonoRPG
{
    public class LineOfSight
    {
        Entity caster;
        MapGrid grid;

        public LineOfSight(Entity _caster, MapGrid _grid)
        {
            caster = _caster;
            grid = _grid;
        }

        public bool IsLineToTargetClear(Entity _target)
        {
            Transform casterTransform = caster.GetComponent<Transform>() as Transform;
            Transform targetTransform = _target.GetComponent<Transform>() as Transform;
            List<Point> lineCoords = PlotLine(casterTransform.GridPosition.X, casterTransform.GridPosition.Y, targetTransform.GridPosition.X, targetTransform.GridPosition.Y);
            
            foreach (Point coord in lineCoords)
            {
                if (grid.IsOutOfBounds(coord.X, coord.Y))
                    return false;

                if (!grid.Grid[coord.X, coord.Y].IsWalkable)
                    return false;
            }

            return true;
        }

        public bool IsLineClear(Point _destination)
        {
            Transform casterTransform = caster.GetComponent<Transform>() as Transform;
            List<Point> lineCoords = PlotLine(casterTransform.GridPosition.X, casterTransform.GridPosition.Y, _destination.X, _destination.Y);

            foreach (Point coord in lineCoords)
            {
                if (grid.IsOutOfBounds(coord.X, coord.Y))
                    return false;
                
                if (!grid.IsNodeWalkable(coord.X, coord.Y))
                    return false;
            }

            return true;
        }

        private List<Point> PlotLine(int x0, int y0, int x1, int y1)
        {
            List<Point> lineCoords = new List<Point>();

            int dx = Math.Abs(x1 - x0), sx = x0<x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0), sy = y0<y1 ? 1 : -1;
            int err = dx + dy, e2;

            while (true)
            {
                lineCoords.Add(new Point(x0, y0));

                if (x0 == x1 && y0 == y1) break;

                e2 = 2 * err;

                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }

            lineCoords.RemoveAt(0); // Remove first entry since it would just be the entity casting
            lineCoords.RemoveAt(lineCoords.Count - 1); // Remove the last entry sicne it's just the target

            return lineCoords;
        }
    }
}