using System.Linq;
using Day13;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Day13Tests
    {
        [TestMethod]
        public void CartMovingNorthOnNorthSouthTrack()
        {
            var c = new CartState(2, 2, CartDirection.North, 0).Move(TrackCell.NorthSouth);
            Assert.AreEqual(2, c.X, "X");
            Assert.AreEqual(1, c.Y, "Y");
            Assert.AreEqual(CartDirection.North, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingSouthOnNorthSouthTrack()
        {
            var c = new CartState(2, 2, CartDirection.South, 0).Move(TrackCell.NorthSouth);
            Assert.AreEqual(2, c.X, "X");
            Assert.AreEqual(3, c.Y, "Y");
            Assert.AreEqual(CartDirection.South, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingNorthOnNorthEastSouthWestTrack()
        {
            var c = new CartState(2, 2, CartDirection.North, 0).Move(TrackCell.NorthEastSouthWest);
            Assert.AreEqual(3, c.X, "X");
            Assert.AreEqual(2, c.Y, "Y");
            Assert.AreEqual(CartDirection.East, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingSouthOnNorthEastSouthWestTrack()
        {
            var c = new CartState(2, 2, CartDirection.South, 0).Move(TrackCell.NorthEastSouthWest);
            Assert.AreEqual(1, c.X, "X");
            Assert.AreEqual(2, c.Y, "Y");
            Assert.AreEqual(CartDirection.West, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingEastOnNorthEastSouthWestTrack()
        {
            var c = new CartState(2, 2, CartDirection.East, 0).Move(TrackCell.NorthEastSouthWest);
            Assert.AreEqual(2, c.X, "X");
            Assert.AreEqual(1, c.Y, "Y");
            Assert.AreEqual(CartDirection.North, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingWestOnNorthEastSouthWestTrack()
        {
            var c = new CartState(2, 2, CartDirection.West, 0).Move(TrackCell.NorthEastSouthWest);
            Assert.AreEqual(2, c.X, "X");
            Assert.AreEqual(3, c.Y, "Y");
            Assert.AreEqual(CartDirection.South, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingEastOnEastWestTrack()
        {
            var c = new CartState(2, 2, CartDirection.East, 0).Move(TrackCell.EastWest);
            Assert.AreEqual(3, c.X, "X");
            Assert.AreEqual(2, c.Y, "Y");
            Assert.AreEqual(CartDirection.East, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingWestOnEastWestTrack()
        {
            var c = new CartState(2, 2, CartDirection.West, 0).Move(TrackCell.EastWest);
            Assert.AreEqual(1, c.X, "X");
            Assert.AreEqual(2, c.Y, "Y");
            Assert.AreEqual(CartDirection.West, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingNorthOnNorthWestSouthEastTrack()
        {
            var c = new CartState(2, 2, CartDirection.North, 0).Move(TrackCell.NorthWestSouthEast);
            Assert.AreEqual(1, c.X, "X");
            Assert.AreEqual(2, c.Y, "Y");
            Assert.AreEqual(CartDirection.West, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingSouthOnNorthWestSouthEastTrack()
        {
            var c = new CartState(2, 2, CartDirection.South, 0).Move(TrackCell.NorthWestSouthEast);
            Assert.AreEqual(3, c.X, "X");
            Assert.AreEqual(2, c.Y, "Y");
            Assert.AreEqual(CartDirection.East, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingEastOnNorthWestSouthEastTrack()
        {
            var c = new CartState(2, 2, CartDirection.East, 0).Move(TrackCell.NorthWestSouthEast);
            Assert.AreEqual(2, c.X, "X");
            Assert.AreEqual(3, c.Y, "Y");
            Assert.AreEqual(CartDirection.South, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingWestOnNorthWestSouthEastTrack()
        {
            var c = new CartState(2, 2, CartDirection.West, 0).Move(TrackCell.NorthWestSouthEast);
            Assert.AreEqual(2, c.X, "X");
            Assert.AreEqual(1, c.Y, "Y");
            Assert.AreEqual(CartDirection.North, c.Direction, "Direction");
            Assert.AreEqual(0, c.IntersectionsEncountered, "IntersectionsEncountered");
        }

        [TestMethod]
        public void CartMovingNorthFirstFourthEtcTurnsLeft()
        {
            CartIntersectionTest(
                CartDirection.North,
                CartDirection.West,
                0);
        }

        [TestMethod]
        public void CartMovingNorthSecondFifthEtcHeadsStraightOn()
        {
            CartIntersectionTest(
                CartDirection.North,
                CartDirection.North,
                1);
        }

        [TestMethod]
        public void CartMovingNorthThirdSixthEtcTurnsRight()
        {
            CartIntersectionTest(
                CartDirection.North,
                CartDirection.East,
                2);
        }

        [TestMethod]
        public void CartMovingWestFirstFourthEtcTurnsLeft()
        {
            CartIntersectionTest(
                CartDirection.West,
                CartDirection.South,
                0);
        }

        [TestMethod]
        public void CartMovingWestSecondFifthEtcHeadsStraightOn()
        {
            CartIntersectionTest(
                CartDirection.West,
                CartDirection.West,
                1);
        }

        [TestMethod]
        public void CartMovingWestThirdSixthEtcTurnsRight()
        {
            CartIntersectionTest(
                CartDirection.West,
                CartDirection.North,
                2);
        }

        [TestMethod]
        public void CartMovingRightFirstFourthEtcTurnsLeft()
        {
            CartIntersectionTest(
                CartDirection.East,
                CartDirection.North,
                0);
        }

        [TestMethod]
        public void CartMovingRightSecondFifthEtcHeadsStraightOn()
        {
            CartIntersectionTest(
                CartDirection.East,
                CartDirection.East,
                1);
        }

        [TestMethod]
        public void CartMovingRightThirdSixthEtcTurnsRight()
        {
            CartIntersectionTest(
                CartDirection.East,
                CartDirection.South,
                2);
        }

        [TestMethod]
        public void CartMovingSouthFirstFourthEtcTurnsLeft()
        {
            CartIntersectionTest(
                CartDirection.South,
                CartDirection.East,
                0);
        }

        [TestMethod]
        public void CartMovingSouthSecondFifthEtcHeadsStraightOn()
        {
            CartIntersectionTest(
                CartDirection.South,
                CartDirection.South,
                1);
        }

        [TestMethod]
        public void CartMovingSouthThirdSixthEtcTurnsRight()
        {
            CartIntersectionTest(
                CartDirection.South,
                CartDirection.West,
                2);
        }

        private void CartIntersectionTest(
            CartDirection initialDirection,
            CartDirection newDirection,
            int iteration)
        {
            int dx = 0;
            int dy = 0;
            switch (newDirection)
            {
                case CartDirection.West:
                    dx = -1;
                    break;

                case CartDirection.East:
                    dx = 1;
                    break;

                case CartDirection.North:
                    dy = -1;
                    break;

                case CartDirection.South:
                    dy = 1;
                    break;
            }
            for (int i = 0; i < 3; ++i)
            {
                var c = new CartState(2, 2, initialDirection, iteration + (i * 3)).Move(TrackCell.Intersection);
                Assert.AreEqual(2 + dx, c.X, $"X ({i})");
                Assert.AreEqual(2 + dy, c.Y, $"Y ({i})");
                Assert.AreEqual(newDirection, c.Direction, $"Direction ({i})");
                Assert.AreEqual(iteration + (i * 3) + 1, c.IntersectionsEncountered, "IntersectionsEncountered");
            }
        }

        [TestMethod]
        public void SimpleCollision()
        {
            const string map = @"|
v
|
|
|
^
|";

            State initial = Program.LoadMap(map);
            Assert.AreEqual(0, initial.Carts[0].X);
            Assert.AreEqual(1, initial.Carts[0].Y);
            Assert.AreEqual(CartDirection.South, initial.Carts[0].Direction);
            Assert.AreEqual(0, initial.Carts[1].X);
            Assert.AreEqual(5, initial.Carts[1].Y);
            Assert.AreEqual(CartDirection.North, initial.Carts[1].Direction);

            State after1 = initial.Update(out var collisions);
            Assert.AreEqual(0, collisions.Count);
            Assert.AreEqual(0, after1.Carts[0].X);
            Assert.AreEqual(2, after1.Carts[0].Y);
            Assert.AreEqual(CartDirection.South, after1.Carts[0].Direction);
            Assert.AreEqual(0, after1.Carts[1].X);
            Assert.AreEqual(4, after1.Carts[1].Y);
            Assert.AreEqual(CartDirection.North, after1.Carts[1].Direction);

            State after2 = after1.Update(out collisions);
            Assert.AreEqual(1, collisions.Count);
            (int x, int y) final = collisions.Single();
            Assert.AreEqual((0, 3), final);
        }

        [TestMethod]
        public void CollisionExample2()
        {
            const string map =
@"/->-\        
|   |  /----\
| /-+--+-\  |
| | |  | v  |
\-+-/  \-+--/
  \------/";

            var firstCollision = Program.SolvePart1(map);
            Assert.AreEqual((7, 3), firstCollision);
        }

        [TestMethod]
        public void Part2Example()
        {
            const string map =
@"/>-<\  
|   |  
| /<+-\
| | | v
\>+</ |
  |   ^
  \<->/";

            var lastPosition = Program.SolvePart2(map);
            Assert.AreEqual((6, 4), lastPosition);
        }
    }
}
