using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Day15;

namespace Tests
{
    [TestClass]
    public class Day15Tests
    {
        [TestMethod]
        public void NearestIsSelectedInReadingOrder()
        {
            // In the map below, we focus our attention on the square directly
            // belong the only Elf. This is adjacent to two cells in range of
            // two different Goblins. So the distance to each Goblin is 2, meaning
            // there is a tie in the 'nearest' ranking. The rules state that such
            // ties must be broken by using reading order. The Goblin on the 2nd
            // line comes before the one on the 3rd line, so we must check that
            // this is the reported one. (Without taking special steps, it won't
            // be. We populate the closeness map by growing out one step at a time
            // from each Goblin (and from each Elf), meaning that after 1 pass,
            // the 3rd line will look like this:
            //   #.1.1G#
            // with that first 1 denoting a distance of 1 to the Goblin directly
            // beneath it (Goblin 1) and the second denoting a distance of 1 to
            // the Goblin to the right (Goblin 0). Since each pass works through
            // the grid in reading order, we'll see that first 1 before we see
            // the second, 
            const string map =
@"#######
#..E..#
#....G#
#.G...#
#.....#
#######";
            GridCell[,] grid = GridOperations.ParseGrid(map);
            grid = GridOperations.CalculateCloseness(grid);

            Assert.AreEqual(0, grid[2, 5].GoblinId, "Test setup error");

            GridCell testCell = grid[1, 3];
            Assert.AreEqual(3, testCell.DistanceToGoblinInRange);
            Assert.AreEqual(0, testCell.GoblinId);
            Assert.AreEqual((5, 1), testCell.GoblinInRangePosition);
        }

        [TestMethod]
        public void Part1MoveExample1()
        {
            const string map =
@"#######
#.E...#
#.....#
#...G.#
#######";

            GridCell[,] grid = GridOperations.ParseGrid(map);
            grid = GridOperations.CalculateCloseness(grid);

            var p = GridOperations.CalculateMove(grid, 2, 1);
            Assert.AreEqual((1, 0), p);
        }

        [TestMethod]
        public void Part1MoveExample2()
        {
            const string map =
@"#########
#G..G..G#
#.......#
#.......#
#G..E..G#
#.......#
#.......#
#G..G..G#
#########";
            GridCell[,] grid = GridOperations.ParseGrid(map);
            var gameState = GameState.Start(grid);
            grid = gameState.Grid;

            IsGoblin(0, grid, 1, 1);
            IsGoblin(1, grid, 4, 1);
            IsGoblin(2, grid, 7, 1);
            IsGoblin(3, grid, 1, 4);
            IsGoblin(4, grid, 7, 4);
            IsGoblin(5, grid, 1, 7);
            IsGoblin(6, grid, 4, 7);
            IsGoblin(7, grid, 7, 7);
            IsElf(0, grid, 4, 4);

            gameState = gameState.PlayRound().state;
            grid = gameState.Grid;

            IsGoblin(0, grid, 2, 1);
            IsGoblin(1, grid, 4, 2);
            IsGoblin(2, grid, 6, 1);
            IsGoblin(3, grid, 2, 4);
            IsGoblin(4, grid, 7, 3);
            IsGoblin(5, grid, 1, 6);
            IsGoblin(6, grid, 4, 6);
            IsGoblin(7, grid, 7, 6);
            IsElf(0, grid, 4, 3);

            gameState = gameState.PlayRound().state;
            grid = gameState.Grid;

            IsGoblin(0, grid, 3, 1);
            IsGoblin(1, grid, 4, 2);
            IsGoblin(2, grid, 5, 1);
            IsGoblin(3, grid, 2, 3);
            IsGoblin(4, grid, 6, 3);
            IsGoblin(5, grid, 1, 5);
            IsGoblin(6, grid, 4, 5);
            IsGoblin(7, grid, 7, 5);
            IsElf(0, grid, 4, 3);

            gameState = gameState.PlayRound().state;
            grid = gameState.Grid;

            IsGoblin(0, grid, 3, 2);
            IsGoblin(1, grid, 4, 2);
            IsGoblin(2, grid, 5, 2);
            IsGoblin(3, grid, 3, 3);
            IsGoblin(4, grid, 5, 3);
            IsGoblin(5, grid, 1, 4);
            IsGoblin(6, grid, 4, 4);
            IsGoblin(7, grid, 7, 5);
            IsElf(0, grid, 4, 3);

            gameState = gameState.PlayRound().state;
            grid = gameState.Grid;

            IsGoblin(0, grid, 3, 2);
            IsGoblin(1, grid, 4, 2);
            IsGoblin(2, grid, 5, 2);
            IsGoblin(3, grid, 3, 3);
            IsGoblin(4, grid, 5, 3);
            IsGoblin(5, grid, 1, 4);
            IsGoblin(6, grid, 4, 4);
            IsGoblin(7, grid, 7, 5);
            IsElf(0, grid, 4, 3);
        }

