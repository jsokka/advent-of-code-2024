namespace AdventOfCode2024.Puzzles;

internal class Day08 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day08.txt")).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(string[] inputData)
    {
        var antennaPositions = GetAntennaPositions(inputData).ToArray();

        var antinodes = new List<(int x, int y)>();

        for (int i = 0; i < antennaPositions.Length; i++)
        {
            var antenna1 = antennaPositions[i];

            for (int j = i + 1; j < antennaPositions.Length; j++)
            {
                var antenna2 = antennaPositions[j];

                if (antenna1.frequency != antenna2.frequency)
                {
                    continue;
                }

                var deltaX1 = antenna1.position.x - antenna2.position.x;
                var deltaY1 = antenna1.position.y - antenna2.position.y;
                var deltaX2 = deltaX1 * -1;
                var deltaY2 = deltaY1 * -1;

                (int x, int y) antinode1Position = (antenna1.position.x + deltaX1, antenna1.position.y + deltaY1);
                (int x, int y) antinode2Position = (antenna2.position.x + deltaX2, antenna2.position.y + deltaY2);

                if (IsInRange(inputData, antinode1Position))
                {
                    antinodes.Add(antinode1Position);
                }

                if (IsInRange(inputData, antinode2Position))
                {
                    antinodes.Add(antinode2Position);
                }
            }
        }

        return antinodes.ToHashSet().Count.ToString();
    }

    private static string Part2(string[] inputData)
    {
        var antennaPositions = GetAntennaPositions(inputData).ToArray();

        var antinodes = new List<(int x, int y)>();

        for (int i = 0; i < antennaPositions.Length; i++)
        {
            var antenna1 = antennaPositions[i];

            for (int j = i + 1; j < antennaPositions.Length; j++)
            {
                var antenna2 = antennaPositions[j];

                if (antenna1.frequency != antenna2.frequency)
                {
                    continue;
                }

                var deltaX1 = antenna1.position.x - antenna2.position.x;
                var deltaY1 = antenna1.position.y - antenna2.position.y;
                var deltaX2 = deltaX1 * -1;
                var deltaY2 = deltaY1 * -1;

                (int x, int y) antinodePosition = (antenna1.position.x + deltaX1, antenna1.position.y + deltaY1);

                while (IsInRange(inputData, antinodePosition))
                {
                    antinodes.Add(antinodePosition);
                    antinodePosition = (antinodePosition.x + deltaX1, antinodePosition.y + deltaY1);
                }

                antinodePosition = (antenna2.position.x + deltaX2, antenna2.position.y + deltaY2);

                while (IsInRange(inputData, antinodePosition))
                {
                    antinodes.Add(antinodePosition);
                    antinodePosition = (antinodePosition.x + deltaX2, antinodePosition.y + deltaY2);
                }
            }
        }

        var antennaPositionAntinodes = antennaPositions.GroupBy(a => a.frequency).Where(grp => grp.Count() > 1).SelectMany(grp => grp.Select(g => g.position));
        return antinodes.Concat(antennaPositionAntinodes).ToHashSet().Count.ToString();
    }

    private static IEnumerable<(char frequency, (int x, int y) position)> GetAntennaPositions(string[] inputData)
    {
        for (int y = 0; y < inputData.Length; y++)
        {
            for (int x = 0; x < inputData[y].Length; x++)
            {
                var frequency = inputData[y][x];
                if (frequency != '.')
                {
                    yield return (frequency, (x, y));
                }
            }
        }
    }


    private static bool IsInRange(string[] inputData, (int x, int y) position)
    {
        var maxY = inputData.Length - 1;
        var maxX = inputData[maxY].Length - 1;

        return position.x > -1 && position.x <= maxX && position.y > -1 && position.y <= maxY;
    }
}
