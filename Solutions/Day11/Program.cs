using System;

namespace Day11
{
    public static class Program
    {
        public const int GridWidth = 300;
        public const int GridHeight = 300;

        private static void Main()
        {
            (int x1, int y1) = SolvePart1(5093);
            Console.WriteLine($"Part 1: {x1},{y1}");

            (int x2, int y2, int size2) = SolvePart2(5093);
            Console.WriteLine($"Part 2: {x2},{y2},{size2}");
        }

        public static (int x, int y) SolvePart1(int serialNumber)
        {
            int[,] windowSummedGrid = GenerateSummedGrid(3, 3, serialNumber);
            return FindMax(windowSummedGrid);
        }

        public static (int x, int y, int size) SolvePart2(int serialNumber)
        {
            int max = int.MinValue;
            int mx = 0;
            int my = 0;
            int ms = 0;
            for (int size = 1; size <= 300; ++size)
            {
                int[,] windowSummedGrid = GenerateSummedGrid(size, size, serialNumber);
                (int x, int y) = FindMax(windowSummedGrid);
                int thisMax = windowSummedGrid[x, y];
                if (thisMax > max)
                {
                    mx = x;
                    my = y;
                    ms = size;
                    max = thisMax;
                }
            }

            return (mx, my, ms);
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

        public static int[] GenerateSummedRow(int windowSize, int y, int serialNumber)
        {
            var result = new int[GridWidth - windowSize + 1];
            var window = new int[windowSize];
            int total = 0;
            for (int x = 0; x < GridWidth; ++x)
            {
                int windowPos = x % windowSize;
                total -= window[windowPos];
                int value = CalculatePower(x, y, serialNumber);
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

        public static int[,] GenerateSummedGrid(int windowWidth, int windowHeight, int serialNumber)
        {
            int outputWidth = GridWidth - windowWidth + 1;
            var result = new int[outputWidth, GridHeight - windowHeight + 1];
            var window = new int[windowHeight][];
            var totals = new int[outputWidth];

            for (int y = 0; y < GridHeight; ++y)
            {
                int windowPos = y % windowHeight;
                int[] oldRow = window[windowPos];
                int[] newRow = GenerateSummedRow(windowWidth, y, serialNumber);
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

        public static (int x, int y) FindMax(int[,] values)
        {
            int max = int.MinValue;
            int mx = 0;
            int my = 0;
            int w = values.GetLength(0);
            int h = values.GetLength(1);
            for (int x = 0; x < w; ++x)
            {
                for (int y = 0; y < h; ++y)
                {
                    int v = values[x, y];
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
