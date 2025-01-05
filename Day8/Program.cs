
using Day8;
using System.Diagnostics;

//var matrix = GetMatrixFromFile("test_input.txt");
var matrix = GetMatrixFromFile("input.txt");

// Part 1

var stopwatch = new Stopwatch();
stopwatch.Start();

var antennaPositions = GetAntennaPositions(matrix);

var distanceBetweenTwoAntennas = GetDistanceBetweenParitcularAntennas(antennaPositions);

var antinodes = new char[matrix.GetLength(0), matrix.GetLength(1)];
FillMatrix(antinodes, '.');

foreach (var distance in distanceBetweenTwoAntennas)
{
    TryToPlaceAntinodePart1(antinodes, distance);
}

var antinodeOccurences = CountAntinodes(antinodes);

Console.WriteLine($"There are {antinodeOccurences} antinodes.");

// Part 2

var antinodes2 = new char[matrix.GetLength(0), matrix.GetLength(1)];

FillMatrix(antinodes2, '.');

foreach (var distance in distanceBetweenTwoAntennas)
{
    TryToPlaceAntinodePart2(antinodes2, distance);
}

AddAntennasAsAntinodes(antinodes2, antennaPositions);

var antinodeOccurencesPart2 = CountAntinodes(antinodes2);

Console.WriteLine($"There are {antinodeOccurencesPart2} antinodes.");

stopwatch.Stop();

Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} ms");

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

List<AntennaPosition> GetAntennaPositions(char[,] matrix)
{
    var antennaPositions = new List<AntennaPosition>();

    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            if (matrix[row, col] == '.')
            {
                continue;
            }

            antennaPositions.Add(new AntennaPosition(row, col, matrix[row, col]));
        }
    }

    return antennaPositions;
}

List<DistanceBetweenTwoAntennas> GetDistanceBetweenParitcularAntennas(List<AntennaPosition> antennaPositions)
{
    var groupedBySign = antennaPositions.GroupBy(x => x.Sign);

    var distanceBetweenTwoAntennas = new List<DistanceBetweenTwoAntennas>();

    foreach (var group in groupedBySign)
    {
        for (var i = 0; i < group.Count() - 1; i++)
        {
            var elements = group.Skip(i + 1).Take(group.Count() - 1);

            foreach (var element in elements)
            {
                var antennaPosition = group.Select(x => x).ToList()[i];
                distanceBetweenTwoAntennas.Add(GetDistance(antennaPosition, element));
            }
        }
    }

    return distanceBetweenTwoAntennas;
}
DistanceBetweenTwoAntennas GetDistance(AntennaPosition firstPosition, AntennaPosition secondPosition)
{
    return new DistanceBetweenTwoAntennas
    {
        VerticalDirection = firstPosition.Row > secondPosition.Row ? Direction.Up :
                           firstPosition.Row < secondPosition.Row ? Direction.Down :
                           Direction.SameLine,
        VerticalDistance = Math.Abs(secondPosition.Row - firstPosition.Row),
        HorizontalDirection = firstPosition.Column > secondPosition.Column ? Direction.Left :
                             firstPosition.Column < secondPosition.Column ? Direction.Right :
                             Direction.SameLine,
        HorizontalDistance = Math.Abs(secondPosition.Column - firstPosition.Column),
        FirstAntennaRow = firstPosition.Row,
        FirstAntennaColumn = firstPosition.Column,
        SecondAntennaRow = secondPosition.Row,
        SecondAntennaColumn = secondPosition.Column
    };
}

void TryToPlaceAntinodePart1(char[,] antinodes, DistanceBetweenTwoAntennas distanceBetweenTwoAntennas)
{
    var firstAntennaAntinode = new Antinode();
    var secondAntennaAntinode = new Antinode();

    GetAntinodeRows(distanceBetweenTwoAntennas, firstAntennaAntinode, secondAntennaAntinode);
    GetAntinodeColumns(distanceBetweenTwoAntennas, firstAntennaAntinode, secondAntennaAntinode);

    var firstAntennaAntinodeRowInMatrix = firstAntennaAntinode.Row >= 0 && firstAntennaAntinode.Row < matrix.GetLength(0);
    var firstAntennaAntinodeColumnInMatrix = firstAntennaAntinode.Column >= 0 && firstAntennaAntinode.Column < matrix.GetLength(1);
    if (firstAntennaAntinodeRowInMatrix && firstAntennaAntinodeColumnInMatrix)
    {
        antinodes[firstAntennaAntinode.Row, firstAntennaAntinode.Column] = '#';
    }

    var secondAntennaAntinodeRowInMatrix = secondAntennaAntinode.Row >= 0 && secondAntennaAntinode.Row < matrix.GetLength(0);
    var secondAntennaAntinodeColumnInMatrix = secondAntennaAntinode.Column >= 0 && secondAntennaAntinode.Column < matrix.GetLength(1);
    if (secondAntennaAntinodeRowInMatrix && secondAntennaAntinodeColumnInMatrix)
    {
        antinodes[secondAntennaAntinode.Row, secondAntennaAntinode.Column] = '#';
    }
}

