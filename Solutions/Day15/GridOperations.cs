using System;
using System.Collections.Generic;
using System.Linq;

namespace Day15
{
    public static class GridOperations
    {
        /// <summary>
        /// Parse a map string into a rectangular array, where the first index is the line
        /// number, and the second index is the column.
        /// </summary>
        /// <param name="map">The string.</param>
        /// <returns>
        /// The map as a rectangular array.
        /// </returns>
        /// <remarks>
        /// Although it's sometimes confusing to use (y, x), we do this because the natural order
        /// in which to process elements in this challenge is generally reading order, and by using
        /// this axis ordering, we ensure that the array elements are arranged in reading order in
        /// memory.
        /// </remarks>
        public static GridCell[,] ParseGrid(string map) => ParseGrid(map.Split("\r\n"));

        private static GridCell[,] ParseGrid(IEnumerable<string> lines)
        {
            int y = 0;
            var allLines = lines.ToList();
            int width = allLines[0].Length;
            var grid = new GridCell[allLines.Count, width];
            int nextGoblinId = 0;
            int nextElfId = 0;
            foreach (string line in lines.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                if (line.Length != width)
                {
                    throw new ArgumentException("All lines must be same length");
                }

                int x = 0;
                foreach (char c in line)
                {
                    GridCell cell;
                    switch (c)
                    {
                        case '#':
                            cell = GridCell.Wall();
                            break;

                        case '.':
                            cell = GridCell.Empty();
                            break;

                        case 'G':
                            cell = GridCell.Goblin(nextGoblinId++);
                            break;

                        case 'E':
                            cell = GridCell.Elf(nextElfId++);
                            break;

                        default:
                            throw new ArgumentException("Unknown map cell type: " + c);
                    }

                    grid[y, x++] = cell;
                }

                y += 1;
            }

            return grid;
        }

        /// <summary>
        /// Populate a grid with information about the closest in range cells.
        /// </summary>
        /// <param name="grid">
        /// A grid array, which must not yet have any closeness information in it.
        /// </param>
        /// <returns>
        /// The grid, populated with closeness information. NOTE: this is the same grid as was
        /// passed in - this method operates in situ.
        /// </returns>
        public static GridCell[,] CalculateCloseness(GridCell[,] grid)
        {
            bool workToDo;
            do
            {
                (grid, workToDo) = ProcessOneClosenessStep(grid);
            } while (workToDo);

            return grid;
        }

        private static (GridCell[,] grid, bool changed) ProcessOneClosenessStep(GridCell[,] lastGrid)
        {
            bool changed = false;
            int height = lastGrid.GetLength(0);
            int width = lastGrid.GetLength(1);
            GridCell[,] newGrid = new GridCell[height, width];
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    GridCell lastCell = lastGrid[y, x];
                    GridCell newCell = lastCell;
                    if (!lastCell.IsWall
                        && ((!lastCell.IsElf && !lastCell.DistanceToElfInRange.HasValue)
                         || (!lastCell.IsGoblin && !lastCell.DistanceToGoblinInRange.HasValue)))
                    {
                        if (!lastCell.IsElf && !newCell.DistanceToElfInRange.HasValue)
                        {
                            var match = FindNearest(
                                c => c.IsElf ? 0 : c.DistanceToElfInRange,
                                (c, _) => c.IsElf ? (x, y) : c.ElfInRangePosition.Value);
                            if (match.HasValue)
                            {
                                (GridCell elfNearest, int hitX, int hitY) = match.Value;
                                int inRangeX, inRangeY;
                                bool hitWasOnUnitItself = !elfNearest.DistanceToElfInRange.HasValue;
                                if (hitWasOnUnitItself)
                                {
                                    inRangeX = x;
                                    inRangeY = y;
                                }
                                else
                                {
                                    (inRangeX, inRangeY) = lastGrid[hitY, hitX].ElfInRangePosition.Value;
                                }
                                newCell = newCell.WithDistanceToElfInRange(
                                    (elfNearest.DistanceToElfInRange ?? 0) + 1,
                                    elfNearest.ElfId.Value,
                                    inRangeX,
                                    inRangeY);
                                changed = true;
                            }
                        }
                        if (!newCell.IsGoblin && !newCell.DistanceToGoblinInRange.HasValue)
                        {
                            var match = FindNearest(
                                c => c.IsGoblin ? 0 : c.DistanceToGoblinInRange,
                                (c, _) => c.IsGoblin ? (x, y) : c.GoblinInRangePosition.Value);
                            if (match.HasValue)
                            {
                                (GridCell goblinNearest, int hitX, int hitY) = match.Value;
                                int inRangeX, inRangeY;
                                bool hitWasOnUnitItself = !goblinNearest.DistanceToGoblinInRange.HasValue;
                                if (hitWasOnUnitItself)
                                {
                                    inRangeX = x;
                                    inRangeY = y;
                                }
                                else
                                {
                                    (inRangeX, inRangeY) = lastGrid[hitY, hitX].GoblinInRangePosition.Value;
                                }
                                newCell = newCell.WithDistanceToGoblinInRange(
                                    (goblinNearest.DistanceToGoblinInRange ?? 0) + 1,
                                    goblinNearest.GoblinId.Value,
                                    inRangeX,
                                    inRangeY);
                                changed = true;
                            }
                        }

                        (GridCell cell, int x, int y)? FindNearest(
                            Func<GridCell, int?> distanceSelector,
                            Func<GridCell, (int x, int y), (int x, int y)> orderingSelector)
                        {
                            return FindBest(
                                lastGrid,
                                x,
                                y,
                                c =>
                                {
                                    var distance = distanceSelector(c);
                                    return distance == 0 ? distance :
                                        (c.IsGoblin || c.IsElf) ? default : distance;
                                },
                                orderingSelector);
                        }
                    }

                    newGrid[y, x] = newCell;
                }
            }

