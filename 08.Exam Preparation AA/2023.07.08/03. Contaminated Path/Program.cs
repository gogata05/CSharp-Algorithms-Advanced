namespace _03._Contaminated_Path
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        private static int[,] matrix;
        private static int[,] dp;

        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine()!);

            matrix = new int[n, n];


            for (int row = 0; row < n; row++)
            {
                var line = Console.ReadLine()!
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();

                for (int col = 0; col < n; col++)
                {
                    matrix[row, col] = line[col];
                }
            }

            var contaminated = Console.ReadLine()!
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray());
            dp = new int[n, n];
            dp[0, 0] = matrix[0, 0];

            foreach (var tuple in contaminated)
            {
                int row = tuple[0];
                int col = tuple[1];

                dp[row, col] = -1;
            }

            FillDpMatrix(n);

            var path = CalcPath(n);

            Console.WriteLine($"[{string.Join(" ", path)}]");
        }

        private static Stack<string> CalcPath(int n)
        {
            var path = new Stack<string>();
            int row = n - 1;
            int col = n - 1;
            int maxFert = -20;


            for (int r = 0; r < n; r++)
            {
                for (int c = 0; c < n; c++)
                {
                    if (dp[r, c] > maxFert)
                    {
                        maxFert = dp[r, c];
                        row = r;
                        col = c;
                    }
                }
            }

            Console.WriteLine($"Max total fertility: {maxFert}");

            while (row > 0 && col > 0)
            {
                path.Push($"({row}, {col})");

                if (dp[row, col - 1] >= dp[row - 1, col])
                {
                    if (dp[row, col - 1] == 0)
                    {
                        return path;
                    }

                    col--;
                }
                else
                {
                    row--;
                }
            }
            while (row > 0)
            {
                path.Push($"({row}, {col})");
                row--;
            }
            while (col > 0)
            {
                path.Push($"({row}, {col})");
                col--;
            }

            path.Push("(0, 0)");

            return path;
        }

        private static void FillDpMatrix(int n)
        {
            for (int row = 1; row < n; row++)
            {
                if (dp[row, 0] == -1)
                {
                    dp[row, 0] = 0;
                    continue;
                }

                dp[row, 0] = matrix[row, 0] + dp[row - 1, 0];
            }

            for (int col = 1; col < n; col++)
            {
                if (dp[0, col] == -1)
                {
                    dp[0, col] = 0;
                    continue;
                }

                dp[0, col] = matrix[0, col] + dp[0, col - 1];
            }

            for (int row = 1; row < n; row++)
            {
                for (int col = 1; col < n; col++)
                {
                    if (dp[row, col] == -1)
                    {
                        dp[row, col] = 0;
                        continue;
                    }

                    dp[row, col] = matrix[row, col] + Math.Max(dp[row, col - 1], dp[row - 1, col]);
                }
            }
        }
    }
}
