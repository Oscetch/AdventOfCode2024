namespace AdventOfCode2024
{
    internal static class Day11
    {
        public static void Part1()
        {
            var input = ParseInput();
            for (var i = 0; i < 25; i++)
            {
                input = Blink(input);
            }

            Console.WriteLine(input.Count);
        }
        public static void Part2()
        {
            var input = ParseInput().ToDictionary(x => x, x => (ulong)1);
            for (var i = 0; i < 75; i++)
            {
                var copy = input.ToDictionary();
                foreach (var (k, v) in copy)
                {
                    if (v == 0) continue;
                    input[k] -= v;
                    var blinkValues = Blink([k]);

                    foreach (var blinkVal in blinkValues)
                    {
                        if (input.ContainsKey(blinkVal))
                        {
                            input[blinkVal] += v;
                        }
                        else
                        {
                            input[blinkVal] = v;
                        }
                    }
                }
            }

            ulong sum = 0;
            foreach (var v in input.Values)
            {
                sum += v;
            }

            Console.WriteLine(sum);
        }

        private static List<long> Blink(IEnumerable<long> longs)
        {
            var result = new List<long>();
            foreach (var item in longs)
            {
                if (item == 0)
                {
                    result.Add(1);
                    continue;
                }
                var s = item.ToString();

                if ((s.Length & 1) > 0)
                {
                    result.Add(item * 2024L);

                    continue;
                }

                var halfLength = s.Length / 2;
                var i1 = long.Parse(s[..halfLength]);
                var i2 = long.Parse(s[halfLength..]);
                result.Add(i1);
                result.Add(i2);
            }
            return result;
        }

        private static List<long> ParseInput()
        {
            var line = File.ReadAllText("day11_part1.txt");
            return line.Split(' ').Select(long.Parse).ToList();
        }
    }
}
