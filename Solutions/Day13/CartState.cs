using System;

namespace Day13
{
    [System.Diagnostics.DebuggerDisplay("({X},{Y}) {Direction} {IntersectionsEncountered}")]
    public class CartState
    {
        public CartState(
            int x,
            int y,
            CartDirection direction,
            int intersectionsEncountered)
        {
            X = x;
            Y = y;
            Direction = direction;
            IntersectionsEncountered = intersectionsEncountered;
        }

        public int X { get; }
        public int Y { get; }
        public CartDirection Direction { get; }
        public int IntersectionsEncountered { get; }

        public CartState Move(TrackCell track)
        {
            int x = X;
            int y = Y;
            int i = IntersectionsEncountered;
            CartDirection d = Direction;
            switch (track)
            {
                case TrackCell.NorthSouth:
                    switch (Direction)
                    {
                        case CartDirection.North:
                        case CartDirection.South:
                            break;

                        default:
                            throw new InvalidOperationException("On N/S track and attempted to move " + Direction);
                    }
                    break;

                case TrackCell.EastWest:
                    switch (Direction)
                    {
                        case CartDirection.East:
                        case CartDirection.West:
                            break;

                        default:
                            throw new InvalidOperationException("On E/W track and attempted to move " + Direction);
                    }
                    break;

                case TrackCell.NorthEastSouthWest:
                    switch (d)
                    {
                        case CartDirection.North: d = CartDirection.East; break;
                        case CartDirection.South: d = CartDirection.West; break;
                        case CartDirection.East: d = CartDirection.North; break;
                        case CartDirection.West: d = CartDirection.South; break;
                    }
                    break;

                case TrackCell.NorthWestSouthEast:
                    switch (d)
                    {
                        case CartDirection.North: d = CartDirection.West; break;
                        case CartDirection.South: d = CartDirection.East; break;
                        case CartDirection.East: d = CartDirection.South; break;
                        case CartDirection.West: d = CartDirection.North; break;
                    }
                    break;

                case TrackCell.Intersection:
                    i += 1;
                    int turnDelta = (IntersectionsEncountered % 3) - 1;
                    d = (CartDirection) (((int) d + turnDelta + 4) % 4);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(track), "Unrecognized track direction " + track);
            }

            int dx = 0;
            int dy = 0;
            switch (d)
            {
                case CartDirection.North: dy = -1; break;
                case CartDirection.South: dy = 1; break;
                case CartDirection.West: dx = -1; break;
                case CartDirection.East: dx = 1; break;
            }
            return new CartState(
                X + dx,
                Y + dy,
                d,
                i);
        }
    }
}
