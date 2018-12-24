using System;
using System.Collections.Generic;
using System.Linq;

using Common;

using LanguageExt;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Day10
{
    /// <summary>
    /// Day 10 - finding the writing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is not very efficient. It produces the answer in a manageable amount of time, so
    /// I've not optimized it, but it could be massively faster. The steps aren't that complex,
    /// so running the 10k or so iterations required for part 2 shouldn't take any significant
    /// amount of time. I suspect the slowness is at least in part due to my code hanging onto
    /// everything, when it really only needs to hang onto a window over the last couple of
    /// items. But also, since this is just a minimization problem, and we can easily calculate
    /// the Nth iteration by just multiplying the velocities by N, we don't need to calculate
    /// every single iteration. The use of Min and Max introduce non-linear behaviour that might
    /// make it hard to find a closed-form solution that goes straight to the answer, but maybe
    /// by finding the points moving fastest in the X and Y directions we could then find the
    /// time at which they are closest, and use that to narrow down the window of times we
    /// need to search. However, I've already got my answer so I'm not going to spend the time
    /// doing that...
    /// </para>
    /// </remarks>
    public static class Program
    {
        private static readonly Parser<Point> pointParser =
            from p in between(str("position=<"), ch('>'), Trim(pInt32CommaInt32))
            from _ in spaces
            from v in between(str("velocity=<"), ch('>'), Trim(pInt32CommaInt32))
            select new Point(p.x, p.y, v.x, v.y);

        private static readonly Func<string, Point> parseLine = LineProcessor(pointParser);

        public static Point[] ExamplePoints { get; } = new[]
        {
            parseLine("position=< 9,  1> velocity=< 0,  2>"),
            parseLine("position=< 7,  0> velocity=<-1,  0>"),
            parseLine("position=< 3, -2> velocity=<-1,  1>"),
            parseLine("position=< 6, 10> velocity=<-2, -1>"),
            parseLine("position=< 2, -4> velocity=< 2,  2>"),
            parseLine("position=<-6, 10> velocity=< 2, -2>"),
            parseLine("position=< 1,  8> velocity=< 1, -1>"),
            parseLine("position=< 1,  7> velocity=< 1,  0>"),
            parseLine("position=<-3, 11> velocity=< 1, -2>"),
            parseLine("position=< 7,  6> velocity=<-1, -1>"),
            parseLine("position=<-2,  3> velocity=< 1,  0>"),
            parseLine("position=<-4,  3> velocity=< 2,  0>"),
            parseLine("position=<10, -3> velocity=<-1,  1>"),
            parseLine("position=< 5, 11> velocity=< 1, -2>"),
            parseLine("position=< 4,  7> velocity=< 0, -1>"),
            parseLine("position=< 8, -2> velocity=< 0,  1>"),
            parseLine("position=<15,  0> velocity=<-2,  0>"),
            parseLine("position=< 1,  6> velocity=< 1,  0>"),
            parseLine("position=< 8,  9> velocity=< 0, -1>"),
            parseLine("position=< 3,  3> velocity=<-1,  1>"),
            parseLine("position=< 0,  5> velocity=< 0, -1>"),
            parseLine("position=<-2,  2> velocity=< 2,  0>"),
            parseLine("position=< 5, -2> velocity=< 1,  2>"),
            parseLine("position=< 1,  4> velocity=< 2,  1>"),
            parseLine("position=<-2,  7> velocity=< 2, -2>"),
            parseLine("position=< 3,  6> velocity=<-1, -1>"),
            parseLine("position=< 5,  0> velocity=< 1,  0>"),
            parseLine("position=<-6,  0> velocity=< 2,  0>"),
            parseLine("position=< 5,  9> velocity=< 1, -2>"),
            parseLine("position=<14,  7> velocity=<-2,  0>"),
            parseLine("position=<-3,  6> velocity=< 2, -1>")
        };

        private static void Main()
        {
            int divergesOn = Program.RunUntilDivergent(Program.ExamplePoints).Take(100).Count();

            var exampleStates = RunUntilDivergent(ExamplePoints);
            ShowLastStates(exampleStates);

            Console.WriteLine();
            Console.WriteLine();

            var inputStart = InputReader.ParseLines(typeof(Program), pointParser).ToList();
            ShowLastStates(RunUntilDivergent(inputStart));
        }

        private static void ShowLastStates(IEnumerable<IEnumerable<Point>> exampleStates)
        {
            var items = exampleStates.ToList();
            int toShow = Math.Min(2, items.Count);
            foreach (var state in items.Skip(items.Count - toShow))
            {
                var map = new Map(state);
                map.Print();
                Console.WriteLine();
            }

            // Subtract two because Run produces a spurious extra copy of the initial state.
            Console.Write(items.Count - 2);
        }

        public static IEnumerable<Point> IteratePoints(IEnumerable<Point> points) =>
            points.Select(p => p.Next());

        public static IEnumerable<IEnumerable<Point>> Run(IEnumerable<Point> start) => EnumerableEx.Generate(
            start,
            _ => true,
            IteratePoints,
            ps => ps);

        public static IEnumerable<IEnumerable<Point>> RunUntilDivergent(IEnumerable<Point> start) =>
            Run(start)
            .Scan(
                (width: int.MaxValue, height: int.MaxValue, largerThanPrevious: false, points: start),
                (acc, ps) =>
                {
                    var map = new Map(ps);
                    Console.WriteLine($"{map.Width}, {map.Height}");
                    return (map.Width, map.Height, map.Width > acc.width || map.Height > acc.height, ps);
                })
            .TakeWhile(ps => !ps.largerThanPrevious)
            .Select(ps => ps.points);
    }
}
