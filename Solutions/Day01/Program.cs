using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

            // Day 2
            Debug.Assert(FindFirstRepeatedFrequency(new[] { +1, -1 }) == 0);
            Debug.Assert(FindFirstRepeatedFrequency(new[] { +3, +3, +4, -2, -4 }) == 10);
            Debug.Assert(FindFirstRepeatedFrequency(new[] { -6, +3, +8, +5, -6 }) == 5);
            Debug.Assert(FindFirstRepeatedFrequency(new[] { +7, +7, -2, -7, -4 }) == 14);

            Console.WriteLine("Day 2: " + FindFirstRepeatedFrequency(input));
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
        /// directly reflects the way the challenge is expressed.
        /// </para>
        /// <para>
        /// Also, these challenges have a way of getting more complex in the 2nd step, or as the
        /// days go on, so keeping the flexibility to update the function is the sort of thing
        /// that often proves useful. For example, in Part 2 of this challenge, we ended up
        /// invoking <see cref="UpdateFrequency(int, int)"/> through a different path in
        /// <see cref="GetAdjustedFrequenciesWithRepeatingInput"/>. If we had used <c>Sum</c>
        /// here, that would have lost sight of the fact that both days are, underneath it all,
        /// executing the logic in <see cref="UpdateFrequency(int, int)"/>.
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


        /// <summary>
        /// Finds the first calculated frequency value that is a repeat of an earlier frequency.
        /// </summary>
        /// <param name="values">Input values.</param>
        /// <returns>The result.</returns>
        /// <remarks>
        /// <para>
        /// This is part 2 of the challenge. Whereas day 1 just needed us to calculate the final
        /// result, this one requires us to look at the result after each step, and watch for the
        /// first result that shows up twice.
        /// </para>
        /// </remarks>
        static int FindFirstRepeatedFrequency(IEnumerable<int> values) =>
            FindFirstRepeatedValue(GetAdjustedFrequenciesWithRepeatingInput(values));

        /// <summary>
        /// Produces a sequence in which each value represents a frequency, starting with the
        /// initial frequency (0), and then the frequence after each adjustment specified in the
        /// input. This produces an endless sequence by looping the input.
        /// </summary>
        /// <param name="changes">
        /// The frequency adjustments from the input.
        /// </param>
        /// <returns>
        /// Every frequency setting the device goes through, in turn.
        /// </returns>
        static IEnumerable<int> GetAdjustedFrequenciesWithRepeatingInput(IEnumerable<int> changes) =>
            EnumerableEx.Return(0)
            .Concat(changes.Repeat().Scan(0, UpdateFrequency));

        /// <summary>
        /// Returns the first element in a list that is a repeated element (i.e., the first element
        /// that we see for a second time).
        /// </summary>
        /// <param name="values"></param>
        /// <returns>The first element that we see a second time.</returns>
        /// <remarks>
        /// <para>
        /// The obvious way to do this would be to write a <c>foreach</c> loop over the input, and
        /// to keep track of what we've seen so far, returning the first item that's not new. But
        /// why do the obvious thing?
        /// </para>
        /// <para>
        /// Instead, to make life 'fun' I've decided to write this in a pure functional way. We're
        /// using the <c>Scan</c> operator to run the <see cref="IsNew((IImmutableSet{int} seen, bool lastWasNew, int value), int)"/>
        /// method over each item in turn, accumulating results as we go, and producing a sequence
        /// with each accumulated result in turn. We use this accumulator to store the items seen
        /// so far. Where it gets a little messy is that we also need to be able to see whether the
        /// accumulation step found that the item was one it has seen before. And when we do detect
        /// that, we also need to know what the result is. So our accumulator has to include three
        /// things: the accumulated information (what have we already seen), a flag indicating
        /// whether the most recent value was new, and the most recent value. This is rather
        /// messier than the simpler <c>foreach</c> loop would have been, but sometimes pure
        /// functional code is like that.
        /// </para>
        /// </remarks>
        static T FindFirstRepeatedValue<T>(IEnumerable<T> values) => values
            .Scan(MakeIsNewStart<T>(), IsNew)
            .First(item => !item.lastWasNew).value;

        /// <summary>
        /// The accumulation step for <see cref="FindFirstRepeatedValue{T}(IEnumerable{T})"/>.
        /// </summary>
        /// <typeparam name="T">The sequence element type.</typeparam>
        /// <param name="acc">The previous accumulated value.</param>
        /// <param name="value">The new element.</param>
        /// <returns>The new accumulated value.</returns>
        static (IImmutableSet<T> seen, bool lastWasNew, T value) IsNew<T>((IImmutableSet<T> seen, bool lastWasNew, T value) acc, T value)
        {
            IImmutableSet<T> newSeen = acc.seen.Add(value);
            return (newSeen, acc.seen != newSeen, value);
        }

        /// <summary>
        /// Makes a suitable initial accumulator value for <see cref="IsNew{T}((IImmutableSet{T} seen, bool lastWasNew, T value), T)"/>.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>An empty accumulator.</returns>
        static (IImmutableSet<T> seen, bool lastWasNew, T value) MakeIsNewStart<T>() => (ImmutableHashSet<T>.Empty, false, default(T));
    }
}
