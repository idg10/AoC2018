using System.Collections.Immutable;

namespace Day13
{
    public class Map
    {
        private readonly int width;
        private readonly int height;
        private readonly IImmutableDictionary<(int x, int y), TrackCell> cells;

        public Map(
            int width,
            int height,
            IImmutableDictionary<(int x, int y), TrackCell> cells)
        {
            this.width = width;
            this.height = height;
            this.cells = cells;
        }

        public TrackCell Get(int x, int y) => cells.TryGetValue((x, y), out var result)
            ? result
            : TrackCell.Empty;
    }
}
