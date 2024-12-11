namespace AdventOfCode2024.Puzzles;

internal class Day11 : IPuzzle
{
    private static Dictionary<(long stone, int depth), long> _cache = new();

    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day11.txt"))
            .Single().Split(' ');

        return (Part1([.. inputData]), Part2([.. inputData]));
    }

    private static string Part1(List<string> stones)
    {
        const int blinkTimes = 2;

        for (int blink = 0; blink < blinkTimes; blink++)
        {
            var stoneCount = stones.Count;
            for (int i = stoneCount - 1; i > -1; i--)
            {
                if (stones[i] == "0")
                {
                    stones[i] = "1";
                }
                else if (stones[i].Length % 2 == 0)
                {
                    var newStone1 = stones[i][..(stones[i].Length / 2)];
                    var newStone2 = stones[i][(stones[i].Length / 2)..];
                    stones.RemoveAt(i);
                    stones.Insert(i, long.Parse(newStone1).ToString());
                    stones.Insert(i + 1, long.Parse(newStone2).ToString());
                }
                else
                {
                    var newStone = (long.Parse(stones[i]) * 2024).ToString();
                    stones.RemoveAt(i);
                    stones.Insert(i, newStone);
                }
            }
        }

        return stones.Count.ToString();
    }

    private static string Part2(List<string> stones)
    {
        return stones.Sum(stone => CountStones(long.Parse(stone), 75)).ToString();
    }

    private static long CountStones(long stone, int depth)
    {
        if (depth == 0)
        {
            return 1;
        }

        if (_cache.TryGetValue((stone, depth), out var value))
        {
            return value;
        }

        long result;
        var s = stone.ToString();

        if (stone == 0)
        {
            result = CountStones(1, depth - 1);
        }
        else if (s.Length % 2 == 0)
        {
            result = CountStones(long.Parse(s[..(s.Length / 2)]), depth - 1) +
                CountStones(long.Parse(s[(s.Length / 2)..]), depth - 1);
        }
        else
        {
            result = CountStones(stone * 2024, depth - 1);
        }

        _cache.Add((stone, depth), result);

        return result;
    }
}
