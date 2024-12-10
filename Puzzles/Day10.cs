using System.Data;

namespace AdventOfCode2024.Puzzles;

internal class Day10 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day10.txt"))
            .Select(x => x.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(int[][] map)
    {
        var totalScore = 0;

        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] != 0)
                {
                    continue;
                }

                var trails = FindTrails(map, row, col).ToHashSet();
                totalScore += trails.Count;
            }
        }

        return totalScore.ToString();
    }

    private static string Part2(int[][] map)
    {
        var totalRating = 0;

        for (int row = 0; row < map.Length; row++)
        {
            for (int col = 0; col < map[row].Length; col++)
            {
                if (map[row][col] != 0)
                {
                    continue;
                }

                var trails = FindTrails(map, row, col);
                totalRating += trails.Count();
            }
        }

        return totalRating.ToString();
    }

    private static IEnumerable<(int row, int col)> FindTrails(int[][] map, int row, int col)
    {
        var currentHeight = map[row][col];

        foreach (var point in GetAdjacentPoints(map, row, col).Where(p => p.height == currentHeight + 1))
        {
            if (point.height == 9)
            {
                yield return point.position;
            }
            else
            {
                foreach (var trail in FindTrails(map, point.position.row, point.position.col))
                {
                    yield return trail;
                }
            }
        }
    }

    private static IEnumerable<(int height, (int row, int col) position)> GetAdjacentPoints(int[][] map, int row, int col)
    {
        foreach (var delta in new (int col, int row)[] { (0, -1), (1, 0), (0, 1), (-1, 0) })
        {
            (int row, int col) position = (row + delta.row, col + delta.col);

            if (position.row < 0 || position.row > map.Length - 1 || position.col < 0 || position.col > map[position.row].Length - 1)
            {
                continue;
            }

            yield return (map[position.row][position.col], position);
        }
    }
}
