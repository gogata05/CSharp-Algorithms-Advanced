using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] numbers = Console.ReadLine().Split().Select(int.Parse).ToArray();
            
            int[] increasingSeq = GetLongestIncreasingSubsequence(numbers);
            int[] decreasingSeq = GetLongestDecreasingSubsequence(numbers);

            if (increasingSeq.Length >= decreasingSeq.Length)
            {
                Console.WriteLine(string.Join(" ", increasingSeq));
            }
            else
            {
                Console.WriteLine(string.Join(" ", decreasingSeq));
            }
        }

        static int[] GetLongestIncreasingSubsequence(int[] arr)
        {
            int[] len = new int[arr.Length];
            int[] prev = new int[arr.Length];
            int maxLen = 0;
            int lastIndex = -1;

            for (int i = 0; i < arr.Length; i++)
            {
                len[i] = 1;
                prev[i] = -1;

                for (int j = 0; j < i; j++)
                {
                    if (arr[j] < arr[i] && len[j] + 1 > len[i])
                    {
                        len[i] = len[j] + 1;
                        prev[i] = j;
                    }
                }

                if (len[i] > maxLen)
                {
                    maxLen = len[i];
                    lastIndex = i;
                }
            }

            List<int> sequence = new List<int>();
            while (lastIndex != -1)
            {
                sequence.Add(arr[lastIndex]);
                lastIndex = prev[lastIndex];
            }
            sequence.Reverse();
            return sequence.ToArray();
        }

        static int[] GetLongestDecreasingSubsequence(int[] arr)
        {
            int[] len = new int[arr.Length];
            int[] prev = new int[arr.Length];
            int maxLen = 0;
            int lastIndex = -1;

            for (int i = 0; i < arr.Length; i++)
            {
                len[i] = 1;
                prev[i] = -1;

                for (int j = 0; j < i; j++)
                {
                    if (arr[j] > arr[i] && len[j] + 1 > len[i])
                    {
                        len[i] = len[j] + 1;
                        prev[i] = j;
                    }
                }

                if (len[i] > maxLen)
                {
                    maxLen = len[i];
                    lastIndex = i;
                }
            }

            List<int> sequence = new List<int>();
            while (lastIndex != -1)
            {
                sequence.Add(arr[lastIndex]);
                lastIndex = prev[lastIndex];
            }
            sequence.Reverse();
            return sequence.ToArray();
        }
    }
}
