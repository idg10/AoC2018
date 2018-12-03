using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Common
{
    public static class EnumerableAocExt
    {
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
        public static T FindFirstRepeatedValue<T>(this IEnumerable<T> values) => values
            .Scan(MakeIsNewStart<T>(), IsNew)
            .First(item => !item.lastWasNew).value;

        /// <summary>
        /// Produces dictionary with reporting the number of times each distinct value appears in
        /// the source collection.
        /// </summary>
        /// <typeparam name="T">The source element type.</typeparam>
        /// <param name="values">The source values.</param>
        /// <returns>
        /// A dictionary with an entry for each distinct source value. These source values are use
        /// as the dictionary entry keys, which then map to an integer reporting how many times
        /// that value appeared in the source.
        /// </returns>
        public static IImmutableDictionary<T, int> FindRepeatedValueCounts<T>(this IEnumerable<T> values) => values
            .Aggregate(MakeRepeatCountsStart<T>(), UpdateSeenCount).seenCounts;

        /// <summary>
        /// The accumulation step for <see cref="FindFirstRepeatedValue{T}(IEnumerable{T})"/>.
        /// </summary>
        /// <typeparam name="T">The sequence element type.</typeparam>
        /// <param name="acc">The previous accumulated value.</param>
        /// <param name="value">The new element.</param>
        /// <returns>The new accumulated value.</returns>
        private static (IImmutableSet<T> seen, bool lastWasNew, T value) IsNew<T>(
            (IImmutableSet<T> seen, bool lastWasNew, T value) acc,
            T value)
        {
            IImmutableSet<T> newSeen = acc.seen.Add(value);
            return (newSeen, acc.seen != newSeen, value);
        }

        /// <summary>
        /// Makes a suitable initial accumulator value for <see cref="IsNew{T}((IImmutableSet{T} seen, bool lastWasNew, T value), T)"/>.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>An empty accumulator.</returns>
        private static (IImmutableSet<T> seen, bool lastWasNew, T value) MakeIsNewStart<T>() => (ImmutableHashSet<T>.Empty, false, default(T));

        /// <summary>
        /// The accumulation step for <see cref="FindFirstRepeatedValue{T}(IEnumerable{T})"/>.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="acc">
        /// The accumulator. This contains a dictionary reporting the number of each distinct
        /// source value seen so far, and it also reports the current value, in case Scan-based
        /// code wants to inspect it.
        /// </param>
        /// <param name="value">The source value to add to the accumulation.</param>
        /// <returns></returns>
        private static (IImmutableDictionary<T, int> seenCounts, T value) UpdateSeenCount<T>(
            (IImmutableDictionary<T, int> seenCounts, T value) acc,
            T value)
        {
            IImmutableDictionary<T, int> updatedCounts = acc.seenCounts.TryGetValue(value, out int currentCount)
                ? acc.seenCounts.SetItem(value, currentCount + 1)
                : acc.seenCounts.Add(value, 1);

            return (updatedCounts, value);
        }

        /// <summary>
        /// Makes a suitable initial accumulator value for <see cref="UpdateSeenCount{T}((IImmutableDictionary{T, int} seenCounts, T value), T)"/>.
        /// </summary>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>An empty accumulator.</returns>
        private static (IImmutableDictionary<T, int> seenCounts, T value) MakeRepeatCountsStart<T>() =>
            (ImmutableDictionary<T, int>.Empty, default(T));
    }
}
