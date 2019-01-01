using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Common;

using LanguageExt;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Day13
{
    public static class Program
    {
        public static readonly Parser<TrackCell> pTrackCell = choice(
            CharAs(' ', TrackCell.Empty),
            CharAs('|', TrackCell.NorthSouth),
            CharAs('-', TrackCell.EastWest),
            CharAs('\\', TrackCell.NorthWestSouthEast),
            CharAs('/', TrackCell.NorthEastSouthWest),
            CharAs('+', TrackCell.Intersection));

        public static readonly Parser<CartDirection> pCartDirection = choice(
            CharAs('>', CartDirection.East),
            CharAs('<', CartDirection.West),
            CharAs('^', CartDirection.North),
            CharAs('v', CartDirection.South));

        public static readonly Parser<Action<IMapReceiver>> pInputChar = choice(
            pTrackCell.AsAction((IMapReceiver r, TrackCell c) => r.Cell(c)),
            pCartDirection.AsAction((IMapReceiver r, CartDirection d) => r.Cart(d)),
            endOfLine.AsAction((IMapReceiver r, char _) => r.NewLine()));

        private static void Main()
        {
            Console.WriteLine("Part 1: " + SolvePart1(InputReader.ReadAll(typeof(Program))));
        }

        public static (int x, int y) SolvePart1(string map) => EnumerableEx
            .Generate(
                //LoadMap(map),
                (state: LoadMap(map), collisions: (ICollection<(int x, int y)>) new (int x, int y)[0]),
                _ => true,
                s => (s.state.Update(out var c), c),
                s => s.collisions)
            .SelectMany(c => c)
            .First();

        public interface IMapReceiver
        {
            void Cell(TrackCell cell);
            void Cart(CartDirection direction);
            void NewLine();
        }

        public static State LoadMap(string map)
        {
            var mapr = ProcessLine(many(pInputChar), map).Aggregate(
                new MapReceiver(),
                (m, a) => { a(m); return m; });
            return new State(
                new Map(
                    mapr.Width,
                    mapr.Height,
                    mapr.Cells),
                mapr.Carts);
        }

        private class MapReceiver : IMapReceiver
        {
            private int currentX;
            private int currentY;

            public IImmutableDictionary<(int x, int y), TrackCell> Cells { get; private set; } = ImmutableDictionary<(int x, int y), TrackCell>.Empty;

            public ImmutableList<CartState> Carts { get; private set; } = ImmutableList<CartState>.Empty;

            public int Width { get; private set; } = 0;

            public int Height { get; private set; } = 0;

            public void Cart(CartDirection direction)
            {
                Carts = Carts.Add(new CartState(currentX, currentY, direction, 0));

                switch (direction)
                {
                    case CartDirection.North:
                    case CartDirection.South:
                        Cell(TrackCell.NorthSouth);
                        break;

                    case CartDirection.West:
                    case CartDirection.East:
                        Cell(TrackCell.EastWest);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction));
                }
            }

            public void Cell(TrackCell cell)
            {
                Cells = Cells.Add((currentX, currentY), cell);
                currentX += 1;
                Width = Math.Max(Width, currentX);
            }

            public void NewLine()
            {
                currentY += 1;
                Height = currentY;
                currentX = 0;
            }
        }
    }
}
