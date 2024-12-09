namespace AdventOfCode2024.Puzzles;

internal class Day09 : IPuzzle
{
    public async Task<(string, string)> Solve()
    {
        var inputData = (await InputDataReader.GetInputDataAsync<string>("Day09.txt"))
            .Single().Select(x => int.Parse(x.ToString())).ToArray();

        return (Part1(inputData), Part2(inputData));
    }

    private static string Part1(int[] diskMap)
    {
        var blocks = ReadFileBlocks(diskMap).ToArray();

        for (int i = blocks.Length - 1; i > -1; i--)
        {
            if (blocks[i] is null)
            {
                continue;
            }

            var freeSpaceIndex = Array.IndexOf(blocks, null);
            blocks[freeSpaceIndex] = blocks[i];
            blocks[i] = null;
        }

        return blocks.Where(b => b is not null).Select((b, i) => (long)b!.Value * i).Sum().ToString();
    }

    private static string Part2(int[] diskMap)
    {
        var blocks = ReadFileBlocks(diskMap).ToArray();

        for (int i = blocks.Length - 1; i > -1; i--)
        {
            var block = blocks[i];

            if (block is null)
            {
                continue;
            }

            var fileStartIndex = GetFileStartIndex(blocks, i);

            MoveFile(blocks, fileStartIndex, i);

#pragma warning disable S127 // "for" loop stop conditions should be invariant
            i = fileStartIndex;
#pragma warning restore S127 // "for" loop stop conditions should be invariant
        }

        return blocks.Select((b, i) => ((long?)b ?? 0) * i).Sum().ToString();
    }

    private static IEnumerable<int?> ReadFileBlocks(int[] diskMap)
    {
        var fileId = 0;

        for (int i = 0; i < diskMap.Length; i++)
        {
            int blockCount = diskMap[i];
            int? blockContent = i % 2 == 0 ? fileId++ : null;

            for (int j = 0; j < blockCount; j++)
            {
                yield return blockContent;
            }
        }
    }

    private static int GetFileStartIndex(int?[] blokcs, int endIndex)
    {
        var fileId = blokcs[endIndex]!;

        for (int i = endIndex; i > -1; i--)
        {
            var currentFileId = blokcs[i];
            if (currentFileId is null || currentFileId != fileId)
            {
                return i + 1;
            }
        }

        return 0;
    }

    private static void MoveFile(int?[] blocks, int fileStartIndex, int fileEndIndex)
    {
        var fileLength = fileEndIndex - fileStartIndex + 1;
        int? freeSpaceStartIndex = null;

        for (int i = 0; i < fileStartIndex; i++)
        {
            if (blocks[i] is null)
            {
                freeSpaceStartIndex ??= i;

                if (fileLength == i - freeSpaceStartIndex.Value + 1)
                {
                    for (int j = freeSpaceStartIndex.Value; j <= i; j++)
                    {
                        blocks[j] = blocks[fileStartIndex + j - freeSpaceStartIndex.Value];
                        blocks[fileStartIndex + j - freeSpaceStartIndex.Value] = null;
                    }

                    return;
                }
            }
            else
            {
                freeSpaceStartIndex = null;
            }
        }
    }
}
