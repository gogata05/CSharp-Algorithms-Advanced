namespace _03._Bank_Robbery
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        static void Main(string[] args)
        {
            var arr = Console.ReadLine()!.Split().Select(int.Parse).ToArray();

            var targetSum = arr.Sum() / 2;

            Dictionary<int, int> possibleSums = CalcSums(arr, targetSum);

            var joshBoxes = GetSubset(possibleSums, targetSum).OrderBy(b => b);

            var prakashBoxes = arr.Except(joshBoxes).OrderBy(b => b);

            Console.WriteLine(string.Join(" ", joshBoxes));
            Console.WriteLine(string.Join(" ", prakashBoxes));
        }

        private static List<int> GetSubset(Dictionary<int, int> possibleSums, int targetSum)
        {
            var subset = new List<int>();

            while (targetSum > 0)
            {
                var box = possibleSums[targetSum];
                subset.Add(box);

                targetSum -= box;
            }

            return subset;
        }

        private static Dictionary<int, int> CalcSums(int[] arr, int targetSum)
        {
            var sums = new Dictionary<int, int>();
            sums.Add(0, 0);

            foreach (int box in arr)
            {
                var curSums = sums.Keys.ToArray();

                foreach (var sum in curSums)
                {
                    var newSum = box + sum;

                    if (newSum > targetSum || sums.ContainsKey(newSum))
                    {
                        continue;
                    }

                    sums.Add(newSum, box);

                    if (newSum == targetSum)
                        return sums;
                }
            }

            return sums;
        }
    }
}
