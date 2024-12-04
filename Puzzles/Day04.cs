namespace AdventOfCode2024.Puzzles;

internal class Day04 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day04.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var count = 0;

        for (int i = 0; i < inputData.Length; i++)
        {
            for (int j = 0; j < inputData[i].Length; j++)
            {
                var character = inputData[i][j];

                if (character == 'X')
                {
                    foreach (var ((rowM, colM), dir) in FindAdjacentCharacters(inputData, i, j, 'M', (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1)))
                    {
                        foreach (var ((rowA, colA), dir2) in FindAdjacentCharacters(inputData, rowM, colM, 'A', dir))
                        {
                            foreach (var ((rowS, colS), dir3) in FindAdjacentCharacters(inputData, rowA, colA, 'S', dir2))
                            {
                                count++;
                            }
                        }
                    }
                }
            }
        }

        return count.ToString();
    }

    private static string Part2(string[] inputData)
    {
        var count = 0;
        var SharedAPositions = new HashSet<(int, int)>();

        for (int i = 0; i < inputData.Length; i++)
        {
            for (int j = 0; j < inputData[i].Length; j++)
            {
                var character = inputData[i][j];

                if (character == 'M')
                {
                    foreach (var ((rowA, colA), dir) in FindAdjacentCharacters(inputData, i, j, 'A', (-1, 1), (1, 1), (1, -1), (-1, -1)))
                    {
                        foreach (var ((rowS, colS), dir2) in FindAdjacentCharacters(inputData, rowA, colA, 'S', dir))
                        {
                            if (SharedAPositions.Contains((rowA, colA)))
                            {
                                count++;
                            }

                            SharedAPositions.Add((rowA, colA));
                        }
                    }
                }
            }
        }

        return count.ToString();
    }

    public static IEnumerable<((int, int) position, (int, int) dir)> FindAdjacentCharacters(string[] inputData, int rowIndex, int colIndex, char characterToFind, params (int, int)[] directions)
    {
        foreach (var (x, y) in directions)
        {
            var row = rowIndex + y;
            var col = colIndex + x;

            if (row < 0 || col < 0 || row > inputData.Length - 1 || col > inputData[row].Length - 1)
            {
                continue;
            }

            if (inputData[row][col] == characterToFind)
            {
                yield return ((row, col), (x, y));
            }
        }
    }
}
