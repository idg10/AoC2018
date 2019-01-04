using System;
using System.Collections.Generic;
using System.Text;

namespace Day15
{
    public struct GridPair
    {
        private GridPair(GridCell[,] primary, GridCell[,] secondary)
        {
            Primary = primary;
            Secondary = secondary;
        }

        public static GridPair For(GridCell[,] grid)
        {
            int height = grid.GetLength(0);
            int width = grid.GetLength(1);
            return new GridPair(grid, new GridCell[height, width]);
        }

        public GridCell[,] Primary { get; }
        public GridCell[,] Secondary { get; }

        public GridPair Swap()
        {
            return new GridPair(Secondary, Primary);
        }
    }
}
