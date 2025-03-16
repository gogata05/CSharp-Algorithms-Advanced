namespace _02._Time
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        static void Main(string[] args)
        {
            var firstTimeline = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
            var secondTimeline = Console.ReadLine()!.Split().Select(int.Parse).ToArray();

            int[,] lcs = FillHelperMatrix(firstTimeline, secondTimeline);

            Stack<int> longestSubsequence = FindLongestSubsequence(firstTimeline, secondTimeline, lcs);

            Console.WriteLine(string.Join(" ", longestSubsequence));
            Console.WriteLine(longestSubsequence.Count);
        }

        private static int[,] FillHelperMatrix(int[] firstTimeline, int[] secondTimeline)
        {
            int[,] lcs = new int[firstTimeline.Length + 1, secondTimeline.Length + 1];

            for (var r = 1; r <= firstTimeline.Length; r++)
            {
                for (var c = 1; c <= secondTimeline.Length; c++)
                {
                    if (firstTimeline[r - 1] == secondTimeline[c - 1])
                    {
                        lcs[r, c] = 1 + lcs[r - 1, c - 1];
                    }
                    else
                    {
                        lcs[r, c] = Math.Max(lcs[r - 1, c], lcs[r, c - 1]);
                    }
                }
            }

            return lcs;
        }

        private static Stack<int> FindLongestSubsequence(int[] firstTimeline, int[] secondTimeline, int[,] lcs)
        {
            var longestSubsequence = new Stack<int>();
            int row = lcs.GetLength(0) - 1;
            int col = lcs.GetLength(1) - 1;

            while (row > 0 && col > 0)
            {
                if (firstTimeline[row - 1] == secondTimeline[col - 1])
                {
                    longestSubsequence.Push(firstTimeline[row - 1]);
                    row--;
                    col--;
                }
                else if (lcs[row, col - 1] >= lcs[row - 1, col])
                {
                    col--;
                }
                else
                {
                    row--;
                }
            }

            return longestSubsequence;
        }
    }
}
