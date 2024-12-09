using System.Text;

namespace AdventOfCode2024
{
    internal static class Day9
    {
        public static void Part1()
        {
            var list = ParseBlocks();

            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] != ".") continue;

                for (var j = list.Count - 1; j > i; j--)
                {
                    if (list[j] != ".")
                    {
                        list[i] = list[j];
                        list[j] = ".";
                        break;
                    }
                }
            }

            var sum = 0L;
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] == ".") break;
                sum += int.Parse(list[i]) * i;
            }

            Console.WriteLine(sum);
        }

        struct Fragment
        {
            public string Id;
            public int Count;
            public int IndexStart;
        }

        public static void Part2()
        {
            var list = ParseBlocks();
            var fragments = new List<Fragment>();
            var lastId = list[0];
            var indexStart = 0;
            var count = 1;
            Fragment fragment;
            for (var i = count; i < list.Count; i++)
            {
                var id = list[i];
                if (id == lastId)
                {
                    count++;
                    continue;
                }

                fragment.Id = lastId;
                fragment.Count = count;
                fragment.IndexStart = indexStart;
                fragments.Add(fragment);

                indexStart = i;
                lastId = id;
                count = 1;
            }

            fragment.Id = lastId;
            fragment.Count = count;
            fragment.IndexStart = indexStart;
            fragments.Add(fragment);

            for (var i = fragments.Count - 1; i >= 0; i--)
            {
                var f = fragments[i];
                if (f.Id == ".") continue;
                for (var j = 0; j < i; j++)
                {
                    var fo = fragments[j];
                    if (fo.Id != "." || fo.Count < f.Count) continue;
                    for (var k = 0; k < f.Count; k++)
                    {
                        list[fo.IndexStart + k] = f.Id;
                        list[f.IndexStart + k] = fo.Id;
                    }
                    Fragment nfo;
                    nfo.Id = fo.Id;
                    nfo.IndexStart = fo.IndexStart + f.Count;
                    nfo.Count = fo.Count - f.Count;
                    fragments[j] = nfo;
                    break;
                }
            }

            var sum = 0L;
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] == ".") continue;
                sum += int.Parse(list[i]) * i;
            }

            Console.WriteLine(sum);
        }

        private static List<string> ParseBlocks()
        {
            var input = ParseInput();
            var values = new List<string>();
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                var intValue = int.Parse(c.ToString());
                if ((i & 1) == 1)
                {
                    for (var j = 0; j < intValue; j++)
                    {
                        values.Add(".");
                    }
                }
                else
                {
                    var iString = (i / 2).ToString();
                    for (var j = 0; j < intValue; j++)
                    {
                        values.Add(iString);
                    }
                }
            }
            return values;
        }

        private static string ParseInput()
        {
            return File.ReadAllText("day9_part1.txt");
        }
    }
}
