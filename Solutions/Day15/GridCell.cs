using System;

namespace Day15
{
    /// <summary>
    /// Information about a cell on the map. May include 'closeness', i.e. information about the
    /// nearest in-range cells for nearby elves and goblins.
    /// </summary>
    public struct GridCell
    {
        private GridCell(
            bool isWall = false,
            bool isElf = false,
            bool isGoblin = false,
            int? elfId = null,
            int? goblinId = null,
            (int x, int y)? elfInRangePosition = null,
            int? distanceToElfInRange = null,
            (int x, int y)? goblinInRangePosition = null,
            int? distanceToGoblinInRange = null)
        {
            IsWall = isWall;
            IsElf = isElf;
            IsGoblin = isGoblin;
            ElfId = elfId;
            GoblinId = goblinId;
            DistanceToElfInRange = distanceToElfInRange;
            DistanceToGoblinInRange = distanceToGoblinInRange;
            ElfInRangePosition = elfInRangePosition;
            GoblinInRangePosition = goblinInRangePosition;
        }

        public bool IsWall { get; }
        public bool IsElf { get; }
        public bool IsGoblin { get; }

        /// <summary>
        /// Gets the id of the nearest reachable elf (which might be in this very cell).
        /// </summary>
        public int? ElfId { get; }

        /// <summary>
        /// Gets the id of the nearest reachable elf (which might be in this very cell).
        /// </summary>
        public int? GoblinId { get; }

        /// <summary>
        /// If this cell does not contain an elf, but an elf is known to be reachable, this returns
        /// the shortest distance to that elf.
        /// </summary>
        public int? DistanceToElfInRange { get; }

        /// <summary>
        /// If this cell does not contain an goblin, but a goblin is known to be reachable, this
        /// returns the shortest distance to that goblin.
        /// </summary>
        public int? DistanceToGoblinInRange { get; }

        /// <summary>
        /// If this cell does not contain an elf, but an elf is known to be reachable, this returns
        /// the grid location of the nearest 'in-range' cell for that elf.
        /// </summary>
        public (int x, int y)? ElfInRangePosition { get; }

        /// <summary>
        /// If this cell does not contain an goblin, but an goblin is known to be reachable, this returns
        /// the grid location of the nearest 'in-range' cell for that goblin.
        /// </summary>
        public (int x, int y)? GoblinInRangePosition { get; }

        /// <summary>
        /// Creates an empty cell.
        /// </summary>
        /// <returns>The cell.</returns>
        public static GridCell Empty() => new GridCell();

        /// <summary>
        /// Creates a cell representing a wall.
        /// </summary>
        /// <returns>The cell.</returns>
        public static GridCell Wall() => new GridCell(isWall: true);

        /// <summary>
        /// Creates a cell containing a goblin.
        /// </summary>
        /// <param name="id">The goblin's unique id.</param>
        /// <returns>The cell.</returns>
        public static GridCell Goblin(int id) => new GridCell(isGoblin: true, goblinId: id);

        /// <summary>
        /// Creates a cell containing a elf.
        /// </summary>
        /// <param name="id">The elf's unique id.</param>
        /// <returns>The cell.</returns>
        public static GridCell Elf(int id) => new GridCell(isElf: true, elfId: id);

        /// <summary>
        /// String representation of cell (used during debugging) .
        /// </summary>
        /// <returns>A representation of the cell.</returns>
        public override string ToString()
        {
            return IsWall
                ? " # "
                : IsElf ? $"E {ElfId}" : IsGoblin ? $"G {GoblinId}" :
                $"{DistanceToElfInRange?.ToString() ?? " "}.{DistanceToGoblinInRange?.ToString() ?? " "}";
        }

        /// <summary>
        /// Returns a modified version of the cell, adding information about the nearest elf
        /// in range.
        /// </summary>
        /// <param name="distance">The distance to the nearest reachable elf.</param>
        /// <param name="id">The id of the nearest reachable elf.</param>
        /// <param name="x">The X coordinate of the nearest reachable in-range cell for the elf.</param>
        /// <param name="x">The Y coordinate of the nearest reachable in-range cell for the elf.</param>
        /// <returns>An updated copy of the cell.</returns>
        public GridCell WithDistanceToElfInRange(
            int distance,
            int id,
            int x,
            int y)
        {
            if (IsWall) { throw new InvalidOperationException("Cannot set distance on wall"); }
            if (IsElf) { throw new InvalidOperationException("Cannot set Elf distance on Elf"); }
            return new GridCell(
                isGoblin: IsGoblin,
                elfId: id,
                goblinId: GoblinId,
                distanceToElfInRange: distance,
                elfInRangePosition: (x, y),
                distanceToGoblinInRange: DistanceToGoblinInRange,
                goblinInRangePosition: GoblinInRangePosition);
        }

        /// <summary>
        /// Returns a modified version of the cell, adding information about the nearest goblin
        /// in range.
        /// </summary>
        /// <param name="distance">The distance to the nearest reachable goblin.</param>
        /// <param name="id">The id of the nearest reachable goblin.</param>
        /// <param name="x">The X coordinate of the nearest reachable in-range cell for the goblin.</param>
        /// <param name="x">The Y coordinate of the nearest reachable in-range cell for the goblin.</param>
        /// <returns>An updated copy of the cell.</returns>
        public GridCell WithDistanceToGoblinInRange(
            int distance,
            int id,
            int x,
            int y)
        {
            if (IsWall) { throw new InvalidOperationException("Cannot set distance on wall"); }
            if (IsGoblin) { throw new InvalidOperationException("Cannot set Goblin distance on Goblin"); }
            return new GridCell(
                isElf: IsElf,
                elfId: ElfId,
                goblinId: id,
                distanceToGoblinInRange: distance,
                goblinInRangePosition: (x, y),
                distanceToElfInRange: DistanceToElfInRange,
                elfInRangePosition: ElfInRangePosition);
        }
    }
}
