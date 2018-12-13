using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Day07
{
    public static class Program
    {
        public static readonly Parser<StepConstraint> LineParser =
            from prereq in between(str("Step "), str(" must be finished before step "), anyChar)
            from then in anyChar
            from _ in str(" can begin.")
            select new StepConstraint(prereq, then);

        private readonly static string[] ExampleLines =
        {
            "Step C must be finished before step A can begin.",
            "Step C must be finished before step F can begin.",
            "Step A must be finished before step B can begin.",
            "Step A must be finished before step D can begin.",
            "Step B must be finished before step E can begin.",
            "Step D must be finished before step E can begin.",
            "Step F must be finished before step E can begin."
        };

        public static readonly StepConstraint[] ExampleInput = ExampleLines
            .Select(line => ProcessLine(LineParser, line))
            .ToArray();

        private static void Main()
        {
            string part1Example = Part1(ExampleInput);
            Console.WriteLine("Part 1 example: " + part1Example);
            string part1 = Part1(InputReader.ParseLines(typeof(Program), LineParser));
            Console.WriteLine("Part 1: " + part1);
        }

        public static string Part1(IEnumerable<StepConstraint> input) => new string(
            Run(input).Select(s => s.step).ToArray());

        public static IEnumerable<(char step, State state)> Run(IEnumerable<StepConstraint> rules) =>
            EnumerableEx.Generate(
                State.Start(rules),
                s => s.HasNextAvailable,
                s => s.ExecuteStep(s.NextAvailable),
                s => (s.NextAvailable, s));
    }
}
