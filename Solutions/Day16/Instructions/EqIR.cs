namespace Day16.Instructions
{
    public class EqIR : Opcode
    {
        public EqIR(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, InputA == registersIn[InputB] ? 1 : 0);
    }
}
