using Day6;

const char Obstacle = '#';

var indicators = new char[] { (char)Direction.Up, (char)Direction.Down, (char)Direction.Left, (char)Direction.Right };
var horizontalIndicators = new char[] { (char)Direction.Left, (char)Direction.Right };
var verticalIndicators = new char[] { (char)Direction.Up, (char)Direction.Down };

// Part 1

var inputMatrix = GetMatrixFromFile("test_input.txt");
//var inputMatrix = GetMatrixFromFile("input.txt");

var guardMatrix = new char[inputMatrix.GetLength(0), inputMatrix.GetLength(1)];
FillGuardMatrix(guardMatrix, '.', inputMatrix);

GetGuardPositionsInMatrix(inputMatrix, horizontalIndicators, verticalIndicators, guardMatrix, 0);

var guardOccurences = CountOccurencesOfGuardPosition(guardMatrix);

Console.WriteLine($"Guard occured {guardOccurences} times.");

// Part 2

inputMatrix = GetMatrixFromFile("test_input.txt");
//inputMatrix = GetMatrixFromFile("input.txt");

var infitiveLoops = CountInfitiveLoops(inputMatrix);

Console.WriteLine($"Found {infitiveLoops} positions to cause infitive loop.");

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

void FillGuardMatrix(char[,] guardMatrix, char fillChar, char[,] inputMatrix)
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

void GetGuardPositionsInMatrix(char[,] matrix, char[] horizontalIndicators, char[] verticalIndicators, char[,] guardMatrix, int collisionIndex)
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
            if (!indicators.Contains(matrix[row, col]))
            {
                continue;
            }

            var direction = GetDirection(matrix[row, col]);
            var guardPosition = verticalIndicators.Contains(matrix[row, col]) ? row : col;
            var singleColOrRow = verticalIndicators.Contains(matrix[row, col]) ? 
                GetOrderedColumn(matrix, col, direction) : 
                GetOrderedRow(matrix, row, direction);
            collisionIndex = verticalIndicators.Contains(matrix[row, col]) ? 
                GetCollisionIndex(direction, singleColOrRow, row) :
                GetCollisionIndex(direction, singleColOrRow, guardPosition);

            if (collisionIndex == -1)
            {
                MarkGuardPositionWhenLeaving(direction, matrix, guardPosition, row, col);
                break;
            }

            if (verticalIndicators.Contains(matrix[row, col]))
            {                         
                matrix[row, col] = '.';
                var rowWithIndicator = direction == Direction.Down ?
                    collisionIndex - 1 :
                    collisionIndex + 1;

                matrix[rowWithIndicator, col] = GetTurnedIndicatorRight90Degrees(direction);
                var startIndex = direction == Direction.Down ? guardPosition : collisionIndex + 1;
                var endIndex = direction == Direction.Down ? collisionIndex - 1 : guardPosition;
                MarkGuardPositionsInCol(guardMatrix, startIndex, endIndex, col);

                break;
            }

            if (horizontalIndicators.Contains(matrix[row, col]))
            {            
                matrix[row, col] = '.';
                var colWithIndicator = direction == Direction.Right ?
                    collisionIndex - 1 :
                    collisionIndex + 1;
                matrix[row, colWithIndicator] = GetTurnedIndicatorRight90Degrees(direction);
                var startIndex = direction == Direction.Right ? guardPosition : collisionIndex + 1;
                var endIndex = direction == Direction.Right ? collisionIndex - 1 : guardPosition;
                MarkGuardPositionsInRow(guardMatrix, startIndex, endIndex, row);

                break;
            }
        }
    }

    GetGuardPositionsInMatrix(matrix, horizontalIndicators, verticalIndicators, guardMatrix, collisionIndex);
}

void MarkGuardPositionWhenLeaving(Direction direction, char[,] matrix, int guardPosition, int row, int col)
{
    switch (direction)
    {
        case Direction.Down:
            MarkGuardPositionsInCol(guardMatrix, guardPosition, matrix.GetLength(0) - 1, col);
            break;
        case Direction.Up:
            MarkGuardPositionsInCol(guardMatrix, 0, guardPosition, col);
            break;
        case Direction.Left:
            MarkGuardPositionsInRow(guardMatrix, 0, guardPosition, row);
            break;
        case Direction.Right:
            MarkGuardPositionsInRow(guardMatrix, guardPosition, matrix.GetLength(1) - 1, row);
            break;
    }
}

