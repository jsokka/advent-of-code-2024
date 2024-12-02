namespace AdventOfCode2024.Puzzles;

internal class Day02 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day02.txt")).ToArray();

        var reports = GetReports(inputData).ToArray();

        return (Part1(reports), Part2(reports));
    }

    private static IEnumerable<int[]> GetReports(string[] inputData)
    {
        foreach (var row in inputData)
        {
            yield return row.Split(' ').Select(int.Parse).ToArray();
        }
    }

    private static string Part1(IEnumerable<int[]> reports)
    {
        return reports.Count(IsReportSafe).ToString();
    }

    private static string Part2(IEnumerable<int[]> reports)
    {
        var safeReportCount = 0;

        foreach (var report in reports)
        {
            if (IsReportSafe(report))
            {
                safeReportCount++;
                continue;
            }

            for (var i = 0; i < report.Length; i++)
            {
                var newReport = new List<int>(report);
                newReport.RemoveAt(i);

                if (IsReportSafe([.. newReport]))
                {
                    safeReportCount++;
                    break;
                }
            }
        }

        return safeReportCount.ToString();
    }

    private static bool IsReportSafe(int[] report)
    {
        var increasing = report[1] > report[0];
        var decreasing = report[1] < report[0];

        for (int i = 1; i < report.Length; i++)
        {
            var current = report[i];
            var previous = report[i - 1];

            if (!((increasing && current > previous) || (decreasing && current < previous))
                || Math.Abs(current - previous) > 3)
            {
                return false;
            }
        }

        return true;
    }
}
