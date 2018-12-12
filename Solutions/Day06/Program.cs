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
    /// For example, point A here at (10, 10) is finite because of constraints imposed by point
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
    /// <para>
    /// Thus we can narrow down the regions of interest. The next question is: how large an area
    /// do we need to consider? Looking at the images above, it looks like for a particular
    /// direction of constraint, e.g., if B constrains A from above, then the constrained region
    /// cannot go past the line containing that constrained point, e.g. A's region cannot get
    /// higher than B if B constrains it from above. The reason for this is that for B to constrain
    /// A from above, it must be within the top quadrant, which has the effect that the shortest
    /// distance from B to a vertical line containing A must necessarily be less than or equal to
    /// the shortest distance from A to that same point. (In the case where B straddles a quadrant
    /// boundary, those distances will be equal, and if B is strictly inside the quadrant, its
    /// distance to that point will be shorter). This means that B excludes A from that point,
    /// and since further moves away from B will also increase the distance from A, any moves
    /// further along the line will continue to exclude A.
    /// </para>
    /// <para>
    /// The upshot of this is that we can take the bounding box of all the points and calculate
    /// region occupancy for that, because points on the edge of the box will be infinite, and
    /// any finite regions will necessarily be unable to extend past the box defined by those
    /// points.
    /// </para>
    /// </remarks>
    public static class Program
    {
        private readonly static string[] exampleInputLines =
            {
                "1, 1",
                "1, 6",
                "8, 3",
                "3, 4",
                "5, 5",
                "8, 9"
            };

        private static Func<string, (int x, int y)> parseLine = LineProcessor(pInt32CommaInt32);

        public readonly static IImmutableList<(int x, int y)> ExampleInput = exampleInputLines
            .Select(parseLine)
            .ToImmutableList();


        static void Main(string[] args)
        {
            IImmutableDictionary<(int x, int y), (int x, int y)> exampleMap = PopulateRegions(10, 10, ExampleInput);

            PrintMap(exampleMap, ExampleInput);

            Console.WriteLine();

            int part1ExampleResult = SolvePart1(ExampleInput);
            Console.WriteLine("Example part 1: " + part1ExampleResult);

            int part1Result = SolvePart1(InputReader.ParseLines(typeof(Program), pInt32CommaInt32).ToImmutableList());
            Console.WriteLine("Part 1: " + part1Result);

            //PrintMapFor(20, 20, new[] { (10, 10), (5, 5), (15, 15) });
            //Console.WriteLine();

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

        public static int SolvePart1(IImmutableList<(int x, int y)> input)
        {
            var size = Program.GetBoundingBoxFromOrigin(input);
            var data = PopulateRegions(size.w, size.h, input);
            var sizes = GetFiniteRegionSizes(data, input);
            return sizes.Max(s => s.Value);
        }

        private static void PrintMapFor(int w, int h, (int, int)[] p)
        {
            PrintMap(PopulateRegions(w, h, p), p);
        }

        public static IEnumerable<(int x, int y)> GetFiniteRegionCentres(
            IEnumerable<(int x, int y)> inputPoints)
        {
            Quadrants GetPointQuadrants((int x, int y) p) => inputPoints
                    .Where(p2 => p2 != p)
                    .Aggregate(
                        default(Quadrants),
                        (q, p2) => q | GetQuadrants(p, p2));

            return inputPoints
                .Where(p => GetPointQuadrants(p) == Quadrants.All);
        }

        public static (int w, int h) GetBoundingBoxFromOrigin(
            IEnumerable<(int x, int y)> inputPoints) => inputPoints.Aggregate(
                (w: 0, h: 0),
                (d, p) => (Math.Max(d.w, p.x), Math.Max(d.h, p.y)));

        public static IImmutableDictionary<(int x, int y), int> GetFiniteRegionSizes(
            IImmutableDictionary<(int x, int y), (int x, int y)> data,
            IEnumerable<(int x, int y)> inputPoints)
        {
            var finiteRegionCentres = new System.Collections.Generic.HashSet<(int x, int y)>(GetFiniteRegionCentres(inputPoints));

            return GetRegionSizes(data)
                .Where(p => finiteRegionCentres.Contains(p.centre))
                .ToImmutableDictionary(p => p.centre, p => p.size);
        }

        public static IEnumerable<((int x, int y) centre, int size)> GetRegionSizes(
            IImmutableDictionary<(int x, int y), (int x, int y)> data) => data
            .GroupBy(
                p => p.Value,
                p => p.Key)
            .Select(g => (g.Key, g.Count()));

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

        /// <summary>
        /// Determine which quadrants a point belongs to.
        /// </summary>
        /// <param name="centre">The centre from which to test.</param>
        /// <param name="pointToTest">The point to test.</param>
        /// <returns>
        /// The quadrant or (if a point straddles a quadrant boundary) quadrants in which the
        /// point to test lies relative to the centre.
        /// </returns>
        /// <remarks>
        /// The quadrant boundaries are at a 45 degree offset to the axes, so instead of the
        /// more usual top right, bottom right, bottom left, top left, we use just top, right
        /// bottom, and left.
        /// </remarks>
        public static Quadrants GetQuadrants((int x, int y) centre, (int x, int y) pointToTest)
        {
            int x = pointToTest.x - centre.x;

            // Inverting to get +ve y => up
            // Apparently I'm incapable of getting this sort of code right when the y axis
            // is inverted, as it is throughout most of Day 6.
            int y = centre.y - pointToTest.y;

            bool topRightHalf = x >= -y;
            bool bottomLeftHalf = x <= -y;
            bool topLeftHalf = x <= y;
            bool bottomRightHalf = x >= y;
            return
                (topRightHalf && topLeftHalf ? Quadrants.Top : default) |
                (topRightHalf && bottomRightHalf ? Quadrants.Right : default) |
                (bottomLeftHalf && bottomRightHalf ? Quadrants.Bottom : default) |
                (bottomLeftHalf && topLeftHalf ? Quadrants.Left : default);
        }
    }
}
