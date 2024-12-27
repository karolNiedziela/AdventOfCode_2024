using Day7;

//var equations = GetInputData("test_input.txt");
var equations = GetInputData("input.txt");

// Part 1

var part1 = Part1GetValidTestValues(equations);

var sumOfPart1 = part1.Sum();

Console.WriteLine($"Part1: {sumOfPart1}");

// Part 2

var part2 = Part2GetValidTestValues(equations);

var sumOfPart2 = part2.Sum();

Console.WriteLine($"Part2: {sumOfPart2}");

Console.ReadKey();

List<Equation> GetInputData(string fileName)
{
    var lines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), fileName));

    var equations = new List<Equation>();

    foreach (var line in lines)
    {
        var expectedValueAndValues = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
        var expectedValue = long.Parse(expectedValueAndValues[0]);
        var numbers = expectedValueAndValues[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).ToList().Select(long.Parse).ToList();
        equations.Add(new Equation(expectedValue, numbers));
    }

    return equations;
}

List<long> Part1GetValidTestValues(List<Equation> equations)
{
    var mathOperators = new List<char> { '+', '*' };
    var validTestValues = new List<long>();

    foreach (var equation in equations)
    {
        var mathOperatorPermutations = GetCombinations(mathOperators, equation.Numbers.Count - 1);

        foreach (var permutation in mathOperatorPermutations)
        {
            var result = equation.Numbers[0];
            for (var i = 0; i < permutation.Count; i++)
            {
                if (permutation[i] == '+')
                {
                    result = result + equation.Numbers[i + 1];
                }
                if (permutation[i] == '*')
                {
                    result = result * equation.Numbers[i + 1];
                }
            }
            if (result == equation.ExpectedResult)
            {
                validTestValues.Add(equation.ExpectedResult);
                break;
            }
        }
    }

    return validTestValues;
}

List<long> Part2GetValidTestValues(List<Equation> equations)
{
    var operators = new List<string> { "+", "*", "||" };
    var validTestValues = new List<long>();

    foreach (var equation in equations)
    {
        var mathOperatorPermutations = GetCombinations(operators, equation.Numbers.Count - 1);

        foreach (var permutation in mathOperatorPermutations)
        {
            var result = equation.Numbers[0];
            for (var i = 0; i < permutation.Count; i++)
            {
                if (permutation[i] == "+")
                {
                    result = result + equation.Numbers[i + 1];
                }
                if (permutation[i] == "*")
                {
                    result = result * equation.Numbers[i + 1];
                }
                if (permutation[i] == "||")
                {
                    result = long.Parse(result.ToString() + equation.Numbers[i + 1].ToString());
                }

            }
            if (result == equation.ExpectedResult)
            {
                validTestValues.Add(equation.ExpectedResult);
                break;
            }
        }
    }

    return validTestValues;
}

List<List<T>> GetCombinations<T>(List<T> list, int length)
{
    var combinations = new List<List<T>> { new List<T>() };

    for (int i = 0; i < length; i++)
    {
        var newCombinations = new List<List<T>>();

        foreach (var combination in combinations)
        {
            foreach (var item in list)
            {
                var newCombination = new List<T>(combination) { item };
                newCombinations.Add(newCombination);
            }
        }

        combinations = newCombinations;
    }

    return combinations;
}
