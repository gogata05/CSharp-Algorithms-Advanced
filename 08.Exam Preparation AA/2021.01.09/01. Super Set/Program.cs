namespace _01._Super_Set
{
    using System;
    using System.Linq;

    internal class Program
    {
        private static int[] nums;

        static void Main(string[] args)
        {
            nums = Console.ReadLine()!.Split(", ").Select(int.Parse).ToArray();

            for (int k = 1; k <= nums.Length; k++)
            {
                var combination = new int[k];
                PrintCombinations(0, 0, combination);
            }
        }

        private static void PrintCombinations(int idx, int startIdx, int[] combination)
        {
            if (idx >= combination.Length)
            {
                Console.WriteLine(string.Join(" ", combination));
                return;
            }

            for (int i = startIdx; i < nums.Length; i++)
            {
                combination[idx] = nums[i];
                PrintCombinations(idx + 1, i + 1, combination);
            }
        }
    }
}
