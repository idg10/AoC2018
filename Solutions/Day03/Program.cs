using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

using Common;

using LanguageExt;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Day03
{
    internal static class Program
    {
        static void Main()
        {
            // Parse lines of this form:
            //  #1 @ 1,3: 4x4
            Parser<(int id, int x, int y, int w, int h)> lineParser =
                from id in between(ch('#'), spaces, pInt32)
                from _ in ch('@')
                from p in between(spaces, ch(':'), pInt32CommaInt32)
                from s3 in spaces
                from s in pInt32By32Int
                select (id: id, x: p.x, y: p.y, w: s.w, h: s.h);


            Func<string, (int id, int x, int y, int w, int h)> parseLine = LineProcessor(lineParser);

            var r = parseLine("#1 @ 1,3: 4x4");
            var r2 = parseLine("#2 @ 3,1: 4x4");
            var r3 = parseLine("#3 @ 5,5: 2x2");

            var state = AddRectangle(GetEmptyState<int>(), r);
            PrintState(state);
            Console.WriteLine();
            state = AddRectangle(state, r2);
            PrintState(state);
            Console.WriteLine();
            state = AddRectangle(state, r3);
            PrintState(state);

            Debug.Assert(CountOverlappingCells(state) == 4);

            int part1Result = CountOverlappingCells(
                InputReader.ParseLines(typeof(Program), lineParser)
                .Aggregate(GetEmptyState<int>(), AddRectangle));
            Console.WriteLine("Part 1: " + part1Result);
        }

        /// <summary>
        /// Gets initial state, in which no fabric has been claimed by anyone.
        /// </summary>
        /// <typeparam name="T">The claim type.</typeparam>
        /// <returns>An empty dictionaray.</returns>
        static IImmutableDictionary<(int x, int y), IImmutableSet<T>> GetEmptyState<T>() => ImmutableDictionary<(int x, int y), IImmutableSet<T>>.Empty;

        /// <summary>
        /// Adds a rectangle of the specified dimensions with the given claim.
        /// </summary>
        /// <typeparam name="T">Claim type.</typeparam>
        /// <param name="state">The current claims.</param>
        /// <param name="rectangle">Rectangle claim and dimensions.</param>
        /// <returns>The modified state.</returns>
        static IImmutableDictionary<(int x, int y), IImmutableSet<T>> AddRectangle<T>(
            IImmutableDictionary<(int x, int y), IImmutableSet<T>> state,
            (T id, int x, int y, int w, int h) rectangle)
        {
            return AddRectangle(state, rectangle.id, rectangle.x, rectangle.y, rectangle.w, rectangle.h);
        }

        /// <summary>
        /// Adds a rectangle of the specified dimensions with the given claim.
        /// </summary>
        /// <typeparam name="T">Claim type.</typeparam>
        /// <param name="state">The current claims.</param>
        /// <param name="claim">Claim to assocaite with this rectangle.</param>
        /// <param name="x">Rectangle left edge.</param>
        /// <param name="y">Rectangle top edge.</param>
        /// <param name="w">Rectangle width</param>
        /// <param name="h">Rectangle height.</param>
        /// <returns>The modified state.</returns>
        static IImmutableDictionary<(int x, int y), IImmutableSet<T>> AddRectangle<T>(
            IImmutableDictionary<(int x, int y), IImmutableSet<T>> state,
            T claim,
            int x,
            int y,
            int w,
            int h)
        {
            for (int j = 0; j < h; ++j)
            {
                for (int i = 0; i < w; ++i)
                {
                    (int x, int y) key = (x + i, y + j);
                    if (state.TryGetValue(key, out IImmutableSet<T> values))
                    {
                        state = state.SetItem(key, values.Add(claim));
                    }
                    else
                    {
                        state = state.Add(key, ImmutableHashSet.Create(claim));
                    }
                }
            }

            return state;
        }

        /// <summary>
        /// Counts the number of cells in the state that have more than one claim attached.
        /// </summary>
        /// <typeparam name="T">Claim type.</typeparam>
        /// <param name="state">The claims.</param>
        /// <returns>The number of cells that have more than one claim.</returns>
        static int CountOverlappingCells<T>(IImmutableDictionary<(int x, int y), IImmutableSet<T>> state) =>
            state.Count(kv => kv.Value.Count > 1);

        /// <summary>
        /// Find the width and height of area of the state in use.
        /// </summary>
        /// <typeparam name="T">Claim type.</typeparam>
        /// <param name="state">The claims.</param>
        /// <returns>
        /// The width and height of the smallest rectangle which, if its top left corner is at (0,0)
        /// contains all claimed cells.</returns>
        static (int w, int h) GetDimensions<T>(IImmutableDictionary<(int x, int y), IImmutableSet<T>> state)
        {
            return state.Aggregate((w: 0, h: 0), (size, kv) => (Math.Max(size.w, kv.Key.x), Math.Max(size.h, kv.Key.y)));
        }

        /// <summary>
        /// Prints the claims on screen, or an X for any cells with multiple claims.
        /// </summary>
        /// <param name="state">The claims.</param>
        static void PrintState(IImmutableDictionary<(int x, int y), IImmutableSet<int>> state)
        {
            (int w, int h) = GetDimensions(state);
            for (int j = 0; j <= h; ++j)
            {
                for (int i = 0; i <= w; ++i)
                {
                    char c = state.TryGetValue((i, j), out IImmutableSet<int> values)
                        ? (values.Count == 1 ? values.Single().ToString().Single() : 'X')
                        : '.';

                    Console.Write(c);
                }

                Console.WriteLine();
            }
        }
    }
}
