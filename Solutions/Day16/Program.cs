using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Common;
using Day16.Instructions;
using LanguageExt;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Day16
{
    public static class Program
    {
        public static readonly Parser<Registers> pRegisters = between(
            ch('['),
            ch(']'),
            from rs in sepBy(Trim(pInt32, spaces), ch(','))
            where rs.Count == 4
            select new Registers(rs[0], rs[1], rs[2], rs[3]));

        private static Parser<Registers> LabelledRegisters(string label) =>
            from _ in Trim(str($"{label}:"))
            from r in pRegisters
            select r;

        public static readonly Parser<byte[]> pInstruction =
            from instruction in sepBy(pByte, spaces)
            where instruction.Count == 4
            select instruction.ToArray();

        public static readonly Parser<Sample> pSample =
            from before in LabelledRegisters("Before")
            from instruction in pInstruction
            from after in LabelledRegisters("After")
            select new Sample(before, instruction, after);

        public static readonly Parser<Sample[]> pSamples =
            from samples in sepEndBy(pSample, spaces)
            select samples.ToArray();

        private static void Main()
        {
            string[] input = InputReader.ReadAll(typeof(Program)).Replace("\r", "").Split("\n\n\n");
            Sample[] samples = ProcessLine(pSamples, input[0]);
            int part1 = SolvePart1(samples);
            Console.WriteLine("Part 1: " + part1);

            //, 
            byte[][] instructions = InputReader.ParseLines(
                input[1].Trim('\n').Split('\n', StringSplitOptions.RemoveEmptyEntries),
                pInstruction)
                .ToArray();
            int part2 = SolvePart2(samples, instructions);
            Console.WriteLine("Part 2: " + part2);
        }

        public static int SolvePart1(IEnumerable<Sample> samples)
        {
            int count = 0;
            foreach (var sample in samples)
            {
                var possibles = PossibleOpcodes(sample);
                if (possibles.Count >= 3)
                {
                    count += 1;
                }
            }

            return count;
        }

        public static int SolvePart2(
            IEnumerable<Sample> samples,
            IEnumerable<byte[]> instructions)
        {
            IReadOnlyDictionary<byte, string> opcodeMap = BuildOpcodeMap(samples);

            Registers r = default;

            foreach (byte[] instruction in instructions)
            {
                string operationName = opcodeMap[instruction[0]];
                Opcode op = Opcode.Opcodes[operationName](instruction[1], instruction[2], instruction[3]);
                r = op.Execute(r);
            }

            return r.R0;
        }

        public static IReadOnlyDictionary<byte, string> BuildOpcodeMap(IEnumerable<Sample> samples)
        {
            var opcodeCandidates = Enumerable
                .Range(0, 16)
                .ToDictionary(
                    i => (byte)i,
                    _ => ImmutableHashSet.CreateRange(Opcode.Opcodes.Keys));

            foreach (Sample sample in samples)
            {
                ISet<string> possibles = PossibleOpcodes(sample);
                byte opcode = sample.Opcode;
                var candidates = opcodeCandidates[opcode];
                opcodeCandidates[opcode] = candidates.Intersect(possibles);
            }

            bool changed;
            do
            {
                changed = false;
                var resolvedOpcodes = opcodeCandidates
                    .Where(kv => kv.Value.Count == 1)
                    .Select(kv => (kv.Key, kv.Value.Single()))
                    .ToList();
                foreach ((byte opcode, string opName) in resolvedOpcodes)
                {
                    // This opcode has only only candidate operation, meaning this has to
                    // be the opcode for that operation.
                    // We can therefore remove this operation from all other opcode candidate
                    // lists.
                    foreach ((var k, var candidates) in opcodeCandidates.Where(kv => kv.Key != opcode).ToList())
                    {
                        var candidatesWithoutResolvedOpcode = candidates.Remove(opName);
                        if (candidatesWithoutResolvedOpcode != candidates)
                        {
                            changed = true;
                            opcodeCandidates[k] = candidatesWithoutResolvedOpcode;
                        }
                    }
                }
            } while (changed);

            return opcodeCandidates
                .ToDictionary(
                    kv => kv.Key,
                    kv => kv.Value.Single());
        }

        public static ISet<string> PossibleOpcodes(Sample s)
        {
            var set = new System.Collections.Generic.HashSet<string>();

            foreach ((string name, var codeFactory) in Opcode.Opcodes)
            {
                Opcode opcode = codeFactory(s.A, s.B, s.C);
                Registers result = opcode.Execute(s.Before);
                if (result == s.After)
                {
                    set.Add(name);
                }
            }
            return set;
        }

        public class Sample
        {
            public Sample(
                Registers before,
                byte[] instruction,
                Registers after)
            {
                if (instruction.Length != 4)
                {
                    throw new ArgumentException("Instruction must contain 4 numbers", nameof(instruction));
                }

                Before = before;
                After = after;
                Opcode = instruction[0];
                A = instruction[1];
                B = instruction[2];
                C = instruction[3];
            }

            public Registers Before { get; }
            public byte Opcode { get; set; }
            public byte A { get; set; }
            public byte B { get; set; }
            public byte C { get; set; }
            public Registers After { get; set; }
        }
    }
}
