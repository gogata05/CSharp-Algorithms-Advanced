namespace _01._Move_Down_Right
{
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            int rows = int.Parse(Console.ReadLine()!);
            int cols = int.Parse(Console.ReadLine()!);

            var dp = new long[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                dp[row, 0] = 1;
            }

            for (int col = 0; col < cols; col++)
            {
                dp[0, col] = 1;
            }

            for (int row = 1; row < rows; row++)
            {
                for (int col = 1; col < cols; col++)
                {
                    dp[row, col] = dp[row - 1, col] + dp[row, col - 1];
                }
            }

            long pathsCount = dp[rows - 1, cols - 1];

            Console.WriteLine(pathsCount);
        }
    }
}
