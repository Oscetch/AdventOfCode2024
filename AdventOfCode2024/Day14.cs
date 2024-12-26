using System.Drawing;
using System.Drawing.Imaging;

namespace AdventOfCode2024
{
    internal static class Day14
    {
        public struct P
        {
            public int X;
            public int Y;

            public static P operator +(P x, P y) => new P
            {
                X = x.X + y.X,
                Y = x.Y + y.Y,
            };
            public static P operator *(P x, P y) => new P
            {
                X = x.X * y.X,
                Y = x.Y * y.Y,
            };
            public static P operator *(P x, int y) => new P
            {
                X = x.X * y,
                Y = x.Y * y,
            };
            public static P operator %(P x, P y) => new P 
            {
                X = Modulo(x.X, y.X),
                Y = Modulo(x.Y, y.Y),
            };

            public override string ToString()
            {
                return $"{X},{Y}";
            }
        }

        private static int Modulo(int a, int b)
        {
            return a - b * (int)Math.Floor((double)a / b);
        }

        struct R
        {
            public P Position;
            public P Size;

            public bool Contains(P position)
            {
                var xOk = position.X >= Position.X && position.X < Position.X + Size.X;
                var yOk = position.Y >= Position.Y && position.Y < Position.Y + Size.Y;
                return xOk && yOk;
            }

            public override string ToString()
            {
                return $"Position: {Position} | Size: {Size}";
            }
        }
         
        public static HashSet<P> Part1(int seconds = 100, bool print = true)
        {
            var space = new P { X = 101, Y = 103 };
            var rSize = new P { X = (int)Math.Floor(space.X / 2d), Y = (int)Math.Floor(space.Y / 2d) };
            Dictionary<R, int> rDict = new()
            {
                { new R { Position = new P { X = 0, Y = 0 }, Size = rSize }, 0 },
                { new R { Position = new P { X = rSize.X + 1, Y = 0 }, Size = rSize }, 0 },
                { new R { Position = new P { X = 0, Y = rSize.Y + 1 }, Size = rSize }, 0 },
                { new R { Position = new P { X = rSize.X + 1, Y = rSize.Y + 1 }, Size = rSize }, 0 },
            };
            if (print)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                for (var y = 0; y < space.Y; y++)
                {
                    for (var x = 0; x < space.X; x++)
                    {
                        Console.Write('.');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Dictionary<P, int> test = [];
            var t = ParseInput().ToList();
            foreach (var (position, velocity) in ParseInput())
            {
                var newPos = (position + (velocity * seconds)) % space;
                foreach (var key in rDict.Keys)
                {
                    if (key.Contains(newPos))
                    {
                        if (test.TryGetValue(newPos, out var r))
                        {
                            r = r + 1;
                            test[newPos] = r;
                        }
                        else
                        {
                            r = 1;
                            test[newPos] = r;
                        }
                        if (print)
                        {
                            Console.SetCursorPosition(newPos.X, newPos.Y);
                            Console.Write('.');
                        }
                        rDict[key] += 1;
                        break;
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            if (print)
            {
                Console.SetCursorPosition(0, space.Y + 1);
            }
            var values = rDict.Values.ToList();

            if (print)
            {
                Console.WriteLine(values[0] * values[1] * values[2] * values[3]);
            }
            return [.. test.Keys];
        }

        public static void Part2()
        {
            // 10403
            var found = new List<HashSet<P>>();
            var i = 0;
            Console.Clear();
            while (true)
            {
                var n = Part1(i, false);
                var wasIn = false;
                foreach (var s in found)
                {
                    if (s.SetEquals(n))
                    {
                        /*Console.Clear();
                        Part1(i, true);
                        Console.WriteLine($"{i} seconds");
                        Console.ReadLine();*/
                        wasIn = true;
                        break;
                    }
                }
                if (!wasIn)
                {
                    found.Add(n);
                }
                else
                {
                    break;
                }
                i++;
            }

            const string DIR = "day14_images";
            if (!Directory.Exists(DIR))
            {
                Directory.CreateDirectory(DIR);
            }

            var space = new P { X = 101, Y = 103 };
            for (i = 0; i < found.Count; i++) 
            {
                var set = found[i];
                using Bitmap bitmap = new(space.X, space.Y);
                foreach (var s in set)
                {
                    bitmap.SetPixel(s.X, s.Y, Color.Black);
                }
                bitmap.Save(Path.Combine(DIR, $"{i}.png"), ImageFormat.Png);
            }
        }

        private static IEnumerable<(P position, P velocity)> ParseInput()
        {
            var lines = File.ReadAllLines("day14_part1.txt");

            foreach (var line in lines) 
            {
                P p;
                P v;
                var s = line.Split(' ');
                var pxy = s[0].Split('=')[1].Split(',');
                var vxy = s[1].Split('=')[1].Split(',');
                p.X = int.Parse(pxy[0]);
                p.Y = int.Parse(pxy[1]);
                v.X = int.Parse(vxy[0]);
                v.Y = int.Parse(vxy[1]);
                yield return (p, v);
            }
        }
    }
}
