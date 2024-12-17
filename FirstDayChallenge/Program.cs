//var inputLines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "test_input.txt"));
var inputLines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));
var firstList = new List<int>();
var secondList = new List<int>();

foreach (var line in inputLines)
{
    var parts = line.Split([' '], StringSplitOptions.RemoveEmptyEntries);
    firstList.Add(int.Parse(parts[0]));
    secondList.Add(int.Parse(parts[1]));
}

firstList = [.. firstList.OrderBy(x => x)];
secondList = [.. secondList.OrderBy(x => x)];

// Part 1

var totalDistance = 0L;

for (int i = 0; i < firstList.Count; i++)
{
    totalDistance += Math.Abs(firstList[i] - secondList[i]);
}

Console.WriteLine(totalDistance);

// Part 2

var repeatsOfParticularNumber = secondList.GroupBy(x => x)
    .ToDictionary(x => x.Key, x => x.Count());

var similarityScore = 0L;

foreach (var item in firstList)
{
    if (repeatsOfParticularNumber.TryGetValue(item, out var repeats))
    {
        similarityScore += repeats * item;
    }
}

Console.WriteLine(similarityScore);

Console.ReadKey();