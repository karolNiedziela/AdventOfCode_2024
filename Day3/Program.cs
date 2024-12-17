using System.Text.RegularExpressions;

var text = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "test_input.txt"));
//var text = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "input.txt"));

// Part 1

var mulInstructionsPattern = @"mul\((\d+,\d+)\)";
var mulMatches = Regex.Matches(text, mulInstructionsPattern);

var pairs = new List<(int, int)>();

foreach (Match match in mulMatches)
{
    var instruction = match.Groups[1].Value;
    var instructionParts = instruction.Split(',').Select(int.Parse).ToArray();
    pairs.Add((instructionParts[0], instructionParts[1]));
}

var result = pairs.Select(x => x.Item1 * x.Item2).Sum();
Console.WriteLine(result);

// Part 2

//var doText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "do_test_input.txt"));
var doText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "do_input.txt"));

var doMulInstructionsPattern = @"do\(\)|don't\(\)|mul\((\d+,\d+)\)";
var doMulMatches = Regex.Matches(doText, doMulInstructionsPattern);

var part2Pairs = new List<(int, int)>();

var isEnabled = true;

foreach (Match match in doMulMatches)
{
    if (match.Value == "don't()")
    {
        isEnabled = false;
        continue;
    }

    if (match.Value == "do()")
    {
        isEnabled = true;
        continue;
    }

    if (!isEnabled)
    {
        continue;
    }

    var instruction = match.Groups[1].Value;
    var instructionParts = instruction.Split(',').Select(int.Parse).ToArray();
    part2Pairs.Add((instructionParts[0], instructionParts[1]));
}

var part2Result = part2Pairs.Select(x => x.Item1 * x.Item2).Sum();
Console.WriteLine(part2Result);

Console.ReadKey();