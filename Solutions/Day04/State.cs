using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Day04
{
    /// <summary>
    /// Aggregated state.
    /// </summary>
    /// <remarks>
    /// As we process each input line, we create an instance of this type to represent the what we
    /// know based on all notifications up to this point.
    /// </remarks>
    internal sealed class State
    {
        public static readonly State Initial = new State(
            ImmutableDictionary<int, IImmutableDictionary<int, int>>.Empty,
            null,
            null,
            null);

        private State(
            IImmutableDictionary<int, IImmutableDictionary<int, int>> guardSleepSlots,
            int? guardId,
            bool? currentlyAwake,
            DateTime? lastChangeTime)
        {
            this.GuardSleepSlots = guardSleepSlots;
            this.GuardId = guardId;
            this.CurrentlyAwake = currentlyAwake;
            this.LastChangeTime = lastChangeTime;
        }

        /// <summary>
        /// Gets the times we know that guards have been asleep for.
        /// </summary>
        /// <remarks>
        /// The key for this dictionary is the guard id. The value is a dictionary where the key
        /// is the time slot (minutes past midnight), and the number of days on which we've seen
        /// the guard be asleep at that time slot.
        /// </remarks>
        public IImmutableDictionary<int, IImmutableDictionary<int, int>> GuardSleepSlots { get; }

        /// <summary>
        /// Gets the guard currently on duty. Null in our initial state.
        /// </summary>
        public int? GuardId { get; }

        /// <summary>
        /// Gets a value indicating whether the guard is currently awake. Null in our initial state.
        /// </summary>
        public bool? CurrentlyAwake { get; }

        /// <summary>
        /// Gets the time of the notification for which this state represents the current state.
        /// </summary>
        public DateTime? LastChangeTime { get; }

        /// <summary>
        /// Create a new <see cref="State"/>, updating this instances state based on a new guard
        /// shift starting.
        /// </summary>
        /// <param name="id">The id of the guard starting their shift.</param>
        /// <param name="dateTime">The time of the notification.</param>
        /// <returns>The updated state.</returns>
        public State ChangeGuard(int id, DateTime dateTime)
        {
            if (!GuardId.HasValue)
            {
                // This is the first ever notification, so we don't have to do anything special.
                return new State(GuardSleepSlots, id, true, dateTime);
            }

            if (dateTime.TimeOfDay.Hours > 22)
            {
                // Guard has clocked on slightly early. Adjust to the timespan we care about.
                dateTime = dateTime.Date.AddDays(1);
            }

            if (dateTime.Date <= LastChangeTime.Value.Date)
            {
                throw new ArgumentException(
                    "New guard has come on shift on or before day previous guard was on shift",
                    nameof(dateTime));
            }

            IImmutableDictionary<int, IImmutableDictionary<int, int>> slots = GuardSleepSlots;

            if (CurrentlyAwake != true)
            {
                // Last we knew, the previous guard was asleep, so presumably they remained asleep
                // until the end of their shift. So we need to fill in minutes to end of hour.
                int lastKnown = GetMinutes(LastChangeTime.Value);
                slots = AddSleepyTime(slots, GuardId.Value, lastKnown, 59);
            }

            return new State(slots, id, true, dateTime);
        }

        /// <summary>
        /// Create a new <see cref="State"/>, updating this instances state based on a guard
        /// falling asleep.
        /// </summary>
        /// <param name="dateTime">The time of the notification.</param>
        /// <returns>The updated state.</returns>
        public State GuardSleeps(DateTime dateTime)
        {
            if (!GuardId.HasValue)
            {
                throw new InvalidOperationException("Guard asleep notification occured before any guard came on duty");
            }

            if (CurrentlyAwake != true)
            {
                throw new InvalidOperationException("Guard asleep notification occured when we thought guard was already asleep");
            }

            if (dateTime < LastChangeTime.Value)
            {
                throw new InvalidOperationException("Guard asleep notification occured before previous notification");
            }

            if (dateTime.Hour != 0)
            {
                throw new ArgumentException("Not expecting notification outside the midnight hour");
            }

            return new State(GuardSleepSlots, GuardId, false, dateTime);
        }

        /// <summary>
        /// Create a new <see cref="State"/>, updating this instances state based on a guard
        /// waking up.
        /// </summary>
        /// <param name="dateTime">The time of the notification.</param>
        /// <returns>The updated state.</returns>
        public State GuardWakes(DateTime dateTime)
        {
            if (!GuardId.HasValue)
            {
                throw new InvalidOperationException("Guard wakes notification occured before any guard came on duty");
            }

            if (CurrentlyAwake != false)
            {
                throw new InvalidOperationException("Guard wakes notification occured when we thought guard was already asleep");
            }

            if (dateTime < LastChangeTime.Value)
            {
                throw new InvalidOperationException("Guard wakes notification occured before previous notification");
            }

            if (dateTime.Hour != 0)
            {
                throw new ArgumentException("Not expecting notification outside the midnight hour");
            }

            int lastKnown = GetMinutes(LastChangeTime.Value);
            var slots = AddSleepyTime(GuardSleepSlots, GuardId.Value, lastKnown, dateTime.Minute - 1);

            return new State(slots, GuardId, true, dateTime);
        }

        private IImmutableDictionary<int, IImmutableDictionary<int, int>> AddSleepyTime(
            IImmutableDictionary<int, IImmutableDictionary<int, int>> slots,
            int guardId,
            int startMinutes,
            int endMinutes)
        {
            IEnumerable<int> minutesInWhichAsleep = Enumerable.Range(startMinutes, endMinutes - startMinutes + 1);
            if (!slots.TryGetValue(guardId, out IImmutableDictionary<int, int> guardSleepCounts))
            {
                var b = ImmutableDictionary.CreateRange(
                    minutesInWhichAsleep
                    .Select(i => new KeyValuePair<int, int>(i, 1)));

                return slots.Add(guardId, b.ToImmutableDictionary());
            }

            guardSleepCounts = minutesInWhichAsleep
                .Aggregate(
                    guardSleepCounts,
                    (s, i) => s.TryGetValue(i, out int currentSleepsThisSlot)
                        ? s.SetItem(i, currentSleepsThisSlot + 1)
                        : s.Add(i, 1));
            return slots.SetItem(guardId, guardSleepCounts);
        }

        private static int GetMinutes(DateTime dt) => (dt.Hour * 60) + dt.Minute;
    }
}
