using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Day09
{
    public static class Program
    {
        // My input:
        // 413 players; last marble is worth 71082 points
        public const int Players = 413;
        public const int LastMarbleValue = 71082;

        private static void Main()
        {
            long part1 = Part1(LastMarbleValue, Players);
            Console.WriteLine("Part 1: " + part1);

            long part2 = Part1(LastMarbleValue * 100, Players);
            Console.WriteLine("Part 2: " + part2);
        }

        public static IEnumerable<(GameState state, int? turnScore)> GenerateSteps() =>
            EnumerableEx.Generate(
                (state: GameState.Initial, turnScore: default(int?)),
                _ => true,
                step => step.state.Next(),
                step => step);

        public static IImmutableList<long> Play(int steps, int players) =>
            GenerateSteps()
            .Take(steps)
            .Aggregate(
                ImmutableList.CreateRange(Enumerable.Repeat(0L, players)),
                (acc, turn) => turn.turnScore.HasValue
                    ? acc.SetItem(turn.state.LastTurn % players, acc[turn.state.LastTurn % players] + turn.turnScore.Value)
                    : acc);

        public static long Part1(int steps, int players) =>
            Play(steps + 1, players)
            .Max();
    }
}
