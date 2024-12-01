namespace AdventOfCode2024.Puzzles
{
    internal interface IPuzzle
    {
        Task<(string, string)> Solve();
    }
}
