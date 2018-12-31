using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Numerics;
using Common;

using LanguageExt;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Day12
{
    public static class Program
    {
        private const int RuleLength = 5;

        // For each iteration we need to pad out the pots on the left and right
        // in case there are any rules of the form ....# or #.... that will match
        // only by virtual of the infinite extent of empty pots to the left and
        // right.
        private const int PadSize = RuleLength - 1;

        public const string TestInput =
@"initial state: #..#.#..##......###...###

...## => #
..#.. => #
.#... => #
.#.#. => #
.#.## => #
.##.. => #
.#### => #
#.#.# => #
#.### => #
##.#. => #
##.## => #
###.. => #
###.# => #
####. => #";

        private static readonly Parser<char> pPotChar = choice(ch('#'), ch('.'));

        public static readonly Parser<string> pPots = asString(many1(pPotChar));

        private static readonly Parser<string> pInitialState =
            from _ in str("initial state:")
            from __ in spaces
            from pots in pPots
            select pots;

        public static readonly Parser<(string pattern, bool pot)> pRule =
            from pattern in asString(many1(pPotChar))
            from _ in between(spaces, spaces, str("=>"))
            from result in pPotChar
            select (pattern, result == '#');

        public static readonly Parser<(string initialState, string[] rulesProducingPots)> pAll =
            from initialState in pInitialState
            from _ in many(endOfLine)
            from rules in sepBy1(pRule, endOfLine)
            select (initialState, rules.Where(r => r.pot).Select(r => r.pattern).ToArray());

        public static string TestInitialState { get; }
        public static string[] TestRulesProducingPots { get; }

        public static Pots[] TestResults { get; }

        static Program()
        {
            (TestInitialState, TestRulesProducingPots) = ProcessLine(pAll, TestInput);
            TestResults = Run(TestInitialState, TestRulesProducingPots)
                .Take(21)
                .ToArray();
        }

        private static void Main()
        {
            BigInteger part1Example = SolvePart1(TestInitialState, TestRulesProducingPots, 20);
            Console.WriteLine("Part 1 example: " + part1Example);

            (string initialState, string[] rulesProducingPots) = InputReader.ParseAll(typeof(Program), pAll);
            BigInteger part1 = SolvePart1(initialState, rulesProducingPots, 20);
            Console.WriteLine("Part 1: " + part1);

            BigInteger part2 = SolvePart2(initialState, rulesProducingPots);
            Console.WriteLine("Part 2: " + part2);
        }

        public static IEnumerable<char> Step(
            string potState,
            IImmutableSet<string> rulesProducingPots)
        {
            IEnumerable<char> pad = Enumerable.Repeat('.', PadSize);
            IEnumerable<char> input = EnumerableEx.Concat(pad, potState, pad);
            return
                from potSetChars in input.Buffer(RuleLength, 1)
                let potSet = new string(potSetChars.ToArray())
                where potSet.Length == RuleLength
                select rulesProducingPots.Contains(potSet) ? '#' : '.';
        }

        public static IEnumerable<Pots> Run(
            string initialState,
            string[] rulesProducingPots)
        {
            var rules = ImmutableHashSet.CreateRange(rulesProducingPots);
            return EnumerableEx.Generate(
                new Pots(initialState, 0),
                _ => true,
                pots =>
                {
                    var next = Step(pots.PotText, rules);
                    int emptyOnLeft = next.TakeWhile(c => c == '.').Count();
                    string nextText = new string(next.Skip(emptyOnLeft).ToArray()).TrimEnd('.');
                    const int initialOffset = (RuleLength - 1) / 2;
                    return new Pots(nextText, pots.LeftIndex + emptyOnLeft - initialOffset);
                },
                pots => pots);
        }

        public static BigInteger SolvePart1(
            string initialState,
            string[] rulesProducingPots,
            int generations)
        {
            Pots state = Run(initialState, rulesProducingPots).Skip(generations).First();
            return CalculatePotsValue(state);
        }

        private static BigInteger CalculatePotsValue(Pots state)
        {
            return state
                .PotText
                .Select((c, i) => c == '.' ? new BigInteger() : i + state.LeftIndex)
                .Sum();
        }

        public static BigInteger SolvePart2(
            string initialState,
            string[] rulesProducingPots)
        {
            // We're going to make the theoretically totally unjustified (but empirically verifiable
            // for my particular input) assumption that we eventually hit a steady state, in which
            // the pattern of pots becomes fixed, but its position moves right by 1 for every iteration.
            (Pots steady, int index) = Run(initialState, rulesProducingPots)
                .Buffer(2, 1)
                .TakeWhile(b => b[0].PotText != b[1].PotText)
                .Aggregate(
                    (pots: default(Pots), count: 0),
                    (a, p) => (p[1], a.count + 1));

            BigInteger valueAtCurrentIteration = CalculatePotsValue(steady);

            var target = new BigInteger(50_000_000_000);
            BigInteger iterationsRemaining = target - index;
            BigInteger positionOfFinalPots = steady.LeftIndex + iterationsRemaining;

            return CalculatePotsValue(new Pots(steady.PotText, positionOfFinalPots));
        }
    }
}
