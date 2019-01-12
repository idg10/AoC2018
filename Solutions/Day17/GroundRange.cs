using System;
using System.Collections.Generic;
using System.Text;

namespace Day17
{
    public struct GroundRange
    {
        public GroundRange(
            bool isVertical,
            int position,
            int start,
            int end)
        {
            IsVertical = isVertical;
            Position = position;
            Start = start;
            End = end;
        }

        public bool IsVertical { get; }
        public int Position { get; }
        public int Start { get; }
        public int End { get; }

        public override string ToString() => $"{(IsVertical ? 'x' : 'y')}={Position}, {(IsVertical ? 'y' : 'x')}={Start}..{End}";
    }
}
