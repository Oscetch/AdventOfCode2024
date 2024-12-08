namespace AdventOfCode2024
{
    internal static class Day5
    {
        struct Rule
        {
            public int X;
            public int Y;

            public readonly bool IsLineValid(List<int> line)
            {
                var indexX = line.IndexOf(X);
                var indexY = line.IndexOf(Y);
                return indexX == -1 || indexY == -1 || indexY > indexX;
            }
        }

        public static void Part1()
        {
            var (rules, updates) = ParseInstructions();
            var result = updates
                .Where(x => rules.All(r => r.IsLineValid(x)))
                .Select(x => x[x.Count / 2])
                .Sum();

            Console.WriteLine(result);
        }

        public static void Part2()
        {
            var (rules, updates) = ParseInstructions();
            var result = updates
                .Where(x => !rules.All(r => r.IsLineValid(x)))
                .Select(invalidResult =>
            {
                var relevantRules = rules.Where(x => invalidResult.Contains(x.X) && invalidResult.Contains(x.Y));
                var leftRules = relevantRules.GroupBy(x => x.X).ToDictionary(x => x.Key, x => x.Count());
                var rightRules = relevantRules.GroupBy(x => x.Y).ToDictionary(x => x.Key, x => x.Count());
                return invalidResult.OrderBy(x => Weight(leftRules, rightRules, x)).ToList();
            })
                .Select(x => x[x.Count / 2])
                .Sum();

            Console.WriteLine(result);
        }

        private static int Weight(Dictionary<int, int> left, Dictionary<int, int> right, int value)
        {
            var leftWeight = left.TryGetValue(value, out var leftCount) ? leftCount : 0;
            var rightWeight = right.TryGetValue(value, out var rightCount) ? rightCount : 0;
            return rightWeight - leftWeight;
        }

        private static (List<Rule> rules, List<List<int>> updates) ParseInstructions()
        {
            var lines = File.ReadAllLines("day5_part1.txt");
            Rule rule;
            var rules = new List<Rule>();
            var updates = new List<List<int>>();

            foreach (var line in lines)
            {
                if (line.Contains('|'))
                {
                    var n = line.Split('|');
                    rule.X = int.Parse(n[0]);
                    rule.Y = int.Parse(n[1]);
                    rules.Add(rule);
                }
                else if (line.Contains(','))
                {
                    updates.Add(line.Split(',').Select(int.Parse).ToList());
                }
            }
            return (rules, updates);
        }
    }
}
