//var input = ReadInput("test_input.txt");
var input = ReadInput("input.txt");

// Part1

var part1Result = ResolvePart1(input);

Console.WriteLine($"[PART 1] Sum of middle elements: {part1Result}");

// Part 2

var part2Result = ResolvePart2(input);

Console.WriteLine($"[PART 2] Sum of middle elements: {part2Result}");

Console.ReadKey();

static (List<(int OrderingRule, int Update)> PageOrderingRules, List<List<int>> Updates) ReadInput(string fileName)
{
    var allText = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName));

    var sections = allText.Split(["\r\n\r\n"], StringSplitOptions.RemoveEmptyEntries);
    var firstSectionLines = sections[0].Split(["\r\n"], StringSplitOptions.RemoveEmptyEntries);

    var pageOrderingRules = new List<(int OrderingRule, int Update)>();

    foreach (var line in firstSectionLines)
    {
        var parts = line.Split('|');
        pageOrderingRules.Add((int.Parse(parts[0].Trim()), int.Parse(parts[1].Trim())));
    }

    var secondSectionLines = sections[1].Split(["\r\n"], StringSplitOptions.RemoveEmptyEntries);

    var updates = new List<List<int>>();
    foreach (var line in secondSectionLines)
    {
        var pageNumbers = line.Split(',').Select(x => int.Parse(x.Trim())).ToList();
        updates.Add(pageNumbers);
    }

    return (pageOrderingRules, updates);
}

static int ResolvePart1((List<(int OrderingRule, int Update)> PageOrderingRules, List<List<int>> Updates) input)
{
    var isValidOrder = true;

    var validUpdates = new List<List<int>>();

    foreach (var update in input.Updates)
    {
        for (var i = 1; i < update.Count; i++)
        {
            var previousPages = update.Take(i).ToList();

            foreach (var previousPage in previousPages)
            {
                var hasInvalidOrder = input.PageOrderingRules.Any(rule => rule.OrderingRule == update[i] && rule.Update == previousPage);
                if (hasInvalidOrder)
                {
                    isValidOrder = false;
                    break;
                }
            }

            if (!isValidOrder)
            {
                break;
            }
        }

        if (isValidOrder)
        {
            validUpdates.Add(update);
        }

        isValidOrder = true;
    }

    return GetSumOfMiddleElements(validUpdates);
}

static int ResolvePart2((List<(int OrderingRule, int Update)> PageOrderingRules, List<List<int>> Updates) input)
{
    var reorderedInvalidUpdates = new List<List<int>>();

    foreach (var update in input.Updates)
    {
        var reorderedUpdate = new List<int>(update);
        var invalidOrder = false;

        for (var i = 1; i < update.Count; i++)
        {
            var previousPages = update.Take(i).ToList();

            foreach (var previousPage in previousPages)
            {
                var invalidItem = input.PageOrderingRules.Any(rule => rule.OrderingRule == update[i] && rule.Update == previousPage);
                if (invalidItem)
                {
                    invalidOrder = true;
                    break;
                }
            }
        }

        if (invalidOrder)
        {
            reorderedUpdate = ReorderUpdate(reorderedUpdate, input.PageOrderingRules);
            reorderedInvalidUpdates.Add(reorderedUpdate);
        }

        invalidOrder = false;
    }

    return GetSumOfMiddleElements(reorderedInvalidUpdates);
}

static List<int> ReorderUpdate(List<int> update, List<(int OrderingRule, int Update)> pageOrderingRules)
{
    var orderedUpdate = new List<int>(update);

    for (int i = 0; i < orderedUpdate.Count - 1; i++)
    {
        for (int j = i + 1; j < orderedUpdate.Count; j++)
        {
            var pageOrderingRule = pageOrderingRules.FirstOrDefault(rule => rule.OrderingRule == orderedUpdate[j] && rule.Update == orderedUpdate[i]);
            if (pageOrderingRule != default)
            {
                Swap(orderedUpdate, i, j);
            }
        }
    }

    return orderedUpdate;
}

static void Swap<T>(List<T> list, int index1, int index2)
{
    T temp = list[index1];
    list[index1] = list[index2];
    list[index2] = temp;
}

static int GetSumOfMiddleElements(List<List<int>> updates)
{
    var middleElements = updates.Select(seq => seq[seq.Count / 2]).ToList();
    var sumOfMiddleElements = middleElements.Sum();
    return sumOfMiddleElements;
}