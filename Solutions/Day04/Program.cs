using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Common;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Day04
{
    internal static class Program
    {
        private static void Main()
        {
            Parser<int> pStartShift = between(str("Guard #"), str("begins shift"), pInt32);
            Parser<string> pFallsAsleep = str("falls asleep");
            Parser<string> pWakesUp = str("wakes up");

            Parser<Action<INotifications>> recordParser = choice(
                pStartShift.Select(id => (Action<INotifications>)(n => n.BeginShift(id))),
                pFallsAsleep.Select(_ => (Action<INotifications>) (n => n.FallsAsleep())),
                pWakesUp.Select(_ => (Action<INotifications>) (n => n.WakesUp())));

            string[] testInput =
            {
                "[1518-11-01 00:00] Guard #10 begins shift",
                "[1518-11-01 00:05] falls asleep",
                "[1518-11-01 00:25] wakes up",
                "[1518-11-01 00:30] falls asleep",
                "[1518-11-01 00:55] wakes up",
                "[1518-11-01 23:58] Guard #99 begins shift",
                "[1518-11-02 00:40] falls asleep",
                "[1518-11-02 00:50] wakes up",
                "[1518-11-03 00:05] Guard #10 begins shift",
                "[1518-11-03 00:24] falls asleep",
                "[1518-11-03 00:29] wakes up",
                "[1518-11-04 00:02] Guard #99 begins shift",
                "[1518-11-04 00:36] falls asleep",
                "[1518-11-04 00:46] wakes up",
                "[1518-11-05 00:03] Guard #99 begins shift",
                "[1518-11-05 00:45] falls asleep",
                "[1518-11-05 00:55] wakes up"
            };

            // Parse lines of this form:
            //  [1518-11-01 00:00] Guard #10 begins shift
            Parser<(DateTime dateTime, Action<INotifications> notifier)> lineParser =
                from dateTime in between(ch('['), ch(']'), pDateTime)
                from _ in spaces
                from n in recordParser
                select (dateTime, n);

            Console.WriteLine("Test");
            Solve(testInput.Select(text => ProcessLine(lineParser, text)));

            Console.WriteLine("Real");
            Solve(InputReader.ParseLines(typeof(Program), lineParser));
        }

        private static void Solve(IEnumerable<(DateTime dateTime, Action<INotifications> notifier)> items)
        {
            State result = items
                .OrderBy(l => l.dateTime)
                .Aggregate(State.Initial, ProcessNotification);

            KeyValuePair<int, IImmutableDictionary<int, int>> mostAsleep = result
                .GuardSleepSlots
                .OrderBy(kv => kv.Value.Sum(kv2 => kv2.Value))
                .Last();
            int mostAsleepGuardId = mostAsleep.Key;
            Console.WriteLine($"Most asleep guard: {mostAsleepGuardId}");

            int timeMostAsleep = mostAsleep.Value.OrderBy(kv => kv.Value).Last().Key;
            Console.WriteLine($"Time most asleep: {timeMostAsleep}");

            Console.WriteLine($"Part 1 result: {mostAsleepGuardId * timeMostAsleep}");

            var mostSleptDuring = result
                .GuardSleepSlots
                .Select(s => (guardId: s.Key, minuteAndSleeps: s.Value.OrderBy(timeAndSleeps => timeAndSleeps.Value).Last()))
                .OrderBy(x => x.minuteAndSleeps.Value)
                .Last();
            int guardWithMostFrequentSleepSlot = mostSleptDuring.guardId;
            int mostFrequentSleepSlot = mostSleptDuring.minuteAndSleeps.Key;
            int sleepsDuringMostFrequentSlot = mostSleptDuring.minuteAndSleeps.Value;
            Console.WriteLine($"Guard {guardWithMostFrequentSleepSlot} slept for {sleepsDuringMostFrequentSlot} at {mostFrequentSleepSlot}");
            Console.WriteLine($"Part 2 result: {guardWithMostFrequentSleepSlot * mostFrequentSleepSlot}");
        }

        private static State ProcessNotification(
            State state,
            (DateTime dateTime, Action<INotifications> notifier) notification)
        {
            var nr = new NotifyReceiver(state, notification.dateTime);
            notification.notifier(nr);
            return nr.State;
        }

        /// <summary>
        /// Notification processing performed by <see cref="ProcessNotification(State, (DateTime dateTime, Action{INotifications} notifier))"/>.
        /// </summary>
        /// <remarks>
        /// In F# this would be a pattern match on a sum type representing a single parsed line.
        /// The mutability here is just tolerable because we're using it to make up for a missing
        /// language feature.
        /// </remarks>
        private class NotifyReceiver : INotifications
        {
            private readonly DateTime dateTime;

            public NotifyReceiver(
                State state,
                DateTime dateTime)
            {
                State = state;
                this.dateTime = dateTime;
            }

            public State State { get; private set; }

            public void BeginShift(int guardId) => State = State.ChangeGuard(guardId, dateTime);
            public void FallsAsleep() => State = State.GuardSleeps(dateTime);
            public void WakesUp() => State = State.GuardWakes(dateTime);
        }
    }
}
