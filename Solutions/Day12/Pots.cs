using System;
using System.Collections.Generic;
using System.Text;

namespace Day12
{
    [System.Diagnostics.DebuggerDisplay("{PotText} {LeftIndex}")]
    public class Pots
    {
        public Pots(
            string potText,
            int leftIndex)
        {
            PotText = potText;
            LeftIndex = leftIndex;
        }

        public string PotText { get; }
        public int LeftIndex { get; }
    }
}
