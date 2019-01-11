namespace Day16.Instructions
{
    public class SetR : Opcode
    {
        public SetR(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA]);
    }
}
