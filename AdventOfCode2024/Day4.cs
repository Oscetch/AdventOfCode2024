namespace AdventOfCode2024
{
    internal static class Day4
    {
        struct FoundCoordinates
        {
            public int StartX;
            public int StartY;
            public int EndX;
            public int EndY;

            public FoundCoordinates Add(int x, int y) 
            {
                return new FoundCoordinates 
                {
                    StartX = StartX + x,
                    StartY = StartY + y,
                    EndX = EndX + x,
                    EndY = EndY + y
                };
            }
        }

        public static void Part1()
        {
            var searchWord = "XMAS";
            var searchWordArray = searchWord.ToCharArray();
            var reverseSearchWordArray = searchWord.Reverse().ToArray();
            var set = new HashSet<FoundCoordinates>();

            ForEachChunk(searchWord, (chunk, x, y) => 
            {
                AddRangeToSet(SearchHorizontal(chunk, searchWordArray, reverseSearchWordArray), set, x, y);
                AddRangeToSet(SearchVertical(chunk, searchWordArray, reverseSearchWordArray), set, x, y);
                AddRangeToSet(SearchDiagonal(chunk, searchWordArray, reverseSearchWordArray), set, x, y);
            });


            Console.WriteLine(set.Count);
        }

        public static void Part2()
        {
            var searchWord = "MAS";
            var searchWordArray = searchWord.ToCharArray();
            var reverseSearchWordArray = searchWord.Reverse().ToArray();
            var sum = 0;

            ForEachChunk(searchWord, (chunk, x, y) =>
            {
                var diagonals = SearchDiagonal(chunk, searchWordArray, reverseSearchWordArray).Count();
                if (diagonals == 2)
                {
                    sum++;
                }
            });

            Console.WriteLine(sum);
        }

        private static void ForEachChunk(string searchWord, Action<char[][], int, int> action)
        {
            var map = MapInput();
            var chunk = CreateChunkFor(searchWord);
            for (var y = 0; y <= map.Length - chunk.Length; y++)
            {
                for (var x = 0; x <= map[y].Length - chunk.Length; x++)
                {
                    CopyToChunk(map, chunk, x, y);
                    action(chunk, x, y);
                }
            }
        }

        private static void AddRangeToSet(IEnumerable<FoundCoordinates> list, HashSet<FoundCoordinates> coordinates, int offsetX, int offsetY)
        {
            foreach (var coord in list)
            {
                coordinates.Add(coord.Add(offsetX, offsetY));
            }
        }

        private static IEnumerable<FoundCoordinates> SearchDiagonal(char[][] chunk, char[] searchWord, char[] reverseSearchWord)
        {
            var topToBottom = new char[chunk.Length];
            var bottomToTop = new char[chunk.Length];
            for (var n = 0; n < chunk.Length; n++) 
            {
                topToBottom[n] = chunk[n][n];
                bottomToTop[n] = chunk[chunk.Length - n - 1][n];
            }
            if (topToBottom.SequenceEqual(searchWord) || topToBottom.SequenceEqual(reverseSearchWord))
            {
                yield return new FoundCoordinates
                {
                    StartX = 0,
                    StartY = 0,
                    EndX = searchWord.Length - 1,
                    EndY = searchWord.Length - 1,
                };
            }
            if (bottomToTop.SequenceEqual(searchWord) || bottomToTop.SequenceEqual(reverseSearchWord))
            {
                yield return new FoundCoordinates
                {
                    StartX = 0,
                    StartY = searchWord.Length - 1,
                    EndX = searchWord.Length - 1,
                    EndY = 0,
                };
            }
        }

        private static IEnumerable<FoundCoordinates> SearchVertical(char[][] chunk, char[] searchWord, char[] reverseSearchWord)
        {
            FoundCoordinates found;
            for (var x = 0; x < chunk.Length; x++) 
            {
                var line = chunk.Select(c => c[x]).ToArray();
                if (line.SequenceEqual(searchWord) || line.SequenceEqual(reverseSearchWord))
                {
                    found.StartX = x;
                    found.EndX = x;
                    found.StartY = 0;
                    found.EndY = searchWord.Length - 1;
                    yield return found;
                }
            }
        }

        private static IEnumerable<FoundCoordinates> SearchHorizontal(char[][] chunk, char[] searchWord, char[] reverseSearchWord)
        {
            FoundCoordinates found;
            for (var y = 0; y < chunk.Length; ++y)
            {
                var line = chunk[y];
                if (line.SequenceEqual(searchWord) || line.SequenceEqual(reverseSearchWord))
                {
                    found.StartX = 0;
                    found.StartY = y;
                    found.EndX = line.Length - 1;
                    found.EndY = y;
                    yield return found;
                }
            }
        }

        private static void CopyToChunk(char[][] map, char[][] chunk, int x, int y)
        {
            for (var chunkY = 0; chunkY < chunk.Length; chunkY++)
            {
                Array.Copy(map[y + chunkY], x, chunk[chunkY], 0, chunk.Length);
            }
        }

        private static char[][] CreateChunkFor(string searchWord)
        {
            var chunk = new char[searchWord.Length][];
            for (var i = 0; i < searchWord.Length; i++)
            {
                chunk[i] = new char[searchWord.Length];
            }
            return chunk;
        }

        private static char[][] MapInput()
        {
            var lines = File.ReadAllLines("day4_1.txt");
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
