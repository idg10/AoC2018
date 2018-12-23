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

            var part2Steps = RunPart2(ExampleInput, 2, 0).Take(40).ToList();
            int i = 1;
            foreach ((State stepState, NationalElfService serviceState, string stepsDone) in part2Steps)
            {
                Console.Write("{0:,###} ", i++);
                for (int w = 0; w < 2; ++w)
                {
                    var ws = serviceState.WorkerState[w];
                    Console.Write(ws?.Step ?? '.');
                    Console.Write(' ');
                }
                Console.WriteLine(stepsDone);
            }

            int part2 = RunPart2(InputReader.ParseLines(typeof(Program), LineParser), 5, 60).Count();
            Console.WriteLine("Part 2: " + part2);
        }

        public static string Part1(IEnumerable<StepConstraint> input) => new string(
            Run(input).Select(s => s.step).ToArray());

        public static IEnumerable<(char step, State state)> Run(IEnumerable<StepConstraint> rules) =>
            EnumerableEx.Generate(
                State.Start(rules),
                s => s.HasNextAvailable,
                s => s.ExecuteStep(s.TopNextAvailable),
                s => (s.TopNextAvailable, s));

        /// <summary>
        /// Execute steps for part two, producing a sequence reporting for each second the current
        /// processed step state (<see cref="State"/>) and the state of all the workers
        /// (<see cref="NationalElfService"/>).
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="workers"></param>
        /// <param name="baseTime"></param>
        /// <returns></returns>
        public static IEnumerable<(State stepState, NationalElfService serviceState, string stepsDone)> RunPart2(
            IEnumerable<StepConstraint> rules,
            int workers,
            int baseTime) =>
            EnumerableEx.Generate(
                (stepState: State.Start(rules), serviceState: NationalElfService.Create(workers, baseTime), stepsDone: ""),
                (s) => s.serviceState.WorkerState.Any(ws => ws != null) || s.stepState.HasNextAvailable,
                s =>
                {
                    (State stepStep, NationalElfService serviceState, IList<char> stepsDone) = s.serviceState.ProcessOneSecond(s.stepState);
                    return (stepStep, serviceState, s.stepsDone + new string(stepsDone.ToArray()));
                },
                s => s)
            .Skip(1);
    }
}
