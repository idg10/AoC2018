using System;
using System.Collections.Generic;
using System.Text;

namespace Day10
{
    public class Point
    {
        public Point(
            int x,
            int y,
            int dx,
            int dy)
        {
            X = x;
            Y = y;
            Dx = dx;
            Dy = dy;
        }

        public int X { get; }
        public int Y { get; }
        public int Dx { get; }
        public int Dy { get; }

        public Point Next()
        {
            return new Point(X + Dx, Y + Dy, Dx, Dy);
        }
    }
}
