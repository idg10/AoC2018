namespace Day16.Instructions
{
    public class AddR : Opcode
    {
        public AddR(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] + registersIn[InputB]);
    }
}
