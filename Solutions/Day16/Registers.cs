using System;
using System.Collections.Generic;
using System.Text;

namespace Day16
{
    public struct Registers
    {
        public Registers(int r0, int r1, int r2, int r3)
        {
            R0 = r0;
            R1 = r1;
            R2 = r2;
            R3 = r3;
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return R0;
                    case 1: return R1;
                    case 2: return R2;
                    case 3: return R3;
                    default: throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        public Registers SetWithValue(int index, int value)
        {
            switch (index)
            {
                case 0: return new Registers(value, R1, R2, R3);
                case 1: return new Registers(R0, value, R2, R3);
                case 2: return new Registers(R0, R1, value, R3);
                case 3: return new Registers(R0, R1, R2, value);
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public Registers SetWithRegister(int index, int register) => SetWithValue(index, this[register]);

        public override bool Equals(object obj) =>
            obj is Registers r &&
            r.R0 == R0 &&
            r.R1 == R1 &&
            r.R2 == R2 &&
            r.R3 == R3;

        public override int GetHashCode() => (R0, R1, R2, R3).GetHashCode();

        public override string ToString() => (R0, R1, R2, R3).ToString();

        public static bool operator ==(Registers l, Registers r) => l.Equals(r);
        public static bool operator !=(Registers l, Registers r) => !l.Equals(r);

        public int R0 { get; }
        public int R1 { get; }
        public int R2 { get; }
        public int R3 { get; }
    }
}
