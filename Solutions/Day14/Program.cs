using System;
using System.Collections.Generic;
using System.Linq;

namespace Day14
{
    public static class Program
    {
        private static void Main()
        {
            string first30 = new string(GenerateScoreboard(30)
                .Select(i => (char) ('0' + i))
                .ToArray());
            Console.WriteLine(first30);

            Console.WriteLine("Part 1: " + SolvePart1(165061));
            Console.WriteLine("Part 2: " + SolvePart2(21000000, "165061"));
        }

        public static string SolvePart1(int count) => new string(
            GenerateScoreboard(count + 10)
            .Skip(count)
            .Take(10)
            .Select(i => (char) ('0' + i))
            .ToArray());

        public static int SolvePart2(int n, string pattern)
        {
            string firstN = new string(GenerateScoreboard(n + pattern.Length)
                .Select(i => (char) ('0' + i))
                .ToArray());

            return firstN.IndexOf(pattern);
        }

        public static IEnumerable<byte> GenerateScoreboard(int length)
        {
            // Sadly, a naive immutable implementation is unusably slow, so we're using a
            // mutable array to avoid allocations.
            var scoreboard = new byte[length];
            scoreboard[0] = 3;
            scoreboard[1] = 7;
            int next = 2;
            var positions = new[] { 0, 1 };
            while (next < length)
            {
                (var newRecipes, var newPositions) = GetNewRecipes(scoreboard, next, positions);
                int i = 0;
                while (next < length && i < newRecipes.Count)
                {
                    scoreboard[next++] = newRecipes[i++];
                }

                positions = newPositions;
            }
            return scoreboard;
        }

        public static (IList<byte> newRecipeScores, int[] elfPositions) GetNewRecipes(
            IReadOnlyList<byte> scoreboard,
            int scoreboardSize,
            int[] elfPositions)
        {
            long total = elfPositions.Sum(p => scoreboard[p]);
            long tt = total;
            int digit;
            int digits = 1 + (int) Math.Log10(Math.Max(1, total));
            byte[] newRecipeScores = DigitArrays[digits - 1];
            for (digit = 0; digit == 0 || tt != 0; ++digit)
            {
                newRecipeScores[newRecipeScores.Length - 1 - digit] = (byte) (tt % 10);
                tt /= 10;
            }

            // It's a bit messy that we edit the positions in place as well as returning
            // those changed positions. That's because when I wrote this, I started out
            // with immutable lists, but the allocation costs were intolerable, so I made
            // modifications to remove as many allocations as possible. So we now just
            // return the same array of positions that was passed in, modifying the
            // positions in situ. By avoiding per-iteration allocations, we speed things
            // up by orders of magnitude.
            int newScoreboardSize = scoreboardSize + digit;
            for (int i = 0; i < elfPositions.Length; ++i)
            {
                int p = elfPositions[i];
                elfPositions[i] = (p + scoreboard[p] + 1) % newScoreboardSize;
            }

            return (newRecipeScores, elfPositions);
        }

        // Reusable arrays to enable GetNewRecipes to return results of variable length without
        // having to allocate a new array every time.
        private static readonly byte[][] DigitArrays = Enumerable.Range(1, 10).Select(i => new byte[i]).ToArray();
    }
}