        [TestMethod]
        public void Part1PlayExample()
        {
            const string map =
@"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";
            var state = GameState.Start(GridOperations.ParseGrid(map));

            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(200, state.GoblinHitPoints[1]);
            Assert.AreEqual(200, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(200, state.ElfHitPoints[0]);
            Assert.AreEqual(200, state.ElfHitPoints[1]);

            // Round 1
            state = state.PlayRound().state;
            var grid = state.Grid;

            IsGoblin(0, grid, 3, 1);
            IsGoblin(1, grid, 5, 2);
            IsGoblin(2, grid, 5, 3);
            IsGoblin(3, grid, 3, 3);
            IsElf(0, grid, 4, 2);
            IsElf(1, grid, 5, 4);
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(197, state.GoblinHitPoints[1]);
            Assert.AreEqual(197, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(197, state.ElfHitPoints[0]);
            Assert.AreEqual(197, state.ElfHitPoints[1]);

            // Round 2
            state = state.PlayRound().state;
            grid = state.Grid;

            IsGoblin(0, grid, 4, 1);
            IsGoblin(1, grid, 5, 2);
            IsGoblin(2, grid, 5, 3);
            IsGoblin(3, grid, 3, 2);
            IsElf(0, grid, 4, 2);
            IsElf(1, grid, 5, 4);
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(194, state.GoblinHitPoints[1]);
            Assert.AreEqual(194, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(188, state.ElfHitPoints[0]);
            Assert.AreEqual(194, state.ElfHitPoints[1]);

            for (int i = 0; i < 21; ++i)
            {
                state = state.PlayRound().state;
                grid = state.Grid;

                IsGoblin(0, grid, 4, 1);
                IsGoblin(1, grid, 5, 2);
                IsGoblin(2, grid, 5, 3);
                IsGoblin(3, grid, 3, 2);
                IsElf(1, grid, 5, 4);
                if (i < 20)
                {
                    IsElf(0, grid, 4, 2);
                }
                else
                {
                    Assert.IsFalse(grid[2, 4].IsElf);
                }
            }
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(131, state.GoblinHitPoints[1]);
            Assert.AreEqual(131, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(-1, state.ElfHitPoints[0]);
            Assert.AreEqual(131, state.ElfHitPoints[1]);

            // Round 24
            state = state.PlayRound().state;
            grid = state.Grid;

            IsGoblin(0, grid, 3, 1);
            IsGoblin(1, grid, 4, 2);
            IsGoblin(2, grid, 5, 3);
            IsGoblin(3, grid, 3, 3);
            IsElf(1, grid, 5, 4);
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(131, state.GoblinHitPoints[1]);
            Assert.AreEqual(128, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(128, state.ElfHitPoints[1]);

            // Round 25
            state = state.PlayRound().state;
            grid = state.Grid;

            IsGoblin(0, grid, 2, 1);
            IsGoblin(1, grid, 3, 2);
            IsGoblin(2, grid, 5, 3);
            IsGoblin(3, grid, 3, 4);
            IsElf(1, grid, 5, 4);
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(131, state.GoblinHitPoints[1]);
            Assert.AreEqual(125, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(125, state.ElfHitPoints[1]);

            // Round 26
            state = state.PlayRound().state;
            grid = state.Grid;

            IsGoblin(0, grid, 1, 1);
            IsGoblin(1, grid, 2, 2);
            IsGoblin(2, grid, 5, 3);
            IsGoblin(3, grid, 3, 5);
            IsElf(1, grid, 5, 4);
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(131, state.GoblinHitPoints[1]);
            Assert.AreEqual(122, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(122, state.ElfHitPoints[1]);

            // Round 27
            state = state.PlayRound().state;
            grid = state.Grid;

            IsGoblin(0, grid, 1, 1);
            IsGoblin(1, grid, 2, 2);
            IsGoblin(2, grid, 5, 3);
            IsGoblin(3, grid, 4, 5);
            IsElf(1, grid, 5, 4);
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(131, state.GoblinHitPoints[1]);
            Assert.AreEqual(119, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(119, state.ElfHitPoints[1]);

            // Round 28
            state = state.PlayRound().state;
            grid = state.Grid;

            IsGoblin(0, grid, 1, 1);
            IsGoblin(1, grid, 2, 2);
            IsGoblin(2, grid, 5, 3);
            IsGoblin(3, grid, 5, 5);
            IsElf(1, grid, 5, 4);
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(131, state.GoblinHitPoints[1]);
            Assert.AreEqual(116, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(113, state.ElfHitPoints[1]);

            // Round 29-47
            for (int i = 0; i < 19; ++i)
            {
                state = state.PlayRound().state;
                grid = state.Grid;

                IsGoblin(0, grid, 1, 1);
                IsGoblin(1, grid, 2, 2);
                IsGoblin(2, grid, 5, 3);
                IsGoblin(3, grid, 5, 5);
                if (i < 18)
                {
                    IsElf(1, grid, 5, 4);
                }
                else
                {
                    Assert.IsFalse(grid[5, 4].IsElf);
                }
            }
            Assert.AreEqual(200, state.GoblinHitPoints[0]);
            Assert.AreEqual(131, state.GoblinHitPoints[1]);
            Assert.AreEqual(59, state.GoblinHitPoints[2]);
            Assert.AreEqual(200, state.GoblinHitPoints[3]);
            Assert.AreEqual(-1, state.ElfHitPoints[1]);
        }

        [TestMethod]
        public void Part1ExampleResult1()
        {
            const string map =
@"#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######";
            Assert.AreEqual(36334, Program.SolvePart1(map).score);
        }

        [TestMethod]
        public void Part1ExampleResult2()
        {
            const string map =
@"#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######";
            Assert.AreEqual(39514, Program.SolvePart1(map).score);
        }

        [TestMethod]
        public void Part1ExampleResult3()
        {
            const string map =
@"#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######";
            Assert.AreEqual(27755, Program.SolvePart1(map).score);
        }

        [TestMethod]
        public void Part1ExampleResult4()
        {
            const string map =
@"#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######";
            Assert.AreEqual(28944, Program.SolvePart1(map).score);
        }

        [TestMethod]
        public void Part1ExampleResult5()
        {
            const string map =
@"#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########";
            Assert.AreEqual(18740, Program.SolvePart1(map).score);
        }

        [TestMethod]
        public void Part2Example1()
        {
            const string map =
@"#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######";
            Assert.AreEqual((15, 4988), Program.SolvePart2(map));
        }

        [TestMethod]
        public void Part2Example2()
        {
            const string map =
@"#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######";
            Assert.AreEqual(31284, Program.SolvePart1(map, elfAttackPower:4).score);
            Assert.AreEqual((4, 31284), Program.SolvePart2(map));
        }

        [TestMethod]
        public void Part2Example3()
        {
            const string map =
@"#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######";
            Assert.AreEqual(3478, Program.SolvePart1(map, elfAttackPower: 15).score);
            Assert.AreEqual((15, 3478), Program.SolvePart2(map));
        }

        [TestMethod]
        public void Part2Example4()
        {
            const string map =
@"#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######";
            Assert.AreEqual((12, 6474), Program.SolvePart2(map));
        }

        [TestMethod]
        public void Part2Example5()
        {
            const string map =
@"#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########";
            Assert.AreEqual((34, 1140), Program.SolvePart2(map));
        }

        private void IsGoblin(int id, GridCell[,] grid, int x, int y)
        {
            Assert.IsTrue(grid[y, x].IsGoblin);
            Assert.AreEqual(id, grid[y, x].GoblinId);
        }

        private void IsElf(int id, GridCell[,] grid, int x, int y)
        {
            Assert.IsTrue(grid[y, x].IsElf);
            Assert.AreEqual(id, grid[y, x].ElfId);
        }
    }
}
