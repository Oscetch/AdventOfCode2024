using System.ComponentModel;

namespace AdventOfCode2024
{
    internal static class Day12
    {
        private static List<(int x, int y)> _directions = [(1, 0), (-1, 0), (0, 1), (0, -1)];

        public static void Part1()
        {
            var map = ParseInput();
            var sum = 0;
            var islands = new List<HashSet<(int x, int y)>>();
            for (var y = 0; y < map.Length; y++) 
            {
                var row = map[y];
                for (var x = 0; x < row.Length; x++)
                {
                    if (islands.Any(i => i.Contains((x, y)))) continue;
                    var island = new HashSet<(int x, int y)>();
                    FloodFill(x, y, map[y][x], map, island);
                    islands.Add(island);
                }
            }
            foreach (var island in islands) 
            {
                var fp = island.First();
                var c = map[fp.y][fp.x];
                var islandSum = 0;
                foreach (var position in island)
                {
                    foreach (var direction in _directions)
                    {
                        var nx = position.x + direction.x;
                        var ny = position.y + direction.y;

                        if (nx < 0 || ny < 0 || nx >= map[0].Length || ny >= map.Length) islandSum++;
                        else if (map[ny][nx] != c) islandSum++;
                    }
                }
                sum += (islandSum * island.Count);
            }

            Console.WriteLine(sum);
        }

        public static void Part2()
        {
            var map = ParseInput();
            var sum = 0;
            var islands = new List<HashSet<(int x, int y)>>();
            for (var y = 0; y < map.Length; y++)
            {
                var row = map[y];
                for (var x = 0; x < row.Length; x++)
                {
                    if (islands.Any(i => i.Contains((x, y)))) continue;
                    var island = new HashSet<(int x, int y)>();
                    FloodFill(x, y, map[y][x], map, island);
                    islands.Add(island);
                }
            }
            foreach (var island in islands)
            {
                var fp = island.First();
                var c = map[fp.y][fp.x];
                var horizontal = new Dictionary<(int n, int m), List<int>>();
                var vertical = new Dictionary<(int n, int m), List<int>>();
                foreach (var position in island) 
                {
                    for (var x = -1; x < 2; x += 2)
                    {
                        var nx = position.x + x;
                        if (nx < 0 || nx >= map[0].Length || map[position.y][nx] != c)
                        {
                            if (!horizontal.TryAdd((nx, position.x), [position.y]))
                            {
                                horizontal[(nx, position.x)].Add(position.y);
                            }
                        }
                    }
                    for (var y = -1; y < 2; y += 2)
                    {
                        var ny = position.y + y;
                        if (ny < 0 || ny >= map.Length || map[ny][position.x] != c)
                        {
                            if (!vertical.TryAdd((ny, position.y), [position.x]))
                            {
                                vertical[(ny, position.y)].Add(position.x);
                            }
                        }
                    }
                }
                var lines = 0;
                foreach (var key in horizontal.Keys)
                {
                    var ordered = horizontal[key].Order().ToList();
                    var splits = 1;
                    for (var i = 1; i < ordered.Count; i++)
                    {
                        if (ordered[i] - ordered[i - 1] > 1) splits++;
                    }
                    lines += splits;
                }
                foreach (var key in vertical.Keys)
                {
                    var ordered = vertical[key].Order().ToList();
                    var splits = 1;
                    for (var i = 1; i < ordered.Count; i++)
                    {
                        if (ordered[i] - ordered[i - 1] > 1) splits++;
                    }
                    lines += splits;
                }

                sum += island.Count * lines;
            }

            Console.WriteLine(sum);
        }

        private static void FloodFill(int x, int y, char target, char[][] map, HashSet<(int x, int y)> found)
        {
            if (x < 0 || y < 0 || x >= map[0].Length || y >= map.Length) return;
            var current = map[y][x];
            if (current != target || !found.Add((x, y))) return;

            foreach (var direction in _directions)
            {
                FloodFill(x + direction.x, y + direction.y, current, map, found);
            }
        }

        private static char[][] ParseInput()
        {
            var lines = /*@"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE".Split("\r\n") //*/File.ReadAllLines("day12_part1.txt")
                .Select(x => x.ToCharArray())
                .ToArray();
            return lines;
        }
    }
}
