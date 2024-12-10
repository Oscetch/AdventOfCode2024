namespace AdventOfCode2024
{
    internal static class Day10
    {
        public static void Part1() => Run(false);
        public static void Part2() => Run(true);

        private static void Run(bool isPart2)
        {
            var map = ParseInput();
            var starts = new List<(int x, int y)>();
            for (var y = 0; y < map.Length; y++)
            {
                var line = map[y];
                for (var x = 0; x < line.Length; x++)
                {
                    if (line[x] == '0') starts.Add((x, y));
                }
            }

            var sum = starts.Sum(x => FindFrom(map, x.x, x.y, isPart2 ? null : []));
            Console.WriteLine();
            Console.WriteLine(sum);
        }


        private static int FindFrom(char[][] map, int x, int y, HashSet<(int x, int y)>? foundFromPos, int last = -1)
        {
            if (!IsInMap(map, x, y)) return 0;

            var current = int.Parse(map[y][x].ToString());
            if (current != last + 1) return 0;
            if (current == 9) return (foundFromPos?.Add((x, y)) ?? true) ? 1 : 0;

            return FindFrom(map, x - 1, y, foundFromPos, current) 
                + FindFrom(map, x + 1, y, foundFromPos, current) 
                + FindFrom(map, x, y - 1, foundFromPos, current) 
                + FindFrom(map, x, y + 1, foundFromPos, current);
        }

        private static bool IsInMap(char[][] map, int x, int y) =>
            x >= 0 && y >= 0 && y < map.Length && x < map[0].Length;


        private static char[][] ParseInput()
        {
            var lines = File.ReadAllLines("day10_part1.txt");
            var map = new char[lines.Length][];
            for (var y = 0; y < lines.Length; y++) map[y] = lines[y].ToCharArray();
            return map;
        }
    }
}
