namespace AdventOfCode2024
{
    internal static class Day15
    {
        public static void Part1()
        {
            var (map, inputs) = ParseInput();
            var spX = -1;
            var spY = -1;
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '@')
                    {
                        map[y][x] = '.';
                        spX = x; spY = y; break;
                    }
                }
                if (spX != -1) break;
            }

            foreach (var c in inputs)
            {
                var (movementX, movementY) = Movement(c);
                var nextX = spX + movementX;
                var nextY = spY + movementY;
                var nextChar = map[nextY][nextX];
                if (nextChar == '#') continue;
                if (nextChar == '.')
                {
                    spX = nextX; spY = nextY;
                    continue;
                }
                if (nextChar != 'O')
                {
                    throw new Exception("Unexpected char");
                }
                if (TryMove(map, nextX, nextY, movementX, movementY))
                {
                    spX = nextX;
                    spY = nextY;
                }
            }

            PrintMap(map, spX, spY);
            Console.ReadLine();
            Console.Clear();
            var sum = 0;
            for (var y = 0; y < map.Length; y++)
            {
                var yM = map[y];
                for (var x = 0; x < yM.Length; x++)
                {
                    if (yM[x] == 'O')
                    {
                        sum += 100 * y + x;
                    }
                }
            }
            Console.WriteLine(sum);
        }

        public static void Part2()
        {
            var (oldMap, inputs) = ParseInput();
            var map = ExpandMap(oldMap);
            var spX = -1;
            var spY = -1;
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '@')
                    {
                        map[y][x] = '.';
                        spX = x; spY = y; break;
                    }
                }
                if (spX != -1) break;
            }

            foreach (var c in inputs)
            {
                var (movementX, movementY) = Movement(c);
                var nextX = spX + movementX;
                var nextY = spY + movementY;
                var nextChar = map[nextY][nextX];
                if (nextChar == '#') continue;
                if (nextChar == '.')
                {
                    spX = nextX; spY = nextY;
                    continue;
                }
                if (nextChar != '[' && nextChar != ']')
                {
                    throw new Exception("Unexpected char");
                }
                if (TryMove2(map, nextX, nextY, movementX, movementY))
                {
                    spX = nextX;
                    spY = nextY;
                }
            }


            PrintMap(map, spX, spY);
            Console.ReadLine();

            Console.Clear();
            var sum = 0;
            for (var y = 0; y < map.Length; y++)
            {
                var yM = map[y];
                for (var x = 0; x < yM.Length; x++)
                {
                    if (yM[x] == '[')
                    {
                        sum += 100 * y + x;
                    }
                }
            }
            Console.WriteLine(sum);
        }

        private static char[][] ExpandMap(char[][] map)
        {
            var newMap = new char[map.Length][];
            for (var y = 0; y < map.Length; y++)
            {
                var mY = map[y];
                var newX = new List<char>();
                for (var x = 0; x < mY.Length; x++)
                {
                    var c = mY[x];
                    if (c == '@') 
                    {
                        newX.Add(c);
                        newX.Add('.');
                        continue;
                    }
                    if (c == 'O')
                    {
                        newX.Add('[');
                        newX.Add(']');
                        continue;
                    }

                    newX.Add(c);
                    newX.Add(c);
                }
                newMap[y] = newX.ToArray();
            }
            return newMap;
        }

        private static void PrintMap(char[][] map, int px, int py)
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            for (var y = 0; y < map.Length; y++) 
            {
                var yM = map[y];
                for (var x = 0; x < yM.Length; x++)
                {
                    Console.SetCursorPosition(x, y);
                    if (x == px && y == py)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('@');
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(map[y][x]);
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static bool TryMove2(char[][] map, int pX, int pY, int vX, int vY)
        {
            var nC = map[pY][pX];
            var positions = new List<(int x, int y)> 
            {
                (pX, pY),
                nC == ']' ? (pX - 1, pY) : (pX + 1, pY)
            };
            while (true)
            {
                var positionsToCheck = positions.Select(x => (x.x + vX, x.y + vY)).ToList();
                var addedPositions = false;
                foreach (var (x, y) in positionsToCheck)
                {
                    if (positions.Contains((x, y))) continue;
                    nC = map[y][x];
                    if (nC == '#') return false;
                    if (nC == '[' || nC == ']')
                    {
                        positions.Add((x, y));
                        positions.Add((nC == ']' ? x - 1 : x + 1, y));
                        addedPositions = true;
                    }
                }
                if (addedPositions) continue;
                if (positions.Count != positionsToCheck.Count) throw new Exception("?");

                var chars = positions.Select(x => map[x.y][x.x]).ToList();

                for (var i = 0; i < positions.Count; i++)
                {
                    var old = positions[i];
                    var neW = positionsToCheck[i];

                    map[neW.Item2][neW.Item1] = chars[i];
                    if (!positionsToCheck.Contains(old))
                    {
                        map[old.y][old.x] = '.';
                    }
                }
                return true;
            }
        }

        private static bool TryMove(char[][] map, int pX, int pY, int vX, int vY)
        {
            var nX = pX;
            var nY = pY;
            while (true)
            {
                nX += vX;
                nY += vY;
                var nC = map[nY][nX];
                if (nC == '#') return false;
                if (nC == 'O') continue;
                if (nC != '.') throw new Exception("Unknown char in move");
                while (nX != pX || nY != pY)
                {
                    map[nY][nX] = 'O';
                    nX -= vX;
                    nY -= vY;
                }
                map[pY][pX] = '.';
                return true;
            }
        }

        private static (int x, int y) Movement(char c)
        {
            return c switch
            {
                '<' => (-1, 0),
                '>' => (1, 0),
                '^' => (0, -1),
                'v' => (0, 1),
                _ => throw new Exception("?"),
            };
        }

        private static (char[][] map, string inputs) ParseInput()
        {
            var lines = File.ReadAllText("day15_part1.txt");
            var split = lines.Split("\r\n\r\n");
            var inputs = string.Join("", split[1].Split("\r\n").Where(x => !string.IsNullOrEmpty(x)));
            var map = split[0].Split("\r\n").Where(x => !string.IsNullOrEmpty(x)).ToList();
            char[][] cMap = new char[map.Count][];
            for (var i = 0; i < map.Count; i++) 
            {
                cMap[i] = map[i].ToCharArray();
            }
            return (cMap, inputs);
        }
    }
}
