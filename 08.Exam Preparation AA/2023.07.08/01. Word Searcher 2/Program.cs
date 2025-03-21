using System;
using System.Collections.Generic;

namespace _01._Word_Searcher_2
{
    class Program
    {
        private static int[] dr = { -1, -1, -1, 0, 0, 1, 1, 1 };
        private static int[] dc = { -1, 0, 1, -1, 1, -1, 0, 1 };

        static void Main(string[] args)
        {
            int rows = int.Parse(Console.ReadLine());
            int cols = int.Parse(Console.ReadLine());

            char[,] grid = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                string row = Console.ReadLine();
                for (int j = 0; j < cols && j < row.Length; j++)
                {
                    grid[i, j] = row[j];
                }
            }

            string[] words = Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> foundWords = new HashSet<string>();

            foreach (string word in words)
            {
                if (CanFindWord(grid, word, rows, cols))
                {
                    foundWords.Add(word);
                }
            }

            foreach (string word in foundWords)
            {
                Console.WriteLine(word);
            }
        }

        static bool CanFindWord(char[,] grid, string word, int rows, int cols)
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid[i, j] == word[0])
                    {
                        bool[,] visited = new bool[rows, cols];
                        if (DFS(grid, word, i, j, 0, visited, rows, cols))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static bool DFS(char[,] grid, string word, int row, int col, int index, bool[,] visited, int rows, int cols)
        {
            if (index == word.Length)
            {
                return true;
            }

            if (row < 0 || row >= rows || col < 0 || col >= cols || visited[row, col] || grid[row, col] != word[index])
            {
                return false;
            }

            visited[row, col] = true;

            for (int i = 0; i < 8; i++)
            {
                int newRow = row + dr[i];
                int newCol = col + dc[i];

                if (DFS(grid, word, newRow, newCol, index + 1, visited, rows, cols))
                {
                    return true;
                }
            }

            visited[row, col] = false;
            return false;
        }
    }
}
