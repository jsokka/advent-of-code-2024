namespace AdventOfCode2024.Puzzles;

internal class Day05 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day05.txt")).ToArray();

        var rules = inputData.Where(l => l.Contains('|')).Select(l =>
        {
            var parts = l.Split('|');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }).ToArray();

        var updates = inputData.Where(l => l.Contains(','))
            .Select(l => l.Split(",").Select(int.Parse).ToArray()).ToArray();

        return (Part1(rules, updates), Part2(rules, updates));
    }

    private static string Part1((int left, int right)[] rules, int[][] updates)
    {
        return updates.Where(u => IsCorrectlyOrdered(u, rules)).Sum(u => u[u.Length / 2]).ToString();
    }

    private static string Part2((int left, int right)[] rules, int[][] updates)
    {
        var incorrectUpdates = updates.Where(u => !IsCorrectlyOrdered(u, rules)).ToArray();

        foreach (var update in incorrectUpdates)
        {
            for (var i = 0; i < update.Length; i++)
            {
                var page = update[i];

                for (int j = i + 1; j < update.Length; j++)
                {
                    var otherPage = update[j];

                    if ((i > j && Array.Exists(rules, r => r.left == page && r.right == otherPage)) ||
                        (i < j && Array.Exists(rules, r => r.left == otherPage && r.right == page)))
                    {
                        update[i] = otherPage;
                        update[j] = page;
                        page = update[i];
                        otherPage = update[j];
                    }
                }
            }
        }

        return incorrectUpdates.Sum(u => u[u.Length / 2]).ToString();
    }

    private static bool IsCorrectlyOrdered(int[] update, (int left, int right)[] rules)
    {
        var i = 0;
        foreach (var page in update)
        {
            if (i > 0 && Array.Exists(rules, r => r.left == page && Array.IndexOf(update, r.right) != -1 && Array.IndexOf(update, r.right) < i))
            {
                return false;
            }

            if (i < update.Length - 1 && Array.Exists(rules, r => r.right == page && Array.IndexOf(update, r.left) != -1 && Array.IndexOf(update, r.left) > i))
            {
                return false;
            }

            i++;
        }

        return true;
    }
}