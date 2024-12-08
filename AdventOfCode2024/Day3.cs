using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2024
{
    internal static class Day3
    {
        private class MulBuilder
        {
            enum LOOKING_FOR
            {
                START,
                START_NUMBER,
                MIDDLE,
                END_NUMBER,
                END,
                FINISHED
            }

            private const string START = "mul(";
            private const char END = ')';
            private const char MIDDLE = ',';
            private readonly StringBuilder _sb = new();
            private LOOKING_FOR _lookingFor = LOOKING_FOR.START;

            public bool TryAdd(char c, out int val)
            {
                val = 0;
                var mul = _sb.Append(c).ToString();
                if (!Validate(mul, c))
                {
                    Clear();
                    return false;
                }

                if (_lookingFor == LOOKING_FOR.FINISHED)
                {
                    val = MullsInString(mul);
                    Clear();
                    return true;
                }
                return false;
            }

            public void Clear()
            {
                _sb.Clear();
                _lookingFor = LOOKING_FOR.START;
            }

            private bool Validate(string mul, char c)
            {
                switch (_lookingFor)
                {
                    case LOOKING_FOR.START:
                        if (mul == START)
                        {
                            _lookingFor = LOOKING_FOR.START_NUMBER;
                            return true;
                        }
                        return START.StartsWith(mul);
                    case LOOKING_FOR.START_NUMBER:
                        if (char.IsDigit(c))
                        {
                            _lookingFor = LOOKING_FOR.MIDDLE;
                            return true;
                        }
                        return false;
                    case LOOKING_FOR.MIDDLE:
                        if (char.IsDigit(c))
                        {
                            return true;
                        }
                        if (c == MIDDLE)
                        {
                            _lookingFor = LOOKING_FOR.END_NUMBER;
                            return true;
                        }
                        return false;
                    case LOOKING_FOR.END_NUMBER:
                        if (char.IsDigit(c))
                        {
                            _lookingFor = LOOKING_FOR.END;
                            return true;
                        }
                        return false;
                    case LOOKING_FOR.END:
                        if (char.IsDigit(c))
                        {
                            return true;
                        }
                        if (c == END)
                        {
                            _lookingFor = LOOKING_FOR.FINISHED;
                            return true;
                        }
                        return false;
                }
                return false;
            }
        }

        private static readonly Regex mulRegex = new ("mul\\(\\d+,\\d+\\)");
        private static readonly Regex numberRegex = new("\\d+");

        public static void Part1()
        {
            var input = File.ReadAllText("day3_1.txt");

            var result = MullsInString(input);

            Console.WriteLine(result);
        }

        public static void Part2()
        {
            var input = File.ReadAllText("day3_1.txt");

            var lookForDo = false;
            var mulBuilder = new MulBuilder();
            var sum = 0;
            for (var i = 0; i < input.Length; i++)
            {
                if (lookForDo)
                {
                    if (IsWordNext(i, input, "do()"))
                    {
                        lookForDo = false;
                    } else
                    {
                        continue;
                    }
                }
                else
                {
                    if (IsWordNext(i, input, "don't()"))
                    {
                        mulBuilder.Clear();
                        lookForDo = true;
                        continue;
                    }
                }

                if (mulBuilder.TryAdd(input[i], out var mul))
                {
                    sum += mul;
                }
            }

            Console.WriteLine(sum);
        }

        private static bool IsWordNext(int i, string input, string word)
        {
            var maxLength = Math.Min(input.Length - i, word.Length);
            return input.Substring(i, maxLength) == word;
        }

        private static int MullsInString(string input) => GetRegexMatches(mulRegex, input)
                .Select(x => GetRegexMatches(numberRegex, x).ToArray())
                .Sum(x => int.Parse(x[0]) * int.Parse(x[1]));

        private static IEnumerable<string> GetRegexMatches(Regex regex, string value) =>
            regex.Matches(value).Select(x => x.Value);
    }
}
