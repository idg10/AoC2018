using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Day10
{
    public class Map
    {
        private readonly IEnumerable<Point> _points;

        public Map(IEnumerable<Point> points)
        {
            (MaxX, MaxY, MinX, MinY) = points.Aggregate(
                (width: 0, height: 0, minX: 0, minY: 0),
                (acc, p) => (
                    Math.Max(acc.width, p.X),
                    Math.Max(acc.height, p.Y),
                    Math.Min(acc.minX, p.X),
                    Math.Min(acc.minY, p.Y)));

            Width = MaxX - MinX;
            Height = MaxY - MinY;
            _points = points;
        }

        public int MaxX { get; }

        public int MaxY { get; }

        public int MinX { get; }

        public int MinY { get; }

        public int Width { get; }

        public int Height { get; }

        public void Print()
        {
            var pointMap = _points.Aggregate(
                ImmutableHashSet<(int x, int y)>.Empty,
                (acc, p) => acc.Add((p.X, p.Y)));
            for (int row = 0; row <= MaxY; ++row)
            {
                for (int col = 0; col <= MaxX; ++col)
                {
                    char c = pointMap.Contains((col, row))
                        ? '#'
                        : '.';
                    Console.Write(c);
                }

                Console.WriteLine();
            }
        }
    }
}
