using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Day16;
using Common;
using Day16.Instructions;

namespace Tests
{
    [TestClass]
    public class Day16Tests
    {
        [TestMethod]
        public void Addr()
        {
            var r0 = new Registers(2, 3, 0, 0);
            Assert.AreEqual(
                new Registers(2, 3, r0.R0 + r0.R1, 0),
                Opcode.Opcodes["addr"](0, 1, 2).Execute(r0));
        }

        [TestMethod]
        public void Addi()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(r0.R2 + 6, 3, 5, 6),
                Opcode.Opcodes["addi"](2, 6, 0).Execute(r0));
        }

        [TestMethod]
        public void Mulr()
        {
            var r0 = new Registers(2, 3, 0, 0);
            Assert.AreEqual(
                new Registers(2, 3, r0.R0 * r0.R1, 0),
                Opcode.Opcodes["mulr"](0, 1, 2).Execute(r0));
        }

        [TestMethod]
        public void Muli()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(r0.R2 * 6, 3, 5, 6),
                Opcode.Opcodes["muli"](2, 6, 0).Execute(r0));
        }

        [TestMethod]
        public void Banr()
        {
            var r0 = new Registers(0xff00, 0x4242, 0, 0);
            Assert.AreEqual(
                new Registers(0xff00, 0x4242, r0.R0 & r0.R1, 0),
                Opcode.Opcodes["banr"](0, 1, 2).Execute(r0));
        }

        [TestMethod]
        public void Bani()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(r0.R2 & 6, 3, 5, 6),
                Opcode.Opcodes["bani"](2, 6, 0).Execute(r0));
        }

        [TestMethod]
        public void Borr()
        {
            var r0 = new Registers(0xff00, 0x4242, 0, 0);
            Assert.AreEqual(
                new Registers(0xff00, 0x4242, r0.R0 | r0.R1, 0),
                Opcode.Opcodes["borr"](0, 1, 2).Execute(r0));
        }

        [TestMethod]
        public void Bori()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(r0.R2 | 6, 3, 5, 6),
                Opcode.Opcodes["bori"](2, 6, 0).Execute(r0));
        }

        [TestMethod]
        public void Setr()
        {
            var r0 = new Registers(0xff00, 0x4242, 0, 0);
            Assert.AreEqual(
                new Registers(0xff00, 0x4242, r0.R0, 0),
                Opcode.Opcodes["setr"](0, 1, 2).Execute(r0));
        }

        [TestMethod]
        public void Seti()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(2, 3, 5, 6),
                Opcode.Opcodes["seti"](2, 6, 0).Execute(r0));
        }

        [TestMethod]
        public void Gtir()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(1, 3, 5, 6),
                Opcode.Opcodes["gtir"](6, 2, 0).Execute(r0));
            Assert.AreEqual(
                new Registers(0, 3, 5, 6),
                Opcode.Opcodes["gtir"](5, 2, 0).Execute(r0));
        }

        [TestMethod]
        public void Gtrr()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(1, 3, 5, 6),
                Opcode.Opcodes["gtrr"](3, 2, 0).Execute(r0));
            Assert.AreEqual(
                new Registers(0, 3, 5, 6),
                Opcode.Opcodes["gtrr"](1, 2, 0).Execute(r0));
        }

        [TestMethod]
        public void Gtri()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(1, 3, 5, 6),
                Opcode.Opcodes["gtri"](3, 5, 0).Execute(r0));
            Assert.AreEqual(
                new Registers(0, 3, 5, 6),
                Opcode.Opcodes["gtri"](2, 5, 0).Execute(r0));
        }

        [TestMethod]
        public void Eqir()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(1, 3, 5, 6),
                Opcode.Opcodes["eqir"](6, 3, 0).Execute(r0));
            Assert.AreEqual(
                new Registers(0, 3, 5, 6),
                Opcode.Opcodes["eqir"](5, 3, 0).Execute(r0));
        }

        [TestMethod]
        public void Eqrr()
        {
            var r0 = new Registers(0, 3, 5, 5);
            Assert.AreEqual(
                new Registers(1, 3, 5, 5),
                Opcode.Opcodes["eqrr"](3, 2, 0).Execute(r0));
            Assert.AreEqual(
                new Registers(0, 3, 5, 5),
                Opcode.Opcodes["eqrr"](3, 1, 0).Execute(r0));
        }

        [TestMethod]
        public void Eqri()
        {
            var r0 = new Registers(0, 3, 5, 6);
            Assert.AreEqual(
                new Registers(1, 3, 5, 6),
                Opcode.Opcodes["eqri"](2, 5, 0).Execute(r0));
            Assert.AreEqual(
                new Registers(0, 3, 5, 6),
                Opcode.Opcodes["eqri"](3, 5, 0).Execute(r0));
        }

        [TestMethod]
        public void NumberOfOpcodes()
        {
            Assert.AreEqual(16, Opcode.Opcodes.Count);
        }

        [TestMethod]
        public void Sample1()
        {
            const string sampleText =
@"Before: [3, 2, 1, 1]
9 2 1 2
After:  [3, 2, 2, 1]";

            Program.Sample s = Parsers.ProcessLine(Program.pSample, sampleText);
            var opcodes = Program.PossibleOpcodes(s);
            Assert.IsTrue(opcodes.Contains("mulr"), "mulr");
            Assert.IsTrue(opcodes.Contains("addi"), "addi");
            Assert.IsTrue(opcodes.Contains("seti"), "seti");
            Assert.AreEqual(3, opcodes.Count);
        }
    }
}
