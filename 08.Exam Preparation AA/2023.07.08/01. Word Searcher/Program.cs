namespace _01._Word_Searcher
{
    using System;
    using System.Linq;

    internal class Program
    {
        private static char[,] matrix;

        static void Main(string[] args)
        {
            int rows = int.Parse(Console.ReadLine()!);
            int cols = int.Parse(Console.ReadLine()!);

            matrix = new char[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                string rowStr = Console.ReadLine()!;

                for (int col = 0; col < cols; col++)
                {
                    matrix[row, col] = rowStr[col];
                }
            }

            var words = Console.ReadLine()!.Split();

            var foundWords = new HashSet<string>();

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    var possibleWords = words.Where(w => w.StartsWith(matrix[row, col]));

                    foreach (var word in possibleWords)
                    {
                        if (IsWordPossible(row, col, word, 0))
                        {
                            foundWords.Add(word);
                        }
                    }
                }
            }

            Console.WriteLine(string.Join(Environment.NewLine, foundWords));
        }

        private static bool IsWordPossible(int row, int col, string word, int wordIdx)
        {
            if (IsCellInValid(row, col) || matrix[row, col] != word[wordIdx])
            {
                return false;
            }

            if (wordIdx == word.Length - 1)
            {
                return true;
            }

            wordIdx++;

            return IsWordPossible(row + 1, col, word, wordIdx) ||
                   IsWordPossible(row + 1, col - 1, word, wordIdx) ||
                   IsWordPossible(row, col - 1, word, wordIdx) ||
                   IsWordPossible(row - 1, col - 1, word, wordIdx) ||
                   IsWordPossible(row - 1, col, word, wordIdx) ||
                   IsWordPossible(row - 1, col + 1, word, wordIdx) ||
                   IsWordPossible(row, col + 1, word, wordIdx) ||
                   IsWordPossible(row + 1, col + 1, word, wordIdx);
        }

        private static bool IsCellInValid(int row, int col)
            => row < 0 || col < 0 || row >= matrix.GetLength(0) || col >= matrix.GetLength(1);
    }
}