void TryToPlaceAntinodePart2(char[,] antinodes, DistanceBetweenTwoAntennas distanceBetweenTwoAntennas)
{
    var firstAntennaAntinode = new Antinode();
    var secondAntennaAntinode = new Antinode();

    GetAntinodeRows(distanceBetweenTwoAntennas, firstAntennaAntinode, secondAntennaAntinode);
    GetAntinodeColumns(distanceBetweenTwoAntennas, firstAntennaAntinode, secondAntennaAntinode);

    var firstAntennaAntinodeRowInMatrix = firstAntennaAntinode.Row >= 0 && firstAntennaAntinode.Row < matrix.GetLength(0);
    var firstAntennaAntinodeColumnInMatrix = firstAntennaAntinode.Column >= 0 && firstAntennaAntinode.Column < matrix.GetLength(1);
    var firstAntennaAntinodeInMatrix = firstAntennaAntinodeRowInMatrix && firstAntennaAntinodeColumnInMatrix;
    var secondAntennaAntinodeRowInMatrix = secondAntennaAntinode.Row >= 0 && secondAntennaAntinode.Row < matrix.GetLength(0);
    var secondAntennaAntinodeColumnInMatrix = secondAntennaAntinode.Column >= 0 && secondAntennaAntinode.Column < matrix.GetLength(1);
    var secondAntennaAntinodeInMatrix = secondAntennaAntinodeRowInMatrix && secondAntennaAntinodeColumnInMatrix;
    if (firstAntennaAntinodeInMatrix)
    {
        antinodes[firstAntennaAntinode.Row, firstAntennaAntinode.Column] = '#';
    }

    if (secondAntennaAntinodeInMatrix)
    {
        antinodes[secondAntennaAntinode.Row, secondAntennaAntinode.Column] = '#';
    }

    if (!firstAntennaAntinodeInMatrix && !secondAntennaAntinodeInMatrix)
    {
        return;
    }

    distanceBetweenTwoAntennas.FirstAntennaRow = firstAntennaAntinode.Row;
    distanceBetweenTwoAntennas.FirstAntennaColumn = firstAntennaAntinode.Column;
    distanceBetweenTwoAntennas.SecondAntennaRow = secondAntennaAntinode.Row;
    distanceBetweenTwoAntennas.SecondAntennaColumn = secondAntennaAntinode.Column;

    TryToPlaceAntinodePart2(antinodes, distanceBetweenTwoAntennas);
}
 

void GetAntinodeRows(DistanceBetweenTwoAntennas distanceBetweenTwoAntennas, Antinode firstAntennaAntinode, Antinode secondAntennaAntinode)
{
    switch (distanceBetweenTwoAntennas.VerticalDirection)
    {
        case Direction.Up:
            firstAntennaAntinode.Row = -1 * distanceBetweenTwoAntennas.VerticalDistance - distanceBetweenTwoAntennas.FirstAntennaRow;

            secondAntennaAntinode.Row = distanceBetweenTwoAntennas.VerticalDistance - distanceBetweenTwoAntennas.SecondAntennaRow;

            break;
        case Direction.Down:
            firstAntennaAntinode.Row = -1 * distanceBetweenTwoAntennas.VerticalDistance + distanceBetweenTwoAntennas.FirstAntennaRow;

            secondAntennaAntinode.Row = distanceBetweenTwoAntennas.VerticalDistance + distanceBetweenTwoAntennas.SecondAntennaRow;

            break;

        case Direction.SameLine:
            firstAntennaAntinode.Row = distanceBetweenTwoAntennas.FirstAntennaRow;

            secondAntennaAntinode.Row = distanceBetweenTwoAntennas.SecondAntennaRow;
            break;
    }
}

void GetAntinodeColumns(DistanceBetweenTwoAntennas distanceBetweenTwoAntennas, Antinode firstAntennaAntinode, Antinode secondAntennaAntinode)
{
    switch (distanceBetweenTwoAntennas.HorizontalDirection)
    {
        case Direction.Left:
            firstAntennaAntinode.Column = distanceBetweenTwoAntennas.FirstAntennaColumn + distanceBetweenTwoAntennas.HorizontalDistance;

            secondAntennaAntinode.Column = distanceBetweenTwoAntennas.SecondAntennaColumn + (-1 * distanceBetweenTwoAntennas.HorizontalDistance);
            break;

        case Direction.Right:
            firstAntennaAntinode.Column = distanceBetweenTwoAntennas.FirstAntennaColumn - distanceBetweenTwoAntennas.HorizontalDistance;

            secondAntennaAntinode.Column = distanceBetweenTwoAntennas.SecondAntennaColumn - ((-1) * distanceBetweenTwoAntennas.HorizontalDistance);

            break;

        case Direction.SameLine:
            firstAntennaAntinode.Column = distanceBetweenTwoAntennas.FirstAntennaColumn;

            secondAntennaAntinode.Column = distanceBetweenTwoAntennas.SecondAntennaColumn;

            break;
    }
}

void FillMatrix(char[,] matrix, char fillChar)
{
    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            matrix[row, col] = fillChar;
        }
    }
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

int CountAntinodes(char[,] antinodes)
{
    var antinodeOccurences = 0;

    for (var row = 0; row < antinodes.GetLength(0); row++)
    {
        for (var col = 0; col < antinodes.GetLength(1); col++)
        {
            if (antinodes[row, col] == '#')
            {
                antinodeOccurences++;
            }
        }
    }

    return antinodeOccurences;
}

void AddAntennasAsAntinodes(char[,] antinodes, List<AntennaPosition> antennaPositions)
{
    for (var row = 0; row < antinodes.GetLength(0); row++)
    {
        for (var col = 0; col < antinodes.GetLength(1); col++)
        {
            foreach (var antennaPosition in antennaPositions)
            {
                if (antennaPosition.Row == row && antennaPosition.Column == col)
                {
                    antinodes[row, col] = '#';
                }
            }
        }
    }
}