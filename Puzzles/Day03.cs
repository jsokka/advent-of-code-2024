using System.Text.RegularExpressions;

namespace AdventOfCode2024.Puzzles;

internal partial class Day03 : IPuzzle
{
    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex _mulPattern();

    [GeneratedRegex(@"(do)\(\)|(don't)\(\)|(mul\((\d{1,3}),(\d{1,3})\))")]
    private partial Regex _mulPattern2();

    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day03.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputLines)
    {
        var total = 0;

        foreach (var line in inputLines)
        {
            var matches = _mulPattern().Matches(line);

            foreach (var groups in matches.Where(match => match != null).Select(m => m.Groups))
            {
                total += int.Parse(groups[1].Value) * int.Parse(groups[2].Value);
            }
        }

        return total.ToString();
    }

    private string Part2(string[] inputLines)
    {
        var enabled = true;
        var total = 0;

        foreach (var line in inputLines)
        {
            foreach (var groups in _mulPattern2().Matches(line).Select(m => m.Groups))
            {
                if (groups[1].Length > 0)
                {
                    enabled = true;
                }
                else if (groups[2].Length > 0)
                {
                    enabled = false;
                }
                else if (enabled)
                {
                    total += int.Parse(groups[4].Value) * int.Parse(groups[5].Value);
                }
            }
        }

        return total.ToString();
    }
}
