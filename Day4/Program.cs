var matrix = GetMatrixFromFile();

DisplayMatrix(matrix);

static char[,] GetMatrixFromFile()
{
    var lines = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "test_input.txt"));
    var matrix = new char[lines.Length, lines[0].Length];

    for (int row = 0; row < lines.Length; row++)
    {
        var charArray = lines[row].ToCharArray();
        for (int col = 0; col < charArray.Length; col++)
        {
            matrix[row, col] = charArray[col];
        }
    }

    return matrix;
}

static void DisplayMatrix(char[,] matrix)
{
    for (int row = 0; row < matrix.GetLength(0); row++)
    {
        for (int col = 0; col < matrix.GetLength(1); col++)
        {
            Console.Write(matrix[row, col]);
        }
        Console.WriteLine();
    }
}