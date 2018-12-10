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

namespace Day06
{
    /// <summary>
    /// Day 6 of the 2018 Advent of Code.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The problem requires us to identify the non-infinite regions in the problem statement.
    /// A region is an area in which all the points are closer to one of the input points than
    /// to any other, measured using Manhattan distance. 
    /// </para>
    /// <para>
    /// A point's region is partially bounded by each other point. The bound depends on which
    /// quadrant it falls in. The quadrants are offset from the axes by 45 degrees:
    /// </para>
    /// <code>
    ///  \       /
    ///   \ Top /
    ///    \   /
    ///     \ /
    /// Left p Right
    ///     / \
    ///    /   \
    ///   /     \
    ///  /       \
    /// / Bottom  \
    /// </code>
    /// <para>
    /// Some point p is constrained from above by any point that falls in its Top quadrant.
    /// for example, if point A is at (10, 10), another point B produces this:
    /// </para>
    /// <code>
    /// bbbbbbbbbbbbbbbbbbb
    /// bbbbbbbbbbbbbbbbbbb
    /// bbbbbbbbbbbbbbbbbbb
    /// bbbbbbbbbbbbbbbbbbb
    /// bbbbbbbbbbbbbbbbbbb
    /// bbbbbbBbbbbbbbbbbbb
    /// bbbbbbbbbbaaaaaaaaa
    /// bbbbbbbbbaaaaaaaaaa
    /// bbbbbbbbaaaaaaaaaaa
    /// bbbbbbbaaaaaaaaaaaa
    /// aaaaaaaaaaAaaaaaaaa
    /// aaaaaaaaaaaaaaaaaaa
    /// aaaaaaaaaaaaaaaaaaa
    /// aaaaaaaaaaaaaaaaaaa
    /// aaaaaaaaaaaaaaaaaaa
    /// aaaaaaaaaaaaaaaaaaa
    /// aaaaaaaaaaaaaaaaaaa
    /// aaaaaaaaaaaaaaaaaaa
    /// aaaaaaaaaaaaaaaaaaa
    /// </code>
    /// <para>
    /// When a point straddles a quadrant boundary of another point, it constrains it in both
    /// directions, e.g., point A at (10, 10) is constraint by point B at (5, 5) thus:
    /// </para>
    /// <code>
    /// bbbbbbbbbb.........
    /// bbbbbbbbbb.........
    /// bbbbbbbbbb.........
    /// bbbbbbbbbb.........
    /// bbbbbbbbbb.........
    /// bbbbbBbbbb.........
    /// bbbbbbbbb.aaaaaaaaa
    /// bbbbbbbb.aaaaaaaaaa
    /// bbbbbbb.aaaaaaaaaaa
    /// bbbbbb.aaaaaaaaaaaa
    /// ......aaaaAaaaaaaaa
    /// ......aaaaaaaaaaaaa
    /// ......aaaaaaaaaaaaa
    /// ......aaaaaaaaaaaaa
    /// ......aaaaaaaaaaaaa
    /// ......aaaaaaaaaaaaa
    /// ......aaaaaaaaaaaaa
    /// ......aaaaaaaaaaaaa
    /// ......aaaaaaaaaaaaa
    /// </code>
    /// <para>
    /// So a point's region is finite if each of its quadrants contains at least one other point.
    /// This can be done by a minimum of 2 points if each of those straddle quadrant boundaries.
    /// For example, point A here at (10, 10) is finite because of constrainst imposed by point
    /// B at (5, 5) and C at (15, 15):
    /// </para>
    /// <code>
    /// bbbbbbbbbb.........
    /// bbbbbbbbbb.........
    /// bbbbbbbbbb.........
    /// bbbbbbbbbb.........
    /// bbbbbbbbbb.........
    /// bbbbbBbbbb.........
    /// bbbbbbbbb.aaaaa....
    /// bbbbbbbb.aaaaaa....
    /// bbbbbbb.aaaaaaa....
    /// bbbbbb.aaaaaaaa....
    /// ......aaaaAaaaa....
    /// ......aaaaaaaa.cccc
    /// ......aaaaaaa.ccccc
    /// ......aaaaaa.cccccc
    /// ......aaaaa.ccccccc
    /// ...........ccccCccc
    /// ...........cccccccc
    /// ...........cccccccc
    /// ...........cccccccc
    /// </code>
    /// </remarks>
    public static class Program
    {
        private static Func<string, (int x, int y)> parseLine = LineProcessor(pInt32CommaInt32);

        static void Main(string[] args)
        {
            string[] testInput =
            {
                "1, 1",
                "1, 6",
                "8, 3",
                "3, 4",
                "5, 5",
                "8, 9"
            };

            IImmutableList<(int x, int y)> exampleInput = testInput
                .Select(parseLine)
                .ToImmutableList();

            IImmutableDictionary<(int x, int y), (int x, int y)> exampleMap = PopulateRegions(10, 10, exampleInput);

            PrintMap(exampleMap, exampleInput);

            Console.WriteLine();

            PrintMapFor(20, 20, new[] { (10, 10), (5, 5), (15, 15) });
            Console.WriteLine();

            //PrintMapFor(20, 20, new[] { (10, 10), (1, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (2, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (3, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (4, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (5, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (6, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (7, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (8, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (9, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (10, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (11, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (12, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (13, 7) });
            //Console.WriteLine();
            //PrintMapFor(20, 20, new[] { (10, 10), (14, 7) });
        }

        private static void PrintMapFor(int w, int h, (int, int)[] p)
        {
            PrintMap(PopulateRegions(w, h, p), p);
        }

        public static IImmutableDictionary<(int x, int y), (int x, int y)> PopulateRegions(
            int w, int h, IEnumerable<(int x, int y)> inputPoints)
        {
            var q =
                from x in Enumerable.Range(0, w)
                from y in Enumerable.Range(0, h)
                let regionCentres = inputPoints.MinBy(p => ManhattanDistance((x, y), p))
                where regionCentres.Count == 1
                select (p: (x, y), regionCentre: regionCentres.Single());

            return q.ToImmutableDictionary(x => x.p, x => x.regionCentre);
        }

        public static int ManhattanDistance(int x, int y) => x + y;

        public static int ManhattanDistance((int x, int y) p1, (int x, int y) p2) => ManhattanDistance(Math.Abs(p1.x - p2.x), Math.Abs(p1.y - p2.y));

        public static void PrintMap(
            IImmutableDictionary<(int x, int y), (int x, int y)> data,
            IEnumerable<(int x, int y)> points)
        {
            (int w, int h) = data.Aggregate((x: 0, y: 0), (d, p) => (Math.Max(d.x, p.Key.x), Math.Max(d.y, p.Key.y)));

            var pointToLabelMap = points
                .Select((p, i) => (p, i))
                .Aggregate(
                    ImmutableDictionary<(int x, int y), char>.Empty,
                    (d, x) => d.Add(x.p, (char) ('a' + ((char) (x.i % 26)))));

            for (int row = 0; row < h; ++row)
            {
                for (int col = 0; col < w; ++col)
                {
                    char c = '.';
                    (int x, int y) mapPoint = (col, row);
                    if (data.TryGetValue(mapPoint, out (int x, int y) centre))
                    {
                        c = pointToLabelMap[centre];

                        if (centre.Equals(mapPoint))
                        {
                            c = char.ToUpper(c);
                        }
                    }
                    Console.Write(c);
                }

                Console.WriteLine();
            }
        }
    }
}
