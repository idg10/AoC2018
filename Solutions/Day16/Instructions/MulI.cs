namespace Day16.Instructions
{
    public class MulI : Opcode
    {
        public MulI(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] * InputB);
    }
}
