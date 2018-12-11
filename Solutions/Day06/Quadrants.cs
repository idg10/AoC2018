using System;

namespace Day06
{
    /// <summary>
    /// Quadrants, at a 45 degree offset from the axes.
    /// </summary>
    [Flags]
    public enum Quadrants
    {
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8
    }
}
