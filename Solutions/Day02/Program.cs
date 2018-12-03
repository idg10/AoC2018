using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Common;

namespace Day02
{
    internal static class Program
    {
        private static void Main()
        {
            TestPart1Example("abcdef", new int[0]);
            TestPart1Example("bababc", new[] { 2, 3 }, ('a', 2), ('b', 3));
            TestPart1Example("abbcde", new[] { 2 }, ('b', 2));
            TestPart1Example("abcccd", new[] { 3 }, ('c', 3));
            TestPart1Example("aabcdd", new[] { 2 }, ('a', 2), ('d', 2));
            TestPart1Example("abcdee", new[] { 2 }, ('e', 2));
            TestPart1Example("ababab", new[] { 3 }, ('a', 3), ('b', 3));

            int exampleChecksum = CalculateChecksum(new[]
            {
                "abcdef",
                "bababc",
                "abbcde",
                "abcccd",
                "aabcdd",
                "abcdee",
                "ababab"
            });

            Debug.Assert(exampleChecksum == 12);

            int partOneResult = CalculateChecksum(InputReader.EnumerateLines(typeof(Program)));
            Console.WriteLine("Part 1: " + partOneResult);

            string[] part2TestInput =
            {
                "abcde",
                "fghij",
                "klmno",
                "pqrst",
                "fguij",
                "axcye",
                "wvxyz"
            };

            Debug.Assert(SolvePart2(part2TestInput) == "fgij");

            string part2Result = SolvePart2(InputReader.EnumerateLines(typeof(Program)));
            Console.WriteLine("Part 2: " + part2Result);
        }

        /// <summary>
        /// Find the first id which has only one character different from an id already seen.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>
        /// The first id which has only one character different from an id already seen.
        /// </returns>
        /// <remarks>
        /// This works by producing 'partial ids', by which we mean the id with one character
        /// removed. For each id, we produce every possible partial id (by first producing a string
        /// which is the id with the first letter missing, then one with the second letter missing,
        /// and so on). We add each of these to a set, and if we produce a partial id that is
        /// already in the set, we know that the id from which it came differs by only one letter
        /// from one we've seen before.
        /// </remarks>
        private static string SolvePart2(IEnumerable<string> ids) => ids
            .Scan(MakeFirstPartialIds(), AccumulatePartialIds)
            .First(x => x.isRepeat).id;

        /// <summary>
        /// Accumulator function which, for each id, adds all its partial ids to a set,
        /// and indicates whether any partial id has been seen before, and produces that partial id,
        /// or, if none of the id's partial ids have been seen before, it produces the id. A
        /// id is the id with one character removed.
        /// </summary>
        /// <param name="acc">The accumulator.</param>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The accumulated dictionary.
        /// </returns>
        private static (IImmutableSet<string> partialIdsSeen, string id, bool isRepeat) AccumulatePartialIds(
            (IImmutableSet<string> partialIdsSeen, string id, bool isRepeat) acc,
            string id)
        {
            var partialIdsSeen = acc.partialIdsSeen;
            IEnumerable<string> partialIds = Enumerable.Range(0, id.Length - 1)
                .Select(i => id.Substring(0, i) + id.Substring(i + 1))
                .Distinct();

            bool isRepeat = false;
            string idToReturn = id;
            foreach (string partialId in partialIds)
            {
                var updatedPartialIdsSeen = partialIdsSeen.Add(partialId);
                if (partialIdsSeen == updatedPartialIdsSeen)
                {
                    isRepeat = true;
                    idToReturn = partialId;
                }
                partialIdsSeen = updatedPartialIdsSeen;
            }

            return (partialIdsSeen, idToReturn, isRepeat);
        }

        private static (IImmutableSet<string> partialIdsSeen, string id, bool isRepeat) MakeFirstPartialIds() =>
            (ImmutableHashSet<string>.Empty, null, false);

        /// <summary>
        /// Calculate the part 1 'checksum', in which for each input id, we count the number of
        /// ids that repeat a character particular number of times, and multiply together each
        /// count. (So if 3 ids repeat 2 characters, and 5 ids repeeat 3 charactes, the result
        /// will be 3*5 = 15.)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        private static int CalculateChecksum(IEnumerable<string> ids)
        {
            var allContributions = ids.SelectMany(GetRepetitionCounts);
            var contributionsByNumber = allContributions.GroupBy(x => x);

            return allContributions
                .GroupBy(x => x)
                .Aggregate(1, (t, g) => t * g.Count());
        }

