namespace AdventOfCode2024
{
    internal static class Day8
    {
        public static void Part1()
        {
            var map = ParseInput();
            var mapYLength = map.Length;
            var mapXLength = map[0].Length;
            var antiNodes = 0;

            IteratePairs(map, (posA, posB, group) =>
            {
                var (x, y) = AntiNodePositionOf(posA, posB);

                if (ContainsPoint(mapXLength, mapYLength, (x, y)))
                {
                    var antiNodeCurrent = map[y][x];
                    if (antiNodeCurrent != group && antiNodeCurrent != '#')
                    {
                        map[y][x] = '#';
                        antiNodes++;
                    }
                }
            });

            Console.WriteLine(antiNodes);
        }

        public static void Part2()
        {
            var map = ParseInput();
            var mapYLength = map.Length;
            var mapXLength = map[0].Length;

            var antiNodes = new HashSet<(int x, int y)>();
            IteratePairs(map, (posA, posB, _) =>
            {
                antiNodes.Add(posA);
                antiNodes.Add(posB);

                var from = posB;
                var antiNode = AntiNodePositionOf(posA, posB);

                while (ContainsPoint(mapXLength, mapYLength, antiNode))
                {
                    if (map[antiNode.y][antiNode.x] == '.')
                    {
                        map[antiNode.y][antiNode.x] = '#';
                    }

                    antiNodes.Add(antiNode);
                    var lastAntiNode = antiNode;
                    antiNode = AntiNodePositionOf(from, antiNode);
                    from = lastAntiNode;
                }
            });

            Console.WriteLine(antiNodes.Count);
        }

        private static void IteratePairs(char[][] map, Action<(int x, int y), (int x, int y), char> onPair)
        {
            var groups = GroupMap(map);
            foreach (var group in groups.Keys)
            {
                var positions = groups[group];
                if (positions.Count < 2) continue;
                for (var a = 0; a < positions.Count; a++)
                {
                    for (var b = 0; b < positions.Count; b++)
                    {
                        if (a == b) continue;
                        var posA = positions[a];
                        var posB = positions[b];
                        onPair(posA, posB, group);
                    }
                }
            }
        }

        private static bool ContainsPoint(int maxX, int maxY, (int x, int y) pos) =>
            pos.x >= 0 && pos.y >= 0 && pos.x < maxX && pos.y < maxY;

        private static (int x, int y) AntiNodePositionOf((int x, int y) a, (int x, int y) b)
        {
            var dY = b.y - a.y;
            var dX = b.x - a.x;

            var angle = Math.Atan2(dY, dX);

            var distanceX = a.x - b.x;
            var distanceY = a.y - b.y;
            var distance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

            var antiNodeX = (int)Math.Round(b.x + (distance * Math.Cos(angle)));
            var antiNodeY = (int)Math.Round(b.y + (distance * Math.Sin(angle)));
            return (antiNodeX, antiNodeY);
        }

        private static Dictionary<char, List<(int x, int y)>> GroupMap(char[][] map)
        {
            var groups = new Dictionary<char, List<(int x, int y)>>();
            for (var y = 0; y < map.Length; y++)
            {
                var line = map[y];
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    if (c == '.') continue;
                    if (groups.TryGetValue(c, out var list))
                    {
                        list.Add((x, y));
                    }
                    else
                    {
                        groups.Add(c, [(x, y)]);
                    }
                }
            }
            return groups;
        }

        private static char[][] ParseInput()
        {
            var lines = File.ReadAllLines("day8_part1.txt");
            char[][] map = new char[lines.Length][];
            for (var i = 0; i < lines.Length; i++) 
            {
                map[i] = lines[i].ToCharArray();
            }
            return map;
        }
    }
}
