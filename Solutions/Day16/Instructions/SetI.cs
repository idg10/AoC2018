namespace Day16.Instructions
{
    public class SetI : Opcode
    {
        public SetI(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, InputA);
    }
}
