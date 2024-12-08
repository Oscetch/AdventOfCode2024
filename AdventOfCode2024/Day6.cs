namespace AdventOfCode2024
{
    internal static class Day6
    {
        public static void Part1()
        {
            var map = ParseInput();
            var positions = new HashSet<(int x, int y)>();
            var (positionX, positionY, _) = IterateMap(map).FirstOrDefault(x => x.c == '^');
            map[positionY][positionX] = '.';
            var (velocityX, velocityY) = (0, -1);
            positions.Add((positionX, positionY));

            while (true)
            {
                var (nextPosX, nextPosY) = (positionX + velocityX, positionY + velocityY);
                if (!InMap(map, nextPosX, nextPosY)) break;
                if (map[nextPosY][nextPosX] == '#')
                {
                    (velocityX, velocityY) = (-velocityY, velocityX);
                    continue;
                }
                (positionX, positionY) = (nextPosX, nextPosY);
                positions.Add((positionX, positionY));
            }

            Console.WriteLine(positions.Count);
        }

        public static void Part2()
        {
            var map = ParseInput();
            var positions = new HashSet<(int x, int y)>();
            var velocityPositions = new List<(int x, int y, int vx, int vy)>();
            var (positionX, positionY, _) = IterateMap(map).FirstOrDefault(x => x.c == '^');
            var (sX, sY) = (positionX, positionY);
            map[positionY][positionX] = '.';
            var (velocityX, velocityY) = (0, -1);
            velocityPositions.Add((positionX, positionY, velocityX, velocityY));

            while (true)
            {
                var (nextPosX, nextPosY) = (positionX + velocityX, positionY + velocityY);
                if (!InMap(map, nextPosX, nextPosY)) break;
                if (map[nextPosY][nextPosX] == '#')
                {
                    (velocityX, velocityY) = (-velocityY, velocityX);
                    continue;
                }
                (positionX, positionY) = (nextPosX, nextPosY);
                velocityPositions.Add((positionX, positionY, velocityX, velocityY));
                positions.Add((positionX, positionY));
            }

            var possible = 0;
            foreach (var (x, y) in positions)
            {
                if (x == sX && y == sY) continue;
                map[y][x] = '#';
                if (TestPositionWithObstacleInfront(map, sX, sY))
                {
                    possible++;
                }
                map[y][x] = '.';
            }

            Console.WriteLine(possible);
        }

        private static bool TestPositionWithObstacleInfront(char[][] map, int x, int y)
        {
            var velocityPositions = new HashSet<(int x, int y, int vx, int vy)> 
            {
                (x, y, 0, -1)
            };

            var (positionX, positionY) = (x, y);
            var (velocityX, velocityY) = (0, -1);
            
            while (true)
            {
                var(nextPosX, nextPosY) = (positionX + velocityX, positionY + velocityY);
                if (!InMap(map, nextPosX, nextPosY)) break;
                if (map[nextPosY][nextPosX] == '#')
                {
                    (velocityX, velocityY) = (-velocityY, velocityX);
                    continue;
                }
                (positionX, positionY) = (nextPosX, nextPosY);

                if (velocityPositions.Contains((positionX, positionY, velocityX, velocityY)))
                {
                    return true;
                }
                velocityPositions.Add((positionX, positionY, velocityX, velocityY));
            }

            return false;
        }


        private static bool InMap(char[][] map, int x, int y)
        {
            var maxX = map[0].Length;
            var maxY = map.Length;
            return x >= 0 && x < maxX && y >= 0 && y < maxY;
        }

        private static IEnumerable<(int x, int y, char c)> IterateMap(char[][] map)
        {
            for (var y = 0; y < map.Length; y++) 
            {
                var line = map[y];
                for (var x = 0; x < map.Length; x++)
                {
                    yield return (x, y, line[x]);
                }
            }
        }

        private static char[][] ParseInput()
        {
            var lines = File.ReadAllLines("day6_part1.txt");
            char[][] map = new char[lines.Length][];
            for (var y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                map[y] = line.ToCharArray();
            }
            return map;
        }
    }
}
