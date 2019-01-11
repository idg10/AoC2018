namespace Day16.Instructions
{
    public class GtRR : Opcode
    {
        public GtRR(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] > registersIn[InputB] ? 1 : 0);
    }
}
