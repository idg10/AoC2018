namespace Day16.Instructions
{
    public class BanI : Opcode
    {
        public BanI(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] & InputB);
    }
}
