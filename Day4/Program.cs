using Day4;

// Part 1

var searchedWord = "XMAS";

var totalOccurencesPart1 = Part1GetTotalOccurences(searchedWord);

Console.WriteLine($"Total occurences of the word '{searchedWord}' in the matrix: {totalOccurencesPart1}");

// Part 2

var totalOccurencesPart2 = GetXMASInXShapeOccurences();

Console.WriteLine($"Total occurences of the word 'MAS/SAM' in the matrix: {totalOccurencesPart2}");

Console.ReadKey();

 static int Part1GetTotalOccurences(string searchedWord)
{
    var matrix = GetMatrixFromFile("input.txt");

    var matrixService = new MatrixService(searchedWord, matrix);

    var horizontalOccurences = matrixService.GetOccurencesOfWordHorizontally();
    var verticalOccurences = matrixService.GetOccurencesOfWordVertically();
    var diagonalOccurences = matrixService.GetOccurencesOfWordDiagonally();
    var backwardHorizontalOccurences = matrixService.GetOccurencesOfWordHorizontallyBackward();
    var backwardVerticalOccurences = matrixService.GetOccurencesOfWordVerticallBackward();

    var totalOccurences = horizontalOccurences + verticalOccurences + diagonalOccurences + backwardHorizontalOccurences + backwardVerticalOccurences;

    return totalOccurences;
}

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

static int GetXMASInXShapeOccurences()
{
    var matrix = GetMatrixFromFile("input_2.txt");

    var occurences = 0;

    for (var row = 0; row < matrix.GetLength(0) - 2; row++)
    {
        for (var col = 0; col < matrix.GetLength(1) - 2; col++)
        {
            var lettersSequence = new char[3, 3];

            foreach (var sequenceRow in Enumerable.Range(0, 3))
            {
                foreach (var sequenceCol in Enumerable.Range(0, 3))
                {
                    lettersSequence[sequenceRow, sequenceCol] = matrix[sequenceRow + row, sequenceCol + col];
                }
            }

           var MAS = new MatrixService("MAS", lettersSequence);
           var SAM = new MatrixService("SAM", lettersSequence);

            var MASDiagonalOccurences = MAS.GetOccurencesOfWordDiagonally();
            var SAMDiagonalOccurences = SAM.GetOccurencesOfWordDiagonally();

            if (MASDiagonalOccurences == 2 || SAMDiagonalOccurences == 2)
            {
                occurences++;
            }
        }
    }

    return occurences;
}
