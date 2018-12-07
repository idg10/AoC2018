using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace Day05
{
    public static class Program
    {

        private static void Main(string[] args)
        {
            string input = InputReader.EnumerateLines(typeof(Program)).Single();
            Console.WriteLine("Part 1: " + Part1(input));
            Console.WriteLine("Part 2: " + Part2(input));
        }

        public static int Part1(string input)
        {
            string result = ProcessAllReactions(input).Last();
            return result.Length;
        }

        public static int Part2(string input)
        {
            List<char> unitTypes = input.Select(char.ToLowerInvariant).Distinct().ToList();
            int min = input.Length;
            foreach (char unitToRemove in unitTypes)
            {
                Console.WriteLine("Removing " + unitToRemove);
                string modifiedInput = input
                    .Replace(unitToRemove.ToString(), "")
                    .Replace(char.ToUpperInvariant(unitToRemove).ToString(), "");

                int reactedLength = Part1(modifiedInput);
                Console.WriteLine("Length: " + reactedLength);
                if (reactedLength < min)
                {
                    min = reactedLength;
                    Console.WriteLine("New minimum!");
                }
            }

            return min;
        }

        private static bool Reacts(char a, char b) => a != b && char.ToLowerInvariant(a) == char.ToLowerInvariant(b);

        public static bool ProcessOneReaction(string input, out string result)
        {
            int indexOfFirstReactableUnit = input.Buffer(2, 1).TakeWhile(pair => pair.Count != 2 || !Reacts(pair[0], pair[1])).Count();
            bool hasReaction = indexOfFirstReactableUnit != input.Length;
            result = hasReaction
                ? input.Substring(0, indexOfFirstReactableUnit) + input.Substring(indexOfFirstReactableUnit + 2)
                : input;
            return hasReaction;
        }

        public static IEnumerable<string> ProcessAllReactions(string input)
        {
            return EnumerableEx.Generate(
                (input: input, done: false),
                s => !s.done,
                s =>
                {
                    bool reacted = ProcessOneReaction(s.input, out string r);
                    return (r, !reacted);
                },
                s => s.input);
        }
    }
}
