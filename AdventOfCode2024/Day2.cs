namespace AdventOfCode2024
{
    internal class Day2
    {
        private enum LevelState 
        {
            UNKNOWN,
            INCREASING,
            DECREASING,
        }

        public static void Part1()
        {
            RunForErrorsAllowed(0);
        }

        public static void Part2()
        {
            RunForErrorsAllowed(1);
        }

        private static void RunForErrorsAllowed(int allowedErrors)
        {
            var safeReports = GetLines()
                .Select(x => x.Trim())
                .Where(x => x != string.Empty)
                .Select(x => x.Split(' ').Select(y => int.Parse(y)).ToList())
                .Where(x => IsSafe(x, allowedErrors))
                .ToList();

            Console.WriteLine(safeReports.Count);
        }

        private static bool IsSafe(List<int> levels, int allowedErrors)
        {
            var s = LevelState.UNKNOWN;
            for (var i = 1; i < levels.Count; i++) 
            {
                var diff = levels[i - 1] - levels[i];
                if (diff == 0 || Math.Abs(diff) > 3)
                {
                    if (allowedErrors <= 0) return false;
                    else return SafeAfterRemoveOne(levels, allowedErrors - 1);
                }
                if (s == LevelState.UNKNOWN) s = StateFromDiff(diff);
                else if (s != StateFromDiff(diff))
                {
                    if (allowedErrors <= 0) return false;
                    else return SafeAfterRemoveOne(levels, allowedErrors - 1);
                }
            }
            return true;
        }

        private static bool SafeAfterRemoveOne(List<int> levels, int allowedErrors)
        {
            for (var i = 0; i < levels.Count; i++)
            {
                var copy = new List<int>(levels);
                copy.RemoveAt(i);
                if (IsSafe(copy, allowedErrors)) return true;
            }
            return false;
        }

        private static LevelState StateFromDiff(int diff) => diff > 0 ? LevelState.DECREASING : LevelState.INCREASING;

        private static string[] GetLines()
        {
            return File.ReadAllLines("day2_1.txt");
        }
    }
}