            return (newGrid, changed);
        }

        /// <summary>
        /// Determine the best next move for a unit.
        /// </summary>
        /// <param name="gridWithCloseness">
        /// The grid, which must be populated with closeness details.
        /// </param>
        /// <param name="x">The unit's X coordinate (column).</param>
        /// <param name="y">The unit's Y coordinate (row)</param>
        /// <returns>
        /// Null if no move is available, and otherwise a delta. When a delta is returned it will
        /// always be a unit move either vertically or horizontally.
        /// </returns>
        public static (int dx, int dy)? CalculateMove(GridCell[,] gridWithCloseness, int x, int y)
        {
            GridCell unit = gridWithCloseness[y, x];
            if (!(unit.IsElf || unit.IsGoblin))
            {
                throw new ArgumentException("Grid position must contain unit");
            }
            if ((unit.IsElf && (unit.DistanceToGoblinInRange ?? 0) <= 1)
                || (unit.IsGoblin && (unit.DistanceToElfInRange ?? 0) <= 1))
            {
                return null;
            }
            var d = FindBest(
                gridWithCloseness,
                x,
                y,
                u => u.IsElf || u.IsGoblin ? default : unit.IsElf ? u.DistanceToGoblinInRange : u.DistanceToElfInRange,
                (u, _) => unit.IsElf ? u.GoblinInRangePosition.Value : u.ElfInRangePosition.Value,
                (_, c) => c);

            return d.HasValue ? (d.Value.x - x, d.Value.y - y) : default;
        }

        /// <summary>
        /// Finds the 'best' neighbouring cell on the grid according to the caller's required
        /// criteria.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="x">The X coordinate of the cell for which to find the 'best' neighbour.</param>
        /// <param name="y">The Y coordinate of the cell for which to find the 'best' neighbour.</param>
        /// <param name="rankSelector">
        /// The primary criterion. This function can return null to exclude cells entirely, and
        /// otherwise can return a number. The neighbour producing the lowest number will win.
        /// Ties will fall back to the next argument.
        /// </param>
        /// <param name="positionOrderSelector1">
        /// If multiple neighbours produce the same score with the <c>rankSelector</c>, this
        /// function will be invoked to break ties. It must return a coordinate, and the 'best'
        /// will be the first one by reading order.
        /// </param>
        /// <param name="positionOrderSelector2">
        /// If there is still a tie after taking the <c>rankSelector</c> and <c>positionOrderSelector1</c>
        /// into account, this function will be used, if one is supplied. Again, this uses reading
        /// order to break ties. (This is used in scenarios where there are two locations to take
        /// into account: when picking a move, first we must use reading order to break ties when
        /// multiple 'in-range' cells have the same distance, and then if there are multiple viable
        /// paths to the chosen in-range cell all with the same score, we must again use reading
        /// order the pick the particular path.)
        /// </param>
        /// <returns></returns>
        public static (GridCell cell, int x, int y)? FindBest(
            GridCell[,] grid,
            int x,
            int y,
            Func<GridCell, int?> rankSelector,
            Func<GridCell, (int x, int y), (int x, int y)> positionOrderSelector1,
            Func<GridCell, (int x, int y), (int x, int y)> positionOrderSelector2 = null)
        {
            positionOrderSelector2 = positionOrderSelector2 ?? ((_, __) => (1, 1));
            (int x, int y)[] neighbourCoordinates =
            {
                (x - 1, y),
                (x + 1, y),
                (x, y - 1),
                (x, y + 1)
            };
            return
                (from coords in neighbourCoordinates
                 let c = grid[coords.y, coords.x]
                 where rankSelector(c).HasValue
                 let irp = positionOrderSelector1(c, coords)
                 let xy2 = positionOrderSelector2(c, coords)
                 orderby rankSelector(c), irp.y, irp.x, xy2.y, xy2.x
                 select ((GridCell, int x, int y)?)(c, coords.x, coords.y))
                .FirstOrDefault();
        }
    }
}
