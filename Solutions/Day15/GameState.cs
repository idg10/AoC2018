using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Day15
{
    public sealed class GameState
    {
        public static GameState Start(
            GridCell[,] grid,
            int elfAttackPower = 3)
        {
            var gridPair = GridPair.For(grid);
            GridOperations.CalculateCloseness(gridPair);
            var elfHitPoints = ImmutableDictionary<int, int>.Empty;
            var goblinHitPoints = ImmutableDictionary<int, int>.Empty;
            int height = gridPair.Primary.GetLength(0);
            int width = gridPair.Primary.GetLength(1);
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    GridCell cell = gridPair.Primary[y, x];
                    if (cell.IsElf)
                    {
                        elfHitPoints = elfHitPoints.Add(cell.ElfId.Value, 200);
                    }
                    else if (cell.IsGoblin)
                    {
                        goblinHitPoints = goblinHitPoints.Add(cell.GoblinId.Value, 200);
                    }
                }
            }

            return new GameState(gridPair, elfHitPoints, goblinHitPoints, elfAttackPower);
        }

        private readonly GridPair _gridPair;

        private GameState(
            GridPair gridPair,
            IImmutableDictionary<int, int> elfHitPoints,
            IImmutableDictionary<int, int> goblinHitPoints,
            int elfAttackPower)
        {
            ElfHitPoints = elfHitPoints;
            GoblinHitPoints = goblinHitPoints;
            ElfAttackPower = elfAttackPower;
            _gridPair = gridPair;
        }

        public GridCell[,] Grid => _gridPair.Primary;
        public IImmutableDictionary<int, int> ElfHitPoints { get; }
        public IImmutableDictionary<int, int> GoblinHitPoints { get; }
        public int ElfAttackPower { get; }

        public (GameState state, bool combatEnds) PlayRound()
        {
            bool combatEnds = false;

            var coordinates = new List<(int x, int y)>();
            int height = Grid.GetLength(0);
            int width = Grid.GetLength(1);
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    GridCell cell = Grid[y, x];
                    if (cell.IsElf || cell.IsGoblin)
                    {
                        coordinates.Add((x, y));
                    }
                }
            }

            var elfHitPoints = ElfHitPoints;
            var goblinHitPoints = GoblinHitPoints;
            GridCell[,] grid = Grid;
            for (int i = 0; i < coordinates.Count; ++i)
            {
                var xy = coordinates[i];

                GridCell unitCell = grid[xy.y, xy.x];
                if (!(unitCell.IsElf || unitCell.IsGoblin))
                {
                    // Already taken out in an earlier turn this round.
                    continue;
                }

                if ((unitCell.IsElf && !goblinHitPoints.Any(kv => kv.Value > 0))
                    || (unitCell.IsGoblin && !elfHitPoints.Any(kv => kv.Value > 0)))
                {
                    combatEnds = true;
                    break;
                }

                int hitPower = unitCell.IsElf
                    ? elfHitPoints[unitCell.ElfId.Value]
                    : goblinHitPoints[unitCell.GoblinId.Value];

                // Move if appropriate.
                var move = GridOperations.CalculateMove(grid, xy.x, xy.y);
                if (move.HasValue)
                {
                    bool isElf = unitCell.IsElf;
                    int unitId = isElf ? unitCell.ElfId.Value : unitCell.GoblinId.Value;

                    RemoveItem(height, width, grid, xy);

                    var newCoordinates = (x: xy.x + move.Value.dx, y: xy.y + move.Value.dy);
                    grid[newCoordinates.y, newCoordinates.x] = isElf
                        ? GridCell.Elf(unitId)
                        : GridCell.Goblin(unitId);
                    GridOperations.CalculateCloseness(_gridPair);
                    coordinates[i] = newCoordinates;
                }

                // Attack if appropriate.
                xy = coordinates[i];
                var match = GridOperations.FindBest(
                    grid,
                    xy.x,
                    xy.y,
                    c =>
                    {
                        bool isTarget = unitCell.IsElf ? c.IsGoblin : c.IsElf;
                        return isTarget
                            ? NullIfDead(unitCell.IsElf ? goblinHitPoints[c.GoblinId.Value] : elfHitPoints[c.ElfId.Value])
                            : default;

                        int? NullIfDead(int v) => v > 0 ? v : default;
                    },
                    (_, loc) => loc);
                if (match.HasValue)
                {
                    int hitPoints;
                    if (unitCell.IsElf)
                    {
                        int goblinId = match.Value.cell.GoblinId.Value;
                        hitPoints = goblinHitPoints[goblinId] - ElfAttackPower;
                        goblinHitPoints = goblinHitPoints.SetItem(
                            goblinId,
                            hitPoints);
                    }
                    else
                    {
                        int elfId = match.Value.cell.ElfId.Value;
                        hitPoints = elfHitPoints[elfId] - 3;
                        elfHitPoints = elfHitPoints.SetItem(
                            elfId,
                            hitPoints);
                    }

                    if (hitPoints <= 0)
                    {
                        RemoveItem(height, width, grid, (match.Value.x, match.Value.y));
                        GridOperations.CalculateCloseness(_gridPair);
                    }
                }
            }

            return (new GameState(_gridPair, elfHitPoints, goblinHitPoints, ElfAttackPower), combatEnds);
        }

        private static void RemoveItem(int height, int width, GridCell[,] grid, (int x, int y) xy)
        {
            // Reset closeness information, and remove this unit
            grid[xy.y, xy.x] = GridCell.Empty();
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    GridCell cell = grid[y, x];
                    if (!(cell.IsWall || cell.IsElf || cell.IsGoblin))
                    {
                        grid[y, x] = GridCell.Empty();
                    }
                    else if (cell.IsElf)
                    {
                        // Drop all distance info.
                        grid[y, x] = GridCell.Elf(cell.ElfId.Value);
                    }
                    else if (cell.IsGoblin)
                    {
                        // Drop all distance info.
                        grid[y, x] = GridCell.Goblin(cell.GoblinId.Value);
                    }
                }
            }
        }
    }
}
