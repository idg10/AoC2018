namespace Day16.Instructions
{
    public class BorR : Opcode
    {
        public BorR(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] | registersIn[InputB]);
    }
}
