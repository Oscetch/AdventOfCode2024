namespace AdventOfCode2024
{
    internal static class Day7Dfs
    {
        public static void Part2()
        {
            var sum = 0L;
            foreach (var line in ParseInput())
            {
                var lineSplit = line.Split(':');
                var testValue = long.Parse(lineSplit[0]);
                var equationValues = lineSplit[1].Split(' ').Where(x => x != "").Select(long.Parse).ToList();
                sum += Dfs(equationValues[0], testValue, equationValues, 1);
            }
            Console.WriteLine(sum);
        }

        private static long Dfs(long sum, long testValue, List<long> equationValues, int index)
        {
            if (sum > testValue) return 0;
            if (index == equationValues.Count) return sum == testValue ? sum : 0;
            var n = equationValues[index];
            if (Dfs(Concat(sum, n), testValue, equationValues, index + 1) == testValue)
            {
                return testValue;
            }
            if (Dfs(sum * n, testValue, equationValues, index + 1) == testValue)
            {
                return testValue;
            }
            if (Dfs(sum + n, testValue, equationValues, index + 1) == testValue)
            {
                return testValue;
            }
            return 0;
        }
        private static long Concat(long left, long right)
        {
            var digitsRight = (int)Math.Log10(right) + 1;
            return left * (long)Math.Pow(10L, digitsRight) + right;
        }


        private static string[] ParseInput() => File.ReadAllLines("day7_part1.txt");
    }
}
