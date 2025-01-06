const int DotAsNumber = -1;

//var input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "test_input.txt")).ToCharArray();
var input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "input.txt")).ToCharArray();


// Part 1

var layoutOfFilesAndFreeSpace = GetLayoutOfFilesAndFreeSpace(input);

var part1Checksum = GetPart1Checksum(layoutOfFilesAndFreeSpace);

Console.WriteLine($"Part 1: {part1Checksum}");

// Part 2

layoutOfFilesAndFreeSpace = GetLayoutOfFilesAndFreeSpace(input);

var part2Checksum = GetPart2Checksum(layoutOfFilesAndFreeSpace);

Console.WriteLine($"Part 2: {part2Checksum}");

Console.ReadKey();

List<int> GetLayoutOfFilesAndFreeSpace(char[] input)
{
    var layoutOfFilesAndFreeSpace = new List<int>();

    var indexOfEventNumbers = 0;

    for (var i = 0; i < input.Length; i++)
    {
        var isEven = i % 2 == 0;

        var charAsNumber = int.Parse(input[i].ToString());
        if (!isEven)
        {
            layoutOfFilesAndFreeSpace.AddRange(Enumerable.Repeat(DotAsNumber, charAsNumber));
        }
        else
        {
            layoutOfFilesAndFreeSpace.AddRange(Enumerable.Repeat(indexOfEventNumbers, charAsNumber));
            indexOfEventNumbers++;
        }
    }

    return layoutOfFilesAndFreeSpace;
}

long GetPart1Checksum(List<int> layoutOfFilesAndFreeSpace)
{
    for (var i = 0; i < layoutOfFilesAndFreeSpace.Count; i++)
    {
        if (layoutOfFilesAndFreeSpace[i] != DotAsNumber)
        {
            continue;
        }

        var substringOfRemainingNumbers = layoutOfFilesAndFreeSpace.Skip(i).Where(x => x != DotAsNumber).ToArray();
        if (substringOfRemainingNumbers.Length == 0)
        {
            break;
        }

        layoutOfFilesAndFreeSpace[i] = substringOfRemainingNumbers.Last();
        var lastNumberIndex = layoutOfFilesAndFreeSpace.ToList().FindLastIndex(x => x == substringOfRemainingNumbers.Last());
        layoutOfFilesAndFreeSpace[lastNumberIndex] = DotAsNumber;
    }

    return CalculateChecksum(layoutOfFilesAndFreeSpace);
}

long GetPart2Checksum(List<int> layoutOfFilesAndFreeSpace)
{
    var dotSubsequences = GetDotSubsequences(layoutOfFilesAndFreeSpace);

    var groupedNumbers = layoutOfFilesAndFreeSpace
       .Where(x => x != DotAsNumber)
       .GroupBy(x => x)
       .Select(g =>
       {
           var startIndex = layoutOfFilesAndFreeSpace.IndexOf(g.Key);
           var endIndex = layoutOfFilesAndFreeSpace.LastIndexOf(g.Key);
           var length = endIndex - startIndex + 1;
           return new
           {
               Number = g.Key,
               Length = length,
               StartIndex = startIndex,
               EndIndex = endIndex
           };
       })
       .ToList();

    for (var j = groupedNumbers.Count - 1; j > 0; j--)
    {
        foreach (var subsequence in dotSubsequences)
        {
            if (groupedNumbers[j].Length <= subsequence.Length && groupedNumbers[j].StartIndex > subsequence.StartIndex)
            {
                for (var k = 0; k < groupedNumbers[j].Length; k++)
                {
                    layoutOfFilesAndFreeSpace[k + subsequence.StartIndex] = groupedNumbers[j].Number;
                    layoutOfFilesAndFreeSpace[groupedNumbers[j].StartIndex + k] = DotAsNumber;
                }
                break;
            }
        }

        dotSubsequences = GetDotSubsequences(layoutOfFilesAndFreeSpace);
    }

    return CalculateChecksum(layoutOfFilesAndFreeSpace);
}


List<(int StartIndex, int Length)> GetDotSubsequences(List<int> layoutOfFilesAndFreeSpace) 
{
    var dotSubsequences = new List<(int StartIndex, int Length)>();
    int startIndex = -1;

    for (int i = 0; i < layoutOfFilesAndFreeSpace.Count; i++)
    {
        if (layoutOfFilesAndFreeSpace[i] == DotAsNumber)
        {
            if (startIndex == -1)
            {
                startIndex = i;
            }
        }
        else
        {
            if (startIndex != -1)
            {
                dotSubsequences.Add((startIndex, i - startIndex));
                startIndex = -1;
            }
        }
    }

    if (startIndex != -1)
    {
        dotSubsequences.Add((startIndex, layoutOfFilesAndFreeSpace.Count - startIndex));
    }

    return dotSubsequences;
}

long CalculateChecksum(List<int> layoutOfFilesAndFreeSpace)
{
    var checksum = 0L;
    for (var i = 0; i < layoutOfFilesAndFreeSpace.Count; i++)
    {
        if (layoutOfFilesAndFreeSpace[i] != DotAsNumber)
        {
            checksum += layoutOfFilesAndFreeSpace[i] * i;
        }
    }

    return checksum;
}