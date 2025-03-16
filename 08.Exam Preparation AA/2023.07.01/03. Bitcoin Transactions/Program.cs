namespace _03._Bitcoin_Transactions
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        static void Main(string[] args)
        {
            var arr1 = Console.ReadLine()!.Split();
            var arr2 = Console.ReadLine()!.Split();

            var dp = new int[arr1.Length + 1, arr2.Length + 1];

            for (int r = 1; r < dp.GetLength(0); r++)
            {
                for (int c = 1; c < dp.GetLength(1); c++)
                {
                    if (arr1[r - 1] == arr2[c - 1])
                    {
                        dp[r, c] = 1 + dp[r - 1, c - 1];
                    }
                    else
                    {
                        dp[r, c] = Math.Max(dp[r - 1, c], dp[r, c - 1]);
                    }
                }
            }

            var sequence = new Stack<string>();

            int row = dp.GetLength(0) - 1;
            int col = dp.GetLength(1) - 1;

            while (row > 0 && col > 0)
            {
                if (arr1[row - 1] == arr2[col - 1])
                {
                    row--;
                    col--;
                    sequence.Push(arr1[row]);
                }
                else if (dp[row, col - 1] > dp[row - 1, col])
                {
                    col--;
                }
                else
                {
                    row--;
                }
            }

            Console.WriteLine($"[{string.Join(" ", sequence)}]");
        }
    }
}
