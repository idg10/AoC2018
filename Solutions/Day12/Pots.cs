using System.Numerics;

namespace Day12
{
    [System.Diagnostics.DebuggerDisplay("{PotText} {LeftIndex}")]
    public class Pots
    {
        public Pots(
            string potText,
            in BigInteger leftIndex)
        {
            PotText = potText;
            LeftIndex = leftIndex;
        }

        public string PotText { get; }
        public BigInteger LeftIndex { get; }
    }
}
