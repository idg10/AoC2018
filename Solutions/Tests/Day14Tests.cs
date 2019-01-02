using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Day14;
using System.Collections.Immutable;
using System.Linq;

namespace Tests
{
    [TestClass]
    public class Day14Tests
    {
        [TestMethod]
        public void GetNewRecipesExample1()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(0, 1, 3, 7);
            Assert.AreEqual(2, recipes.Length);
            Assert.AreEqual(1, recipes[0]);
            Assert.AreEqual(0, recipes[1]);

            Assert.AreEqual(0, positions[0]);
            Assert.AreEqual(1, positions[1]);
        }

        [TestMethod]
        public void GetNewRecipesExample2()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(0, 1, 3, 7, 1, 0);
            Assert.AreEqual(2, recipes.Length);
            Assert.AreEqual(1, recipes[0]);
            Assert.AreEqual(0, recipes[1]);

            Assert.AreEqual(4, positions[0]);
            Assert.AreEqual(3, positions[1]);
        }

        [TestMethod]
        public void GetNewRecipesExample3()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(4, 3, 3, 7, 1, 0, 1, 0);
            Assert.AreEqual(1, recipes.Length);
            Assert.AreEqual(1, recipes[0]);

            Assert.AreEqual(6, positions[0]);
            Assert.AreEqual(4, positions[1]);
        }

        [TestMethod]
        public void GetNewRecipesExample4()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(6, 4, 3, 7, 1, 0, 1, 0, 1);
            Assert.AreEqual(1, recipes.Length);
            Assert.AreEqual(2, recipes[0]);

            Assert.AreEqual(0, positions[0]);
            Assert.AreEqual(6, positions[1]);
        }

        [TestMethod]
        public void GetNewRecipesExample5()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(0, 6, 3, 7, 1, 0, 1, 0, 1, 2);
            Assert.AreEqual(1, recipes.Length);
            Assert.AreEqual(4, recipes[0]);

            Assert.AreEqual(4, positions[0]);
            Assert.AreEqual(8, positions[1]);
        }

        [TestMethod]
        public void GetNewRecipesExample6()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(4, 8, 3, 7, 1, 0, 1, 0, 1, 2, 4);
            Assert.AreEqual(1, recipes.Length);
            Assert.AreEqual(5, recipes[0]);

            Assert.AreEqual(6, positions[0]);
            Assert.AreEqual(3, positions[1]);
        }

        [TestMethod]
        public void GetNewRecipesExample7()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(6, 3, 3, 7, 1, 0, 1, 0, 1, 2, 4, 5);
            Assert.AreEqual(1, recipes.Length);
            Assert.AreEqual(1, recipes[0]);

            Assert.AreEqual(8, positions[0]);
            Assert.AreEqual(4, positions[1]);
        }

        [TestMethod]
        public void GetNewRecipesExample8()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(8, 4, 3, 7, 1, 0, 1, 0, 1, 2, 4, 5, 1);
            Assert.AreEqual(1, recipes.Length);
            Assert.AreEqual(5, recipes[0]);

            Assert.AreEqual(1, positions[0]);
            Assert.AreEqual(6, positions[1]);
        }

        [TestMethod]
        public void GetNewRecipesExample11()
        {
            (byte[] recipes, int[] positions) = MakeRecipes(1, 13, 3, 7, 1, 0, 1, 0, 1, 2, 4, 5, 1, 5, 8, 9);
            Assert.AreEqual(2, recipes.Length);
            Assert.AreEqual(1, recipes[0]);
            Assert.AreEqual(6, recipes[1]);

            Assert.AreEqual(9, positions[0]);
            Assert.AreEqual(7, positions[1]);
        }

        private static (byte[] recipes, int[] positions) MakeRecipes(
            int p1,
            int p2,
            params byte[] existing)
        {
            (IList<byte> scoreBoard, IEnumerable<int> elfPositions) = Program.GetNewRecipes(
                ImmutableList.Create<byte>(existing), existing.Length, new[] { p1, p2 });
            return (scoreBoard.ToArray(), elfPositions.ToArray());
        }

        [TestMethod]
        public void Part1Example1()
        {
            Assert.AreEqual("5158916779", Program.SolvePart1(9));
        }

        [TestMethod]
        public void Part1Example2()
        {
            Assert.AreEqual("0124515891", Program.SolvePart1(5));
        }

        [TestMethod]
        public void Part1Example3()
        {
            Assert.AreEqual("9251071085", Program.SolvePart1(18));
        }

        [TestMethod]
        public void Part1Example4()
        {
            Assert.AreEqual("5941429882", Program.SolvePart1(2018));
        }

        [TestMethod]
        public void Part1Idg10()
        {
            Assert.AreEqual("5992684592", Program.SolvePart1(165061));
        }

        [TestMethod]
        public void Part2Example1()
        {
            Assert.AreEqual(9, Program.SolvePart2(100, "51589"));
        }

        [TestMethod]
        public void Part2Example2()
        {
            Assert.AreEqual(5, Program.SolvePart2(100, "01245"));
        }

        [TestMethod]
        public void Part2Example3()
        {
            Assert.AreEqual(18, Program.SolvePart2(100, "92510"));
        }

        [TestMethod]
        public void Part2Example4()
        {
            Assert.AreEqual(2018, Program.SolvePart2(3000, "59414"));
        }
    }
}
