using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Common;
using LanguageExt;
using LanguageExt.Parsec;

using static Common.Parsers;
using static LanguageExt.Parsec.Char;
using static LanguageExt.Parsec.Prim;

namespace Day17
{
    public static class Program
    {
        public static readonly Parser<GroundRange> pRange =
            from posDimension in either(ch('x'), ch('y'))
            let isVertical = posDimension == 'x'
            from eq1 in ch('=')
            from pos in pInt32
            from c in str(", ")
            from rangeDimension in ch(isVertical ? 'y' : 'x')
            from eq2 in ch('=')
            from range in Sep2By(pInt32, str(".."))
            select new GroundRange(isVertical, pos, range.Item1, range.Item2);

        private static void Main()
        {
            var ranges = InputReader.ParseLines(typeof(Program), pRange).ToArray();
        }
    }
}
