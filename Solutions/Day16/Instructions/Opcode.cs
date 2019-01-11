using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Day16.Instructions
{
    public abstract class Opcode
    {
        static Opcode()
        {
            Opcodes = typeof(Opcode).Assembly
                .GetTypes()
                .Where(t => t != typeof(Opcode) && typeof(Opcode).IsAssignableFrom(t))
                .ToDictionary(
                    t => t.Name.ToLowerInvariant(),
                    t =>
                    {
                        ConstructorInfo ctor = t.GetConstructor(new[] { typeof(Arguments) });
                        return (Func<byte, byte, byte, Opcode>)((a, b, c) =>
                            (Opcode) ctor.Invoke(new object[] { new Arguments(a, b, c) }));
                    });
        }

        protected Opcode(Arguments arguments)
        {
            this.OpArguments = arguments;
        }

        public static IReadOnlyDictionary<string, Func<byte, byte, byte, Opcode>> Opcodes { get; }

        public Arguments OpArguments { get; }

        protected byte InputA => OpArguments.InputA;
        protected byte InputB => OpArguments.InputB;
        protected byte OutputRegisterC => OpArguments.OutputRegisterC;

        public abstract Registers Execute(Registers registersIn);

        public struct Arguments
        {
            public Arguments(
                byte inputA,
                byte inputB,
                byte outputRegisterC)
            {
                InputA = inputA;
                InputB = inputB;
                OutputRegisterC = outputRegisterC;
            }

            public byte InputA { get; }
            public byte InputB { get; }
            public byte OutputRegisterC { get; }
        }
    }
}
