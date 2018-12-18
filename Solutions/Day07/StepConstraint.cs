namespace Day07
{
    public class StepConstraint
    {
        public StepConstraint(char prereq, char then)
        {
            Prereq = prereq;
            Then = then;
        }

        public char Prereq { get; }
        public char Then { get; }
    }
}
