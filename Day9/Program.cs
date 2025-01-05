using System.Collections.Generic;
using System.Text;

const int DotAsNumber = -1;

var input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "test_input.txt")).ToCharArray();
//var input = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "input.txt")).ToCharArray();



// Part 1

var layoutOfFilesAndFreeSpace = GetLayoutOfFilesAndFreeSpace(input);

var part1Checksum = GetPart1Checksum(layoutOfFilesAndFreeSpace);

Console.WriteLine($"Part 1: {part1Checksum}");

// Part 2

layoutOfFilesAndFreeSpace = GetLayoutOfFilesAndFreeSpace(input);

var previousLayout = GetLayoutOfFilesAndFreeSpace(input);

var part2Checksum = GetPart2Checksum(previousLayout, layoutOfFilesAndFreeSpace, 0);

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

    var arrangedLayout = layoutOfFilesAndFreeSpace.Where(c => c != DotAsNumber).ToList();
    var checksum = 0L;

    for (var i = 0; i < arrangedLayout.Count; i++)
    {
        checksum += long.Parse(arrangedLayout[i].ToString()) * i;
    }

    return checksum;
}

long GetPart2Checksum(List<int> previousLayoutOfFilesAndFreeSpace, List<int> layoutOfFilesAndFreeSpace, int sameListOccurrences)
{
    var usedNumbers = new List<int>();
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

        var groupedNumbers = substringOfRemainingNumbers
          .GroupBy(x => x)
          .Where(x => !usedNumbers.Contains(x.Key))
          .Select(g =>
          {
              var startIndex = layoutOfFilesAndFreeSpace.IndexOf(g.Key, i);
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

        var emptySpaceLength = groupedNumbers[0].StartIndex - i;
        if (emptySpaceLength < 0)
        {
            break;
        }

        for (var j = groupedNumbers.Count - 1; j > 0; j--)
        {
            if (groupedNumbers[j].Length <= emptySpaceLength)
            {
                for (var k = 0; k < groupedNumbers[j].Length; k++)
                {
                    layoutOfFilesAndFreeSpace[k + i] = groupedNumbers[j].Number;
                    layoutOfFilesAndFreeSpace[groupedNumbers[j].StartIndex + k] = DotAsNumber;
                    usedNumbers.Add(groupedNumbers[j].Number);
                }

                break;
            }
        }

        Console.WriteLine(string.Join(string.Empty, layoutOfFilesAndFreeSpace));
    }


    return 0L;
}

