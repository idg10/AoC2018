using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Day13
{
    public class State
    {
        public State(
            Map map,
            ImmutableList<CartState> carts)
        {
            this.Map = map;
            this.Carts = carts;
        }

        public Map Map { get; }
        public ImmutableList<CartState> Carts { get; }

        public State Update(out ICollection<(int x, int y)> collisions)
        {
            (ImmutableList<CartState> carts, ImmutableList<(int x, int y)> c) = Enumerable
                .Range(0, Carts.Count)
                .Aggregate(
                    (carts: Carts, collisions: ImmutableList<(int x, int y)>.Empty),
                    (a, i) =>
                    {
                        CartState cart = a.carts[i];
                        TrackCell cell = Map.Get(cart.X, cart.Y);
                        CartState updatedCart = cart.Move(cell);

                        return (
                            a.carts.SetItem(i, updatedCart),
                            a.carts.Any(oc => oc.X == updatedCart.X && oc.Y == updatedCart.Y)
                                ? a.collisions.Add((updatedCart.X, updatedCart.Y))
                                : a.collisions);
                    });

            collisions = c;
            return new State(Map, carts);
        }
    }
}
