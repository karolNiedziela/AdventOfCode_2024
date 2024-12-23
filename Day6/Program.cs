using Day6;

const char Obstacle = '#';

//var matrix = GetMatrixFromFile("test_input.txt");
var matrix = GetMatrixFromFile("input.txt");

var guardMatrix = new char[matrix.GetLength(0), matrix.GetLength(1)];
FillGuardMatrix(guardMatrix, '.', matrix);

var horizontalIndicators = new char[] { (char)Direction.Left, (char)Direction.Right };
var verticalIndicators = new char[] { (char)Direction.Up, (char)Direction.Down };

GetGuardPositionsInMatrix(matrix, horizontalIndicators, verticalIndicators, guardMatrix, 0);

var guardOccurences = CountOccurencesOfGuardPosition(guardMatrix);

Console.WriteLine($"Guard occured {guardOccurences} times.");

Console.ReadKey();

static char[,] GetMatrixFromFile(string fileName)
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

static void FillGuardMatrix(char[,] guardMatrix, char fillChar, char[,] inputMatrix)
{
    for (var row = 0; row < guardMatrix.GetLength(0); row++)
    {
        for (var col = 0; col < guardMatrix.GetLength(1); col++)
        {
            guardMatrix[row, col] = fillChar;

            if (inputMatrix[row, col] == Obstacle)
            {
                guardMatrix[row, col] = Obstacle;
            }
        }
    }
}

static void GetGuardPositionsInMatrix(char[,] matrix, char[] horizontalIndicators, char[] verticalIndicators, char[,] guardMatrix, int collisionIndex)
{
    if (collisionIndex == -1)
    {
        DisplayMatrix(guardMatrix);
        return;
    }

    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        for (var col = 0; col < matrix.GetLength(1); col++)
        {
            if (verticalIndicators.Contains(matrix[row, col]))
            {
                var guardPosition = row;
                var direction = GetDirection(matrix[row, col]);
                var singleCol = GetOrderedColumn(matrix, col, direction);
                collisionIndex = GetCollisionIndex(direction, singleCol, row);

                if (collisionIndex == -1)
                {
                    if (direction == Direction.Down)
                    {
                        MarkGuardPositionsInCol(guardMatrix, guardPosition, matrix.GetLength(0) - 1, col);
                    }
                    else
                    {
                        MarkGuardPositionsInCol(guardMatrix, 0, guardPosition, col);
                    }

                    break;
                }

                if (direction == Direction.Down)
                {
                    MarkGuardPositionsInCol(guardMatrix, guardPosition, collisionIndex - 1, col);
                    matrix[row, col] = '.';
                    matrix[collisionIndex - 1, col] = GetTurnedIndicatorRight90Degrees(direction);
                }
                else
                {
                    MarkGuardPositionsInCol(guardMatrix, collisionIndex + 1, guardPosition, col);
                    matrix[row, col] = '.';
                    matrix[collisionIndex + 1, col] = GetTurnedIndicatorRight90Degrees(direction);
                }

                CountOccurencesOfGuardPosition(guardMatrix);
                DisplayMatrix(matrix);
                break;
            }

            if (horizontalIndicators.Contains(matrix[row, col]))
            {
                var guardPosition = col;
                var direction = GetDirection(matrix[row, col]);
                var singleRow = GetOrderedRow(matrix, row, direction);
                collisionIndex = GetCollisionIndex(direction, singleRow, guardPosition);

                if (collisionIndex == -1)
                {
                    if (direction == Direction.Right)
                    {
                        MarkGuardPositionsInRow(guardMatrix, guardPosition, matrix.GetLength(1) - 1, row);
                    }
                    else
                    {
                        MarkGuardPositionsInRow(guardMatrix, 0, guardPosition, row);
                    }

                    break;
                }

                if (direction == Direction.Right)
                {
                    MarkGuardPositionsInRow(guardMatrix, guardPosition, collisionIndex - 1, row);
                    matrix[row, col] = '.';
                    matrix[row, collisionIndex - 1] = GetTurnedIndicatorRight90Degrees(direction);
                }
                else
                {
                    MarkGuardPositionsInRow(guardMatrix, collisionIndex + 1, guardPosition, row);
                    matrix[row, col] = '.';
                    matrix[row, collisionIndex + 1] = GetTurnedIndicatorRight90Degrees(direction);
                }

                CountOccurencesOfGuardPosition(guardMatrix);
                DisplayMatrix(matrix);
                break;
            }
        }
    }

    GetGuardPositionsInMatrix(matrix, horizontalIndicators, verticalIndicators, guardMatrix, collisionIndex);
}