        /// <summary>
        /// For a single id, calculate for each repeating character, how many repeats there
        /// are, and return each distinct number of repeats. (E.g. given "aaabbbcc", both a and
        /// b are repeated 3 times, and c is repeated twice, so this will return 3 and 2.)
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="id">The id. (A sequence of elements - a string in this challenge.)</param>
        /// <returns>
        /// A sequence of numbers. If the incoming id repeats any element exactly twice, this
        /// sequence will include the number 2, and if it repeats any element three times, it
        /// will include the number 3. (If the id repeats multiple elements, it will report each
        /// repeat count exactly once. E.g., given "aaabbb", which repeats a 3 times, and also b
        /// 3 times, it will return a single 3.)
        /// </returns>
        /// <remarks>
        /// The problem specification does not make clear whether we are to consider any number
        /// of repeats, or we should only consider elements repeated 2 or 3 times. It appears to
        /// make no difference to the results. As it stands, we only consider 2 or 3 repeats, but
        /// there is a commented-out implementation that works for any number of repetitions.
        /// Either code produces the correct result for all examples in the problem statement,
        /// and either seems to produce the correct result for the challange input.
        /// </remarks>
        private static IEnumerable<int> GetRepetitionCounts<T>(IEnumerable<T> id)
        {
            var d = id.FindRepeatedValueCounts();

            // The problem description is specifically about 2 or 3 repeats, in which
            // case this is the logic we require.
            if (d.Any(x => x.Value == 2))
            {
                yield return 2;
            }
            if (d.Any(x => x.Value == 3))
            {
                yield return 3;
            }

            // However, if it's nore general, and we also want to include IDs containing exactly N
            // letters for any N>1, then this is the right answer:
            //return d.Select(x => x.Value).Where(x => x > 1).Distinct();

            // Either passes the example tests for day 1, and either produces the correct
            // result for my day 1 input.
        }

        /// <summary>
        /// Check that when we try to find the number of repeated values, and how those repetitions
        /// contribute to the checksum, that we get the expected results for each example given in
        /// the problem statement.
        /// </summary>
        /// <typeparam name="T">
        /// The element type in the id (<c>char</c> in the problems given).
        /// </typeparam>
        /// <param name="source">The id.</param>
        /// <param name="expectedContributions">
        /// The contributions this id should produce towards the checksum, according to the problem
        /// definition.
        /// </param>
        /// <param name="expectedCounts">
        /// The repetition counts expected for any characters that repeat. (All other characters in
        /// the source string are expected to have a repetition count of 1.)
        /// </param>
        private static void TestPart1Example<T>(
            IEnumerable<T> source,
            IEnumerable<int> expectedContributions,
            params (T value, int expectedCount)[] expectedCounts)
        {
            IImmutableDictionary<T, int> counts = source.FindRepeatedValueCounts();
            CheckDictionary(counts, source.Distinct(), expectedCounts);
            List<int> contributionsSorted = GetRepetitionCounts(source).OrderBy(x => x).ToList();
            List<int> expecteContributionsSorted = expectedContributions.ToList();

            Debug.Assert(contributionsSorted.SequenceEqual(expecteContributionsSorted));
        }

        /// <summary>
        /// Check that a dictionary contains entries for each of the expected keys, and only those
        /// keys, and that each key has the specified repetition count (or a count of 1 if no count
        /// is specified for that key).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <param name="expectedKeys"></param>
        /// <param name="expectedCounts"></param>
        private static void CheckDictionary<T>(
            IImmutableDictionary<T, int> d,
            IEnumerable<T> expectedKeys,
            params (T value, int expectedCount)[] expectedCounts)
        {
            Debug.Assert(expectedKeys.Count() == d.Count);
            Dictionary<T, int> countMap = expectedCounts.ToDictionary(x => x.value, x => x.expectedCount);
            foreach (T key in expectedKeys)
            {
                if (!countMap.TryGetValue(key, out int expectedCount))
                {
                    expectedCount = 1;
                }

                Debug.Assert(d[key] == expectedCount);
            }
        }
    }
}
