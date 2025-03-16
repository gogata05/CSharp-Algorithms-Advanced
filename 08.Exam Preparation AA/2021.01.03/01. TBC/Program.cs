namespace _01._TBC
{
    using System;

    internal class Program
    {
        private static char[,] matrix;

        static void Main(string[] args)
        {
            int rows = int.Parse(Console.ReadLine()!);
            int cols = int.Parse(Console.ReadLine()!);

            matrix = new char[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                var line = Console.ReadLine()!;

                for (int c = 0; c < cols; c++)
                {
                    matrix[r, c] = line[c];
                }
            }

            int tunnelCount = 0;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (matrix[r, c] == 't')
                    {
                        FindTunnel(r, c);
                        tunnelCount++;
                    }
                }
            }

            Console.WriteLine(tunnelCount);
        }

        private static void FindTunnel(int row, int col)
        {
            if (IsInvalidCell(row, col) || matrix[row, col] != 't')
            {
                return;
            }

            matrix[row, col] = 'v';

            FindTunnel(row, col + 1);
            FindTunnel(row, col - 1);
            FindTunnel(row + 1, col);
            FindTunnel(row - 1, col);
            FindTunnel(row + 1, col + 1);
            FindTunnel(row + 1, col - 1);
            FindTunnel(row - 1, col + 1);
            FindTunnel(row - 1, col - 1);
        }

        private static bool IsInvalidCell(int row, int col)
            => row < 0 || col < 0 || row >= matrix.GetLength(0) || col >= matrix.GetLength(1);
    }
}
