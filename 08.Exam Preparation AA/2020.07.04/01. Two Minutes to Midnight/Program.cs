namespace _01._Two_Minutes_to_Midnight
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        private static Dictionary<string, long> cache = new Dictionary<string, long>();

        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine()!);
            int k = int.Parse(Console.ReadLine()!);

            long combinations = Binom(n, k);

            Console.WriteLine(combinations);
        }

        private static long Binom(int row, int col)
        {
            if(col == 0 || row == col) 
            {
                return 1;
            }

            string key = row.ToString() + col.ToString();

            if (cache.ContainsKey(key))
            {
                return cache[key];
            }

            var result = Binom(row - 1, col - 1) + Binom(row - 1, col);

            cache.Add(key, result);

            return result;
        }
    }
}
