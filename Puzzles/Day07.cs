namespace AdventOfCode2024.Puzzles;

internal class Day07 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day07.txt"))
            .Select(l =>
            {
                var parts = l.Split(':');
                return (long.Parse(parts[0]), parts[1].Trim().Split(' ').Select(long.Parse).ToArray());
            })
            .ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1((long testValue, long[] numbers)[] inputData)
    {
        var matchingResults = new List<long>();

        foreach (var (testValue, numbers) in inputData)
        {
            foreach (var combination in GetOperatorCombinations(numbers.Length, '+', '*'))
            {
                var result = numbers[0];

                for (var i = 1; i < numbers.Length; i++)
                {
                    var operation = combination[i - 1];

                    if (operation == '+')
                    {
                        result += numbers[i];
                    }
                    else if (operation == '*')
                    {
                        result *= numbers[i];
                    }
                }

                if (result == testValue)
                {
                    matchingResults.Add(result);
                    break;
                }
            }
        }

        return matchingResults.Sum().ToString();
    }

    private static string Part2((long testValue, long[] numbers)[] inputData)
    {
        var matchingResults = new List<long>();

        foreach (var (testValue, numbers) in inputData)
        {
            foreach (var combination in GetOperatorCombinations(numbers.Length, '+', '*', '|'))
            {
                var result = numbers[0];

                for (var i = 1; i < numbers.Length; i++)
                {
                    var operation = combination[i - 1];

                    if (operation == '+')
                    {
                        result += numbers[i];
                    }
                    else if (operation == '*')
                    {
                        result *= numbers[i];
                    }
                    else if (operation == '|')
                    {
                        result = long.Parse(result.ToString() + numbers[i].ToString());
                    }
                }

                if (result == testValue)
                {
                    matchingResults.Add(result);
                    break;
                }
            }
        }

        return matchingResults.Sum().ToString();
    }

    private static List<List<char>> GetOperatorCombinations(int n, params char[] operators)
    {
        var combinations = new List<List<char>>();

        for (int i = 0; i < (int)Math.Pow(operators.Length, n - 1); i++)
        {
            var operations = new List<char>();
            int current = i;

            for (int slot = 0; slot < n - 1; slot++)
            {
                int remainder = current % operators.Length;
                current /= operators.Length;

                operations.Add(operators[remainder]);
            }

            combinations.Add(operations);
        }

        return combinations;
    }
}
