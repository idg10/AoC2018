using System;
using System.Collections.Generic;
using System.Text;

namespace Day16.Instructions
{
    public class MulR : Opcode
    {
        public MulR(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] * registersIn[InputB]);
    }
}
