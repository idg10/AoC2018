using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TextHandling;

namespace Day01
{
    /// <summary>
    /// Solution to Day 1 of the 2018 Advent of Code.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            // The challenge instructions first give us some examples of how frequency adjustments
            // are to be processed, so verify that we get the same results.
            Debug.Assert(UpdateFrequency(0, 1) == 1);
            Debug.Assert(UpdateFrequency(1, -2) == -1);
            Debug.Assert(UpdateFrequency(-1, +3) == 2);
            Debug.Assert(UpdateFrequency(2, +1) == 3);

            // The instructions then give us some sequences, so we should also check that we get
            // the right results for each of them.
            Debug.Assert(ApplyFrequencyAdjustments(new[] { +1, +1, +1 }) == 3);
            Debug.Assert(ApplyFrequencyAdjustments(new[] { +1, +1, -2 }) == 0);
            Debug.Assert(ApplyFrequencyAdjustments(new[] { -1, -2, -3 }) == -6);

            IEnumerable<int> input = InputReader
                .EnumerateLines(typeof(Program))
                .Select(ParseInput);

            Console.WriteLine("Day 1: " + ApplyFrequencyAdjustments(input));
        }

        /// <summary>
        /// Adjust the frequency by the amount specified.
        /// </summary>
        /// <param name="current">
        /// The current frequency.
        /// </param>
        /// <param name="change">
        /// The adjustment required.
        /// </param>
        /// <returns>
        /// The adjusted frequency.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This is the basic calculation at the heart of the Day 1 challenge. It may seem
        /// ridiculous to put something so basic - addition - into a method when C# has a built-in
        /// operator (+) to do this. However, I want to be able to pass this basic operation as an
        /// argument to higher order operators to do the overall work of the challenge. Some
        /// languages represent the + operator as a 2-argument function like any other, and in
        /// those langauges we wouldn't need to do this. But in C#, the arithmetic operators are
        /// a special kind of thing, so if we want to pass them as functions, we need to define
        /// functions that use them, like this one.
        /// </para>
        /// </remarks>
        static int UpdateFrequency(int current, int change) => current + change;

        /// <summary>
        /// Accumulate all of the frequency adjustments into a single result.
        /// </summary>
        /// <param name="changes">
        /// The list of frequency changes - the input to the puzzle.
        /// </param>
        /// <returns>
        /// The final frequency.
        /// </returns>
        /// <remarks>
        /// <para>
        /// I'm using <c>Aggregate</c> to apply <see cref="UpdateFrequency(int, int)"/> over the
        /// entire list. There is a simpler way I could have done this: I could have just run
        /// <c>Sum</c> directly over the inputs, because the operation applied at each stage is
        /// just addition. However, I like the way the code as it stands has struture that
        /// directly reflects the way the challenge is expressed. (Also, these challenges have a
        /// way of getting more complex in the 2nd step, or as the days go on, so keeping the
        /// flexibility to update the function is the sort of thing that often proves useful.)
        /// </para>
        /// </remarks>
        static int ApplyFrequencyAdjustments(IEnumerable<int> changes) =>
            changes.Aggregate(UpdateFrequency);

        /// <summary>
        /// Parse one line of input into a form that our code can process.
        /// </summary>
        /// <param name="line">
        /// The line of input. In this puzzle, it's an integer expressed as a sign (+ or -)
        /// and then the value as a decimal.
        /// </param>
        /// <returns>
        /// The value of the integer.
        /// </returns>
        static int ParseInput(string line) => int.Parse(line);
    }
}
