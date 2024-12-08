namespace AdventOfCode2024
{
    internal static class Day7
    {
        public static void Part1()
        {
            var sum = 0L;
            foreach (var line in ParseInput())
            {
                var lineSplit = line.Split(':');
                var testValue = long.Parse(lineSplit[0]);
                var equationValues = lineSplit[1].Split(' ').Where(x => x != "").Select(long.Parse).ToList();
                if (CanEqual(equationValues, testValue, false))
                {
                    sum += testValue;
                }
            }
            Console.WriteLine(sum);
        }

        public static void Part2()
        {
            var sum = 0L;
            foreach (var line in ParseInput())
            {
                var lineSplit = line.Split(':');
                var testValue = long.Parse(lineSplit[0]);
                var equationValues = lineSplit[1].Split(' ').Where(x => x != "").Select(long.Parse).ToList();
                if (CanEqual(equationValues, testValue, true))
                {
                    sum += testValue;
                }
            }
            Console.WriteLine(sum);
        }

        private static bool CanEqual(List<long> values, long testValue, bool useConcat)
        {
            var combinationRange = values.Count - 1;
            var fullRange = SetBitsInRange(0, combinationRange);
            var validBitPositions = fullRange;
            if (useConcat)
            {
                fullRange |= SetBitsInRange(combinationRange, combinationRange * 2);
            }
            for (var n = 0; n <= fullRange; n++)
            {
                if ((n == 0 || (n & validBitPositions) > 0) && TestValue(values, testValue, n))
                {
                    return true;
                }
            }
            return false;
        }

        private static int SetBitsInRange(int startIndex, int endIndex) =>
            ((1 << startIndex) - 1) ^ ((1 << endIndex) - 1);

        private static bool TestValue(List<long> values, long testValue, int operatorsMask)
        {
            var sum = values[0];
            for (var i = 0; i < values.Count - 1; i++)
            {
                var value = values[i + 1];
                var multiplyMask = 1 << i;
                var concatMask = multiplyMask | (1 << (values.Count - 1 + i));
                if ((concatMask & operatorsMask) == concatMask)
                {
                    sum = Concat(sum, value);
                }
                else if ((multiplyMask & operatorsMask) > 0)
                {
                    sum *= value;
                }
                else
                {
                    sum += value;
                }
                if (sum > testValue)
                {
                    return false;
                }
            }
            return sum == testValue;
        }

        private static long Concat(long left, long right)
        {
            var digitsRight = (int)Math.Log10(right) + 1;
            return left * (long)Math.Pow(10L, digitsRight) + right;
        }

        private static string[] ParseInput() => File.ReadAllLines("day7_part1.txt");
    }
}
