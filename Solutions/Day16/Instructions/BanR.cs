namespace Day16.Instructions
{
    public class BanR : Opcode
    {
        public BanR(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] & registersIn[InputB]);
    }
}
