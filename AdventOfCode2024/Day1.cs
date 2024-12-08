namespace AdventOfCode2024
{
    internal class Day1
    {
        public static void Part1() 
        {
            var (left, right) = ReadInput();
            left.Sort();
            right.Sort();
            var score = left.Select((item, index) => Math.Abs(item - right[index])).Sum();
            Console.WriteLine(score);
        }

        public static void Part2() 
        {
            var (left, right) = ReadInput();
            var score = left.Select(item => item * right.Count(x => x == item)).Sum();
            Console.WriteLine(score);
        }

        private static (List<int> left, List<int> right) ReadInput() 
        {
            var input = File.ReadAllText("day1_1.txt").Trim();
            var inputSplit = input.Split('\n');
            var left = new List<int>();
            var right = new List<int>();
            foreach (var line in inputSplit)
            {
                var lineSplit = line.Trim().Split(' ').Where(x => x != "").ToArray();
                left.Add(int.Parse(lineSplit[0]));
                right.Add(int.Parse(lineSplit[1]));
            }
            return (left, right);
        }
    }
}
