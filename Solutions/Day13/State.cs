using System;
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
            (ImmutableList<CartState> carts, ImmutableList<(int x, int y)> c) = Carts
                .OrderBy(cx => cx.X)
                .ThenBy(cx => cx.Y)
                .Aggregate(
                    (carts: Carts, collisions: ImmutableList<(int x, int y)>.Empty),
                    (a, cart) =>
                    {
                        if (!a.carts.Contains(cart))
                        {
                            // We already removed cart this in an earlier iteration due to collision,
                            // so we shouldn't attempt to process it.
                            return a;
                        }

                        TrackCell cell = Map.Get(cart.X, cart.Y);
                        CartState updatedCart = cart.Move(cell);

                        // After moving each cart we need to check to see if it collides with
                        // an existing cart, and if so, we want to remove *both* carts.
                        var clash = a.carts
                            .Select((clashingCart, i) => (clashingCart, clashingIndex: (int?) i))
                            .SingleOrDefault(x => x.clashingCart.X == updatedCart.X && x.clashingCart.Y == updatedCart.Y);
                        bool collision = clash.clashingIndex.HasValue;
                        if (collision)
                        {
                            Console.WriteLine($"Removing carts at ({clash.clashingCart.X},{clash.clashingCart.Y}). Indices {a.carts.IndexOf(cart)}, {clash.clashingIndex}");
                        }

                        return (
                            collision
                                ? a.carts.RemoveAt(clash.clashingIndex.Value).Remove(cart)
                                : a.carts.SetItem(a.carts.IndexOf(cart), updatedCart),
                            collision
                                ? a.collisions.Add((updatedCart.X, updatedCart.Y))
                                : a.collisions);
                    });

            collisions = c;
            return new State(Map, carts);
        }
    }
}
