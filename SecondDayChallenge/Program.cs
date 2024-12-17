//var inputLines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "test_input.txt"));
var inputLines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

var reports = new List<List<int>>();

foreach (var line in inputLines)
{
    var levels = line.Split([' '], StringSplitOptions.RemoveEmptyEntries).ToList()
        .Select(int.Parse)
        .ToList();
    reports.Add(levels);
}

// Part 1
var safeReports = reports.Count(IsSafeReport);
Console.WriteLine(safeReports);

// Part 2
var safeReportsWithMaxOneLevelBad = reports.Count(IsSafeReportWithOneBadLevel);
Console.WriteLine(safeReportsWithMaxOneLevelBad);
Console.ReadKey();

static bool IsSafeReport(List<int> report)
{
    if (!IsIncreasing(report) && !IsDecreasing(report))
    {
        return false;
    }

    for (var i = 1; i < report.Count; i++)
    {
        var differenceBetweenPreviousAndCurrent = Math.Abs(report[i] - report[i - 1]);
        if (differenceBetweenPreviousAndCurrent < 1 || differenceBetweenPreviousAndCurrent > 3)
        {
            return false;
        }
    }

    return true;
}

static bool IsSafeReportWithOneBadLevel(List<int> report)
{
    if (IsSafeReport(report))
    {
        return true;
    }

    for (var i = 0; i < report.Count; i++)
    {
        var copyOfReport = new List<int>(report);
        copyOfReport.RemoveAt(i);

        if (IsSafeReport(copyOfReport))
        {
            return true;
        }
    }

    return false;
}

static bool IsIncreasing(List<int> list)
{
    for (int i = 1; i < list.Count; i++)
    {
        if (list[i] <= list[i - 1])
        {
            return false;
        }
    }

    return true;
}

static bool IsDecreasing(List<int> list)
{
    for (int i = 1; i < list.Count; i++)
    {
        var element = list[i];
        var element2 = list[i - 1];
        if (list[i] >= list[i - 1])
        {
            return false;
        }
    }

    return true;
}