using System;

namespace Day11
{
    public static class Program
    {
        public const int GridWidth = 300;
        public const int GridHeight = 300;

        private static void Main()
        {
            (int x, int y) = SolvePart1(5093);
            Console.WriteLine($"Part 1: {x},{y}");
        }

        public static (int x, int y) SolvePart1(int serialNumber)
        {
            sbyte[,] windowSummedGrid = GenerateSummedGrid(3, 3, serialNumber);
            return FindMax(windowSummedGrid);
        }

        public static sbyte CalculatePower(int x, int y, int serialNumber)
        {
            int rackId = x + 10;
            int powerLevel = rackId * y;
            powerLevel += serialNumber;
            powerLevel *= rackId;

            int hundredths = (powerLevel / 100) % 10;

            return (sbyte) (hundredths - 5);
        }

        public static sbyte[] GenerateSummedRow(int windowSize, int y, int serialNumber)
        {
            var result = new sbyte[GridWidth - windowSize + 1];
            var window = new sbyte[windowSize];
            sbyte total = 0;
            for (int x = 0; x < GridWidth; ++x)
            {
                int windowPos = x % windowSize;
                total -= window[windowPos];
                sbyte value = CalculatePower(x, y, serialNumber);
                window[windowPos] = value;
                total += value;
                int resultIndex = x - windowSize + 1;
                if (resultIndex >= 0)
                {
                    result[resultIndex] = total;
                }
            }

            return result;
        }

        public static sbyte[,] GenerateSummedGrid(int windowWidth, int windowHeight, int serialNumber)
        {
            int outputWidth = GridWidth - windowWidth + 1;
            var result = new sbyte[outputWidth, GridHeight - windowHeight + 1];
            var window = new sbyte[windowHeight][];
            var totals = new sbyte[outputWidth];

            for (int y = 0; y < GridHeight; ++y)
            {
                int windowPos = y % windowHeight;
                sbyte[] oldRow = window[windowPos];
                sbyte[] newRow = GenerateSummedRow(windowWidth, y, serialNumber);
                for (int x = 0; x < outputWidth; ++x)
                {
                    totals[x] -= oldRow?[x] ?? 0;
                    totals[x] += newRow[x];
                    int resultY = y - windowHeight + 1;
                    if (resultY >= 0)
                    {
                        result[x, resultY] = totals[x];
                    }

                    window[windowPos] = newRow;
                }
            }
            return result;
        }

        public static (int x, int y) FindMax(sbyte[,] values)
        {
            int max = sbyte.MinValue;
            int mx = 0;
            int my = 0;
            int w = values.GetLength(0);
            int h = values.GetLength(1);
            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    sbyte v = values[x, y];
                    if (v > max)
                    {
                        mx = x;
                        my = y;
                        max = v;
                    }
                }
            }

            return (mx, my);
        }
    }
}
