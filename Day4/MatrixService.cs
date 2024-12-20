using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day4
{
    public class MatrixService
    {
        public char[,] Matrix { get; set; }

        public string SearchedWord { get; set; }

        public int SearchedWordLength => SearchedWord.Length;

        public char[] SearchedWordCharArray => SearchedWord.ToCharArray();

        public MatrixService(string searchedWord, char[,] matrix)
        {
            SearchedWord = searchedWord;
            Matrix = matrix;
        }

        public void DisplayMatrix(char[,] matrix)
        {
            for (var row = 0; row < matrix.GetLength(0); row++)
            {
                for (var col = 0; col < matrix.GetLength(1); col++)
                {
                    Console.Write(matrix[row, col]);
                }
                Console.WriteLine();
            }
        }

        public int GetOccurencesOfWordHorizontally()
        {
            var occurences = 0;

            for (var row = 0; row < Matrix.GetLength(0); row++)
            {
                for (var col = 0; col <= Matrix.GetLength(1) - SearchedWordLength; col++)
                {
                    var lettersSequence = new char[SearchedWordLength];

                    for (var sequenceCol = col; sequenceCol < col + SearchedWordLength; sequenceCol++)
                    {
                        lettersSequence[sequenceCol - col] = Matrix[row, sequenceCol];
                    }

                    if (SearchedWordCharArray.SequenceEqual(lettersSequence))
                    {
                        occurences++;
                    }
                }
            }

            return occurences;
        }

        public int GetOccurencesOfWordVertically()
        {
            var occurences = 0;

            for (var col = 0; col < Matrix.GetLength(1); col++)
            {
                for (var row = 0; row <= Matrix.GetLength(0) - SearchedWordLength; row++)
                {
                    var lettersSequence = new char[SearchedWordLength];

                    for (var sequenceRow = row; sequenceRow < row + SearchedWordLength; sequenceRow++)
                    {
                        lettersSequence[sequenceRow - row] = Matrix[sequenceRow, col];
                    }

                    if (SearchedWordCharArray.SequenceEqual(lettersSequence))
                    {
                        occurences++;
                    }
                }
            }
            return occurences;
        }
        public int GetOccurencesOfWordDiagonally()
        {
            var diagonalFromTopLeftOccurences = GetOccurencesOfWordDiagonallyStartingFromTopLeft();
            var diagonalFromBottomLeftOccurences = GetOccurencesOfWordDiagonallyStartingFromBottomLeft();
            var diagonalFromTopRightOccurences = GetOccurencesOfWordDiagonallyStartingFromTopRight();
            var diagonalFromBottomRightOccurences = GetOccurencesOfWordDiagonallyStartingFromBottomRight();

            return diagonalFromTopLeftOccurences + diagonalFromBottomLeftOccurences + diagonalFromTopRightOccurences + diagonalFromBottomRightOccurences;
        }

        public int GetOccurencesOfWordDiagonallyStartingFromTopLeft()
        {
            var occurences = 0;

            for (var row = 0; row < Matrix.GetLength(0) - SearchedWordLength + 1; row++)
            {
                for (var col = 0; col < Matrix.GetLength(1) - SearchedWordLength + 1; col++)
                {
                    // [0, 0] -> [1, 1] -> [2, 2] -> [3, 3]
                    var lettersSequence = new char[SearchedWordLength];

                    foreach (var sequenceIndex in Enumerable.Range(0, SearchedWordLength))
                    {
                        lettersSequence[sequenceIndex] = Matrix[sequenceIndex + row, sequenceIndex + col];
                    }

                    if (SearchedWordCharArray.SequenceEqual(lettersSequence))
                    {
                        occurences++;
                    }
                }
            }

            return occurences;
        }

        public int GetOccurencesOfWordDiagonallyStartingFromBottomLeft()
        {
            var occurences = 0;

            var test = Matrix.GetLength(0);

            for (var row = Matrix.GetLength(0) - 1; row >= 0 + SearchedWordLength - 1; row--)
            {
                for (var col = 0; col < Matrix.GetLength(1) - SearchedWordLength + 1; col++)
                {
                    // [3, 0] -> [2, 2] -> [3, 1] -> [0, 3]
                    var lettersSequence = new char[SearchedWordLength];
                    foreach (var sequenceIndex in Enumerable.Range(0, SearchedWordLength))
                    {
                        lettersSequence[sequenceIndex] = Matrix[row - sequenceIndex, col + sequenceIndex];
                    }

                    if (lettersSequence.SequenceEqual(SearchedWordCharArray))
                    {
                        occurences++;
                    }
                }
            }

            return occurences;
        }

        public int GetOccurencesOfWordDiagonallyStartingFromTopRight()
        {
            var occurences = 0;

            for (var row = 0; row < Matrix.GetLength(0) - SearchedWordLength + 1; row++)
            {
                for (var col = Matrix.GetLength(1) - 1; col >= 0 + SearchedWordLength - 1; col--)
                {
                    // [0, 3] -> [1, 2] -> [2, 1] -> [3, 0]
                    var lettersSequence = new char[SearchedWordLength];
                    foreach (var sequenceIndex in Enumerable.Range(0, SearchedWordLength))
                    {
                        lettersSequence[sequenceIndex] = Matrix[row + sequenceIndex, col - sequenceIndex];
                    }

                    if (lettersSequence.SequenceEqual(SearchedWordCharArray))
                    {
                        occurences++;
                    }
                }
            }

            return occurences;
        }

        public int GetOccurencesOfWordDiagonallyStartingFromBottomRight()
        {
            var occurences = 0;

            for (var row = Matrix.GetLength(0) - 1; row >= 0 + SearchedWordLength - 1; row--)
            {
                for (var col = Matrix.GetLength(1) - 1; col >= 0 + SearchedWordLength - 1; col--)
                {
                    // [3, 3] -> [2, 2] -> [1, 1] -> [0, 0]
                    var lettersSequence = new char[SearchedWordLength];
                    foreach (var sequenceIndex in Enumerable.Range(0, SearchedWordLength))
                    {
                        lettersSequence[sequenceIndex] = Matrix[row - sequenceIndex, col - sequenceIndex];
                    }

                    if (lettersSequence.SequenceEqual(SearchedWordCharArray))
                    {
                        occurences++;
                    }
                }
            }

            return occurences;
        }

        public int GetOccurencesOfWordHorizontallyBackward()
        {
            var occurences = 0;

            for (var row = 0; row < Matrix.GetLength(0); row++)
            {
                for (var col = Matrix.GetLength(1) - 1; col >= 0 + SearchedWordLength - 1; col--)
                {
                    var lettersSequence = new char[SearchedWordLength];

                    foreach (var sequenceIndex in Enumerable.Range(0, SearchedWordLength))
                    {
                        lettersSequence[sequenceIndex] = Matrix[row, col - sequenceIndex];
                    }

                    if (SearchedWordCharArray.SequenceEqual(lettersSequence))
                    {
                        occurences++;
                    }
                }
            }

            return occurences;
        }

        public int GetOccurencesOfWordVerticallBackward()
        {
            var occurences = 0;

            for (var row = Matrix.GetLength(0) - 1; row >= 0 + SearchedWordLength - 1; row--)
            {
                for (var col = 0; col <= Matrix.GetLength(1) - 1; col++)
                {
                    var lettersSequence = new char[SearchedWordLength];
                    foreach (var sequenceIndex in Enumerable.Range(0, SearchedWordLength))
                    {
                        lettersSequence[sequenceIndex] = Matrix[row - sequenceIndex, col];
                    }

                    if (lettersSequence.SequenceEqual(SearchedWordCharArray))
                    {
                        occurences++;
                    }
                }
            }

            return occurences;
        }
    }
}