static Direction GetDirection(char indicator)
{
    return indicator switch
    {
        (char)Direction.Up => Direction.Up,
        (char)Direction.Down => Direction.Down,
        (char)Direction.Left => Direction.Left,
        (char)Direction.Right => Direction.Right,
        _ => throw new ArgumentException("Invalid direction indicator")
    };
}

static char[] GetOrderedColumn(char[,] matrix, int col, Direction direction)
{
    var column = new char[matrix.GetLength(0)];
    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        column[row] = matrix[row, col];
    }

    return column;
}

static char[] GetOrderedRow(char[,] matrix, int row, Direction direction)
{
    var rowArray = new char[matrix.GetLength(1)];
    for (var col = 0; col < matrix.GetLength(1); col++)
    {
        rowArray[col] = matrix[row, col];
    }

    return rowArray;
}

static int GetCollisionIndex(Direction direction, char[] rowOrCol, int guardPosition)
{
    if (direction == Direction.Down)
    {
        var elementsToCheck = rowOrCol.Skip(guardPosition).ToList();
        var index = elementsToCheck.FindIndex(x => x == Obstacle);
        if (index == -1)
        {
            return index;
        }

        return index + guardPosition;
    }

    if (direction == Direction.Up)
    {
        var elementsToCheck = rowOrCol.Take(guardPosition + 1).ToList();
        var index = elementsToCheck.FindLastIndex(x => x == Obstacle);
        if (index == -1)
        {
            return index;
        }

        return index;
    }

    if (direction == Direction.Right)
    {
        var elementsToCheck = rowOrCol.Skip(guardPosition).ToList();
        var index = elementsToCheck.FindIndex(x => x == Obstacle);

        if (index == -1)
        {
            return index;
        }

        return index + guardPosition;
    }

    if (direction == Direction.Left)
    {
        var elementsToCheck = rowOrCol.Take(guardPosition + 1).ToList();
        var index = elementsToCheck.FindLastIndex(x => x == Obstacle);
        if (index == -1)
        {
            return index;
        }

        return index;
    }

    return -1;
}

static void MarkGuardPositionsInCol(char[,] guardMatrix, int startIndex, int endIndex, int collisionCol)
{
    for (var row = 0; row < guardMatrix.GetLength(0); row++)
    {
        for (var col = 0; col < guardMatrix.GetLength(1); col++)
        {
            if (col == collisionCol && row >= startIndex && row <= endIndex)
            {
                guardMatrix[row, col] = 'X';
            }
        }
    }

    DisplayMatrix(guardMatrix);
}

static void MarkGuardPositionsInRow(char[,] guardMatrix, int startIndex, int endIndex, int collisionRow)
{
    for (var row = 0; row < guardMatrix.GetLength(0); row++)
    {
        for (var col = 0; col < guardMatrix.GetLength(1); col++)
        {
            if (row == collisionRow && col >= startIndex && col <= endIndex)
            {
                guardMatrix[row, col] = 'X';
            }
        }
    }
    DisplayMatrix(guardMatrix);
}
static void DisplayMatrix(char[,] matrix)
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

static char GetTurnedIndicatorRight90Degrees(Direction direction)
{
    return direction switch
    {
        Direction.Up => (char)Direction.Right,
        Direction.Down => (char)Direction.Left,
        Direction.Left => (char)Direction.Up,
        Direction.Right => (char)Direction.Down,
        _ => throw new ArgumentException("Invalid direction")
    };
}
static int CountOccurencesOfGuardPosition(char[,] guardMatrix)
{
    var guardOccurences = 0;

    for (var row = 0; row < guardMatrix.GetLength(0); row++)
    {
        for (var col = 0; col < guardMatrix.GetLength(1); col++)
        {
            if (guardMatrix[row, col] == 'X')
            {
                guardOccurences++;
            }
        }
    }

    Console.WriteLine(guardOccurences);
    return guardOccurences;
}