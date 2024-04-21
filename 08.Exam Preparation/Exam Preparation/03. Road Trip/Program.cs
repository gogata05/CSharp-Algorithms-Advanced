using System;
using System.Linq;

namespace _03._Road_Trip
{
    class Program
    {
        static void Main(string[] args)
        {
            var value = Console.ReadLine()
                 .Split(", ")
                 .Select(int.Parse)
                 .ToArray();
             var weight = Console.ReadLine()
                .Split(", ")
                .Select(int.Parse)
                .ToArray();
            int maxCapacity = int.Parse(Console.ReadLine());
            int items = value.Length;
            var dp = new int[items + 1, maxCapacity + 1];
            for (int row = 1; row < dp.GetLength(0); row++)
            {
                int itemValue = value[row - 1];
                int itemWeight = weight[row - 1];
                for (int capacity = 1; capacity < dp.GetLength(1); capacity++)
                {
                    int skipValue = dp[row - 1, capacity];
                    if (capacity<itemWeight)
                    {
                        dp[row, capacity] = skipValue;
                        continue;
                    }
                    int takeValue = itemValue + dp[row - 1, capacity - itemWeight];
                    if (takeValue>skipValue)
                    {
                        dp[row, capacity] = takeValue;
                    }
                    else
                    {
                        dp[row, capacity] = skipValue;
                    }
                }
            }
            int maxValue = dp[items, maxCapacity];
            Console.WriteLine("Maximum value: {0}",maxValue);
        }
    }
}
