using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Day09
{
    public sealed class GameState
    {
        private GameState(
            IImmutableList<int> marbles,
            int current,
            int lastTurn)
        {
            Marbles = marbles;
            CurrentMarblePosition = current;
            LastTurn = lastTurn;
        }

        public IImmutableList<int> Marbles { get; }

        public int CurrentMarblePosition { get; }

        public static GameState Initial { get; } = new GameState(ImmutableList.Create(0), 0, 0);

        public int LastTurn { get; }

        public (GameState nextState, int? valueRemoved) Next()
        {
            int turn = LastTurn + 1;

            if (turn % 23 == 0)
            {
                int marbleToRemove = (CurrentMarblePosition - 7 + Marbles.Count) % Marbles.Count;
                int scoreOfRemovedMarble = Marbles[marbleToRemove];
                int newPosition = marbleToRemove;
                var newState = new GameState(
                    Marbles.RemoveAt(marbleToRemove),
                    newPosition,
                    turn);
                return (newState, scoreOfRemovedMarble + turn);
            }
            else
            {
                int insertAfter = (CurrentMarblePosition + 1) % Marbles.Count;
                int newPosition = insertAfter + 1;
                var newState = new GameState(
                    Marbles.Insert(newPosition, turn),
                    newPosition,
                    turn);
                return (newState, default);
            }
        }
    }
}
