using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Day07
{
    public sealed class NationalElfService
    {
        private readonly int baseStepTime;

        private NationalElfService(
            IImmutableList<WorkInProgress> workerState,
            int baseStepTime)
        {
            this.WorkerState = workerState;
            this.baseStepTime = baseStepTime;
        }

        public static NationalElfService Create(int workerCount, int baseStepTime) => new NationalElfService(
            EnumerableEx.Repeat(default(WorkInProgress), workerCount).ToImmutableList(),
            baseStepTime);

        public (State stepStep, NationalElfService serviceState, IList<char> stepsDone) ProcessOneSecond(
            State stepState)
        {
            (State updatedStepState, IImmutableList<WorkInProgress> workerState, ImmutableList<char> completed) =
                Enumerable.Range(0, WorkerState.Count).Aggregate(
                (stepState, workerState: WorkerState, completed: ImmutableList<char>.Empty),
                (acc, workerIndex) =>
                {
                    WorkInProgress ws = acc.workerState[workerIndex];
                    return ws.MinutesRemaining == 1
                        ? (stepState.ExecuteStep(ws.Step),
                           acc.workerState.SetItem(workerIndex, null),
                           acc.completed.Add(ws.Step))
                        : (stepState,
                           acc.workerState.SetItem(workerIndex, new WorkInProgress(ws.Step, ws.MinutesRemaining - 1)),
                           acc.completed);
                });

            IEnumerable<(char step, int workerIndex)> workToStart = updatedStepState
                .NextAvailable
                .Zip(WorkerState.Select((w, i) => (w, i)).Where(x => x.w == null).Select(x => x.i));

            var updatedWorkerState = workToStart.Aggregate(
                workerState,
                (s, x) => s.SetItem(x.workerIndex, new WorkInProgress(x.step, GetStepExecutionTime(x.step))));

            return (updatedStepState, new NationalElfService(updatedWorkerState, baseStepTime), completed);
        }

        /// <summary>
        /// Gets a list of which workers are doing what.
        /// </summary>
        public IImmutableList<WorkInProgress> WorkerState { get; }

        private int GetStepExecutionTime(char step) => step - 'A' + baseStepTime;

        public class WorkInProgress
        {
            public WorkInProgress(char step, int minutesRemaining)
            {
                Step = step;
                MinutesRemaining = minutesRemaining;
            }

            public char Step { get; }

            public int MinutesRemaining { get; }
        }
    }
}
