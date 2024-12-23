namespace AdventOfCode2024
{
    internal static class Day13
    {
        struct Info
        {
            public int AX;
            public int AY;

            public int BX;
            public int BY;

            public long PriceX;
            public long PriceY;
        }

        public static void Part1()
        {
            var sum = 0L;
            foreach (var item in ParseInput())
            {
                var determinant = item.AX * item.BY - item.AY * item.BX;
                var determinantA = item.PriceX * item.BY - item.PriceY * item.BX;
                var determinantB = item.AX * item.PriceY - item.AY * item.PriceX;

                if (determinantA % determinant != 0 || determinantB % determinant != 0) continue;

                var aPresses = determinantA / determinant;
                var bPresses = determinantB / determinant;

                if (aPresses > 100 || bPresses > 100) continue;

                sum += aPresses * 3 + bPresses;
            }

            Console.WriteLine(sum);
        }

        public static void Part2()
        {
            var sum = 0L;
            foreach (var item in ParseInput(10000000000000L))
            {
                var determinant = item.AX * item.BY - item.AY * item.BX;
                var determinantA = item.PriceX * item.BY - item.PriceY * item.BX;
                var determinantB = item.AX * item.PriceY - item.AY * item.PriceX;

                if (determinantA % determinant != 0 || determinantB % determinant != 0) continue;

                var aPresses = determinantA / determinant;
                var bPresses = determinantB / determinant;

                sum += aPresses * 3 + bPresses;
            }

            Console.WriteLine(sum);
        }

        private static IEnumerable<Info> ParseInput(long prizeAdjustment = 0)
        {
            var lines = File.ReadAllLines("day13_part1.txt");
            var info = new Info();
            foreach (var line in lines)
            {
                if (line.StartsWith("Button A"))
                {
                    var aMovement = line.Split(':')[1].Trim().Split(',');
                    info.AX = int.Parse(aMovement[0].Split('+')[1]);
                    info.AY = int.Parse(aMovement[1].Split('+')[1]);
                }
                else if (line.StartsWith("Button B"))
                {
                    var bMovement = line.Split(':')[1].Trim().Split(',');
                    info.BX = int.Parse(bMovement[0].Split('+')[1]);
                    info.BY = int.Parse(bMovement[1].Split('+')[1]);
                }
                else if (line.StartsWith("Prize"))
                {
                    var prize = line.Split(':')[1].Trim().Split(',');
                    info.PriceX = int.Parse(prize[0].Split('=')[1]) + prizeAdjustment;
                    info.PriceY = int.Parse(prize[1].Split('=')[1]) + prizeAdjustment;
                    yield return info;
                }
                else
                {
                    continue;
                }
            }
        }
    }
}
