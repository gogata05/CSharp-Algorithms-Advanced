namespace _02._Socks
{
    using System;
    using System.Linq;

    internal class Program
    {
        static void Main(string[] args)
        {
            var arr1 = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
            var arr2 = Console.ReadLine()!.Split().Select(int.Parse).ToArray();

            var dp = new int[arr1.Length + 1, arr2.Length + 1];

            for (int r = 1; r <= arr1.Length; r++)
            {
                for (int c = 1; c <= arr2.Length; c++)
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

            int longestSequenceCount = dp[arr1.Length, arr2.Length];

            Console.WriteLine(longestSequenceCount);
        }
    }
}
