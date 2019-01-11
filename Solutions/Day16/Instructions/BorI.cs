namespace Day16.Instructions
{
    public class BorI : Opcode
    {
        public BorI(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] | InputB);
    }
}
