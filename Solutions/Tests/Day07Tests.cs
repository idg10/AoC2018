using System.Linq;
using Common;
using Day07;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class Day07Tests
    {
        [TestMethod]
        public void Part1Example()
        {
            string sequence = Program.Part1(Program.ExampleInput);
            Assert.AreEqual("CABDFE", sequence);
        }

        [TestMethod]
        public void Part1IdgSolution()
        {
            string part1 = Program.Part1(InputReader.ParseLines(typeof(Program), Program.LineParser));
            Assert.AreEqual("BCADPVTJFZNRWXHEKSQLUYGMIO", part1);
        }

        [TestMethod]
        public void Part2Example()
        {
            var steps = Program.RunPart2(Program.ExampleInput, 2, 0).Take(40).ToList();
            Assert.AreEqual(15, steps.Count);

            Assert.AreEqual('C', steps[0].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[0].serviceState.WorkerState[1]);
            Assert.AreEqual("", steps[0].stepsDone);

            Assert.AreEqual('C', steps[1].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[1].serviceState.WorkerState[1]);
            Assert.AreEqual("", steps[1].stepsDone);

            Assert.AreEqual('C', steps[2].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[2].serviceState.WorkerState[1]);
            Assert.AreEqual("", steps[2].stepsDone);

            Assert.AreEqual('A', steps[3].serviceState.WorkerState[0].Step);
            Assert.AreEqual('F', steps[3].serviceState.WorkerState[1].Step);
            Assert.AreEqual("C", steps[3].stepsDone);

            Assert.AreEqual('B', steps[4].serviceState.WorkerState[0].Step);
            Assert.AreEqual('F', steps[4].serviceState.WorkerState[1].Step);
            Assert.AreEqual("CA", steps[4].stepsDone);

            Assert.AreEqual('B', steps[5].serviceState.WorkerState[0].Step);
            Assert.AreEqual('F', steps[5].serviceState.WorkerState[1].Step);
            Assert.AreEqual("CA", steps[5].stepsDone);

            Assert.AreEqual('D', steps[6].serviceState.WorkerState[0].Step);
            Assert.AreEqual('F', steps[6].serviceState.WorkerState[1].Step);
            Assert.AreEqual("CAB", steps[6].stepsDone);

            Assert.AreEqual('D', steps[7].serviceState.WorkerState[0].Step);
            Assert.AreEqual('F', steps[7].serviceState.WorkerState[1].Step);
            Assert.AreEqual("CAB", steps[7].stepsDone);

            Assert.AreEqual('D', steps[8].serviceState.WorkerState[0].Step);
            Assert.AreEqual('F', steps[8].serviceState.WorkerState[1].Step);
            Assert.AreEqual("CAB", steps[8].stepsDone);

            Assert.AreEqual('D', steps[9].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[9].serviceState.WorkerState[1]);
            Assert.AreEqual("CABF", steps[9].stepsDone);

            Assert.AreEqual('E', steps[10].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[10].serviceState.WorkerState[1]);
            Assert.AreEqual("CABFD", steps[10].stepsDone);

            Assert.AreEqual('E', steps[11].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[11].serviceState.WorkerState[1]);
            Assert.AreEqual("CABFD", steps[11].stepsDone);

            Assert.AreEqual('E', steps[12].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[12].serviceState.WorkerState[1]);
            Assert.AreEqual("CABFD", steps[12].stepsDone);

            Assert.AreEqual('E', steps[13].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[13].serviceState.WorkerState[1]);
            Assert.AreEqual("CABFD", steps[13].stepsDone);

            Assert.AreEqual('E', steps[14].serviceState.WorkerState[0].Step);
            Assert.IsNull(steps[14].serviceState.WorkerState[1]);
            Assert.AreEqual("CABFD", steps[14].stepsDone);
        }
    }
}