int CountInfitiveLoops(char[,] inputMatrix)
{
    var infitiveLoops = 0;
    var visitedLocations = new List<(int Row, int Col, Direction Direction)>();

    for (var row = 0; row < inputMatrix.GetLength(0); row++)
    {
        for (var col = 0; col < inputMatrix.GetLength(1); col++)
        {
            if (inputMatrix[row, col] == Obstacle)
            {
                continue;
            }

            if (indicators.Contains(inputMatrix[row, col]))
            {
                continue;
            }

            var matrixCopy = (char[,])inputMatrix.Clone();
            matrixCopy[row, col] = Obstacle;
            Console.WriteLine($"Obstacle placed [{row}, {col}].");
            if (IsCausingInfitiveLoop(matrixCopy, horizontalIndicators, verticalIndicators, 0, visitedLocations))
            {
                infitiveLoops++;
            }
            visitedLocations.Clear();
        }
    }

    return infitiveLoops;
}

bool IsCausingInfitiveLoop(char[,] matrixCopy, char[] horizontalIndicators, char[] verticalIndicators, int collisionIndex, List<(int Row, int Col, Direction Direction)> visitedLocations)
{
    if (collisionIndex == -1)
    {        
        return false;
    }

    for (var row = 0; row < matrixCopy.GetLength(0); row++)
    {
        for (var col = 0; col < matrixCopy.GetLength(1); col++)
        {
            if (!indicators.Contains(matrixCopy[row, col])) 
            {
                continue;
            }

            var direction = GetDirection(matrixCopy[row, col]);
            var alreadyVisited = visitedLocations.Any(x => x.Row == row && x.Col == col && x.Direction == direction);
            if (alreadyVisited)
            {
                return true;
            }
            else
            {
                visitedLocations.Add((row, col, direction));
            }

            if (verticalIndicators.Contains(matrixCopy[row, col]))
            {           
                var singleCol = GetOrderedColumn(matrixCopy, col, direction);
                collisionIndex = GetCollisionIndex(direction, singleCol, row);
                if (collisionIndex == -1)
                {      
                    break;
                }

                matrixCopy[row, col] = '.';
                var rowWithIndicator = direction == Direction.Down ?
                    collisionIndex - 1 :
                    collisionIndex + 1;

                matrixCopy[rowWithIndicator, col] = GetTurnedIndicatorRight90Degrees(direction);
                break;
            }

            if (horizontalIndicators.Contains(matrixCopy[row, col]))
            {
                var guardPosition = col;               
                var singleRow = GetOrderedRow(matrixCopy, row, direction);
                collisionIndex = GetCollisionIndex(direction, singleRow, guardPosition);

                if (collisionIndex == -1)
                {
                    break;
                }

                matrixCopy[row, col] = '.';
                var colWithIndicator = direction == Direction.Right ?
                    collisionIndex - 1 :
                    collisionIndex + 1;
                matrixCopy[row, colWithIndicator] = GetTurnedIndicatorRight90Degrees(direction);            
                break;
            }           
        }
    }

    return IsCausingInfitiveLoop(matrixCopy, horizontalIndicators, verticalIndicators, collisionIndex, visitedLocations);
}

Direction GetDirection(char indicator)
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

char[] GetOrderedColumn(char[,] matrix, int col, Direction direction)
{
    var column = new char[matrix.GetLength(0)];
    for (var row = 0; row < matrix.GetLength(0); row++)
    {
        column[row] = matrix[row, col];
    }

    return column;
}

char[] GetOrderedRow(char[,] matrix, int row, Direction direction)
{
    var rowArray = new char[matrix.GetLength(1)];
    for (var col = 0; col < matrix.GetLength(1); col++)
    {
        rowArray[col] = matrix[row, col];
    }

    return rowArray;
}

int GetCollisionIndex(Direction direction, char[] rowOrCol, int guardPosition)
{
    var elementsToCheck = new List<char>();
    var obstacleIndex = -1;
    var downAndRightDirections = new Direction[] { Direction.Down, Direction.Right };

    if (downAndRightDirections.Contains(direction))
    {
        elementsToCheck = rowOrCol.Skip(guardPosition).ToList();
        obstacleIndex = elementsToCheck.FindIndex(x => x == Obstacle);
    }
    else
    {
        elementsToCheck = rowOrCol.Take(guardPosition + 1).ToList();
        obstacleIndex = elementsToCheck.FindLastIndex(x => x == Obstacle);
    }

    if (obstacleIndex == -1)
    {
        return obstacleIndex;
    }

    return downAndRightDirections.Contains(direction) ? 
        obstacleIndex + guardPosition : 
        obstacleIndex;    
}

void MarkGuardPositionsInCol(char[,] guardMatrix, int startIndex, int endIndex, int collisionCol)
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
}

void MarkGuardPositionsInRow(char[,] guardMatrix, int startIndex, int endIndex, int collisionRow)
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

char GetTurnedIndicatorRight90Degrees(Direction direction)
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
int CountOccurencesOfGuardPosition(char[,] guardMatrix)
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