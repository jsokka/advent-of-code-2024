namespace AdventOfCode2024.Puzzles;

internal class Day01 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day01.txt")).ToArray();

        var (left, right) = GetIdPairs(inputData);

        return (Part1(left, right), Part2(left, right));
    }

    private static (int[], int[]) GetIdPairs(string[] inputData)
    {
        var pairs = inputData.Select(s =>
        {
            var pair = s.Split(new string(' ', 3));
            return (int.Parse(pair[0]), int.Parse(pair[1]));
        });

        return ([.. pairs.Select(p => p.Item1).Order()], [.. pairs.Select(p => p.Item2).Order()]);
    }

    private static string Part1(int[] left, int[] right)
    {
        var totalDistance = 0;

        for (var i = 0; i < left.Length; i++)
        {
            totalDistance += Math.Abs(left[i] - right[i]);
        }

        return totalDistance.ToString();
    }

    private static string Part2(int[] left, int[] right)
    {
        var similarityScore = 0;

        for (var i = 0; i < left.Length; i++)
        {
            similarityScore += left[i] * right.Count(p => p == left[i]);
        }

        return similarityScore.ToString();
    }
}
