using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Day07
{
    /// <summary>
    /// Represents the state of execution at a particular point in time.
    /// </summary>
    public sealed class State
    {
        private readonly char nextAvailable;

        private State(
            IImmutableDictionary<char, IImmutableSet<char>> waitingForByState)
        {
            WaitingForByState = waitingForByState;
            (HasNextAvailable, nextAvailable) = WaitingForByState
                .Where(kv => kv.Value.IsEmpty())
                .Aggregate(
                    (false, min: default(char)),
                    (a, s) => (true, a.min > s.Key || a.min == default ? s.Key : a.min));
        }

        /// <summary>
        /// Returns the initial state, based on a set of rules
        /// </summary>
        /// <param name="rules">The rules from which to construct the state.</param>
        /// <returns>The initial state.</returns>
        public static State Start(IEnumerable<StepConstraint> rules)
            => new State(MakeInitialWaitingForByState(rules));

        private static IImmutableDictionary<char, IImmutableSet<char>> MakeInitialWaitingForByState(
            IEnumerable<StepConstraint> rules) =>
            rules.Aggregate(
                ImmutableDictionary<char, IImmutableSet<char>>.Empty,
                (d, rule) => EnsureStepKnown(AddPrequisitesToRules(d, rule), rule.Prereq));

        private static ImmutableDictionary<char, IImmutableSet<char>> AddPrequisitesToRules(
            ImmutableDictionary<char, IImmutableSet<char>> d,
            StepConstraint rule)
                => d.TryGetValue(rule.Then, out var prereqs)
                    ? d.SetItem(rule.Then, prereqs.Add(rule.Prereq))
                    : d.Add(rule.Then, ImmutableHashSet.Create(rule.Prereq));

        private static ImmutableDictionary<char, IImmutableSet<char>> EnsureStepKnown(
            ImmutableDictionary<char, IImmutableSet<char>> d, char step) =>
            d.ContainsKey(step) ? d : d.Add(step, ImmutableHashSet<char>.Empty);

        private IImmutableDictionary<char, IImmutableSet<char>> WaitingForByState { get; }

        /// <summary>
        /// Gets a value indicating whether there is an available next step.
        /// </summary>
        public bool HasNextAvailable { get; }

        /// <summary>
        /// Gets the next available step.
        /// </summary>
        public char NextAvailable => HasNextAvailable
            ? nextAvailable
            : throw new InvalidOperationException("No next item available");

        /// <summary>
        /// Builds a new <see cref="State"/> representing the state after completing the specified step.
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public State ExecuteStep(char step) => new State(
            WaitingForByState
                .Where(kv => kv.Key != step)
                .Select(kv => (key: kv.Key, waitingFor: kv.Value.Remove(step)))
                .ToImmutableDictionary(x => x.key, x => x.waitingFor));
    }
}
