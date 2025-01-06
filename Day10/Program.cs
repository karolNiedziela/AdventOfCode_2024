using Day10;

const int HighestPosition = 9;

//var inputMatrix = GetMatrixFromFile("test_input.txt");
var inputMatrix = GetMatrixFromFile("input.txt");

DisplayMatrix(inputMatrix);

var startingPoints = GetStartingPoints(inputMatrix);
var score = 0;

var allReacheableHighestPoints = new List<Point>();

foreach (var startingPoint in startingPoints)
{
    var reacheableHighestPoints = GetReacheableHighestPoints(inputMatrix, 1, startingPoint);

    var distinctReacheableHighestPoints = reacheableHighestPoints.Distinct().ToList();

    score += distinctReacheableHighestPoints.Count;

    allReacheableHighestPoints.AddRange(reacheableHighestPoints);
}

Console.WriteLine($"Part 1: Total score: {score}");

Console.WriteLine($"Part 2: Total rating: {allReacheableHighestPoints.Count}");

Console.ReadKey();

char[,] GetMatrixFromFile(string fileName)
{
    var lines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), fileName));
    var matrix = new char[lines.Length, lines[0].Length];

    for (var row = 0; row < lines.Length; row++)
    {
        var charArray = lines[row].ToCharArray();
        for (var col = 0; col < charArray.Length; col++)
        {
            matrix[row, col] = charArray[col];
        }
    }

    return matrix;
}

void DisplayMatrix(char[,] matrix)
{
    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            Console.Write(matrix[row, col]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

List<Point> GetStartingPoints(char[,] matrix) 
{
    var startingPoints = new List<Point>();

    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            if (matrix[row, col] == '0')
            {
                startingPoints.Add(new Point(row, col, matrix[row, col]));
            }
        }
    }

    return startingPoints;
}

List<Point> GetReacheableHighestPoints(char[,] matrix, int searchedValue, Point startingPoint)
{
    var adjacentPoints = new List<Point>();

    var totalRows = matrix.GetLength(0);
    var totalCols = matrix.GetLength(1);

    var movementDirections = new (int RowOffset, int ColOffset)[]
    {
        (-1, 0), // Up
        (1, 0),  // Down
        (0, -1), // Left
        (0, 1)   // Right
    };

    foreach (var (rowOffset, colOffset) in movementDirections)
    {
        var newRow = startingPoint.Row + rowOffset;
        var newCol = startingPoint.Column + colOffset;

        if (newRow >= 0 && newRow < totalRows && newCol >= 0 && newCol < totalCols)
        {
            adjacentPoints.Add(new Point(newRow, newCol, matrix[newRow, newCol]));
        }
    }

    var searchedValueAsChar = char.Parse(searchedValue.ToString());
    var pointsWithSearchedValue = adjacentPoints.Where(x => x.Value == searchedValueAsChar).ToList();
    if (pointsWithSearchedValue.Count == 0)
    {
        return[];
    }

    var validPaths = new List<Point>();

    if (searchedValue == HighestPosition)
    {
        validPaths.AddRange(pointsWithSearchedValue);
    }
    else
    {
        foreach (var point in pointsWithSearchedValue)
        {
            validPaths.AddRange(GetReacheableHighestPoints(matrix, searchedValue + 1, point));
        }
    }

    return validPaths;
}