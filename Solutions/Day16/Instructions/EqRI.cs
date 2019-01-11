namespace Day16.Instructions
{
    public class EqRI : Opcode
    {
        public EqRI(Arguments arguments) : base(arguments)
        {
        }

        public override Registers Execute(Registers registersIn)
            => registersIn.SetWithValue(OutputRegisterC, registersIn[InputA] == InputB ? 1 : 0);
    }
}
