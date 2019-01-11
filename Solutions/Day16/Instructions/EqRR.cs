namespace Day16.Instructions
{
    public class EqRR : Opcode
    {
        public EqRR(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] == registersIn[InputB] ? 1 : 0);
    }
}
