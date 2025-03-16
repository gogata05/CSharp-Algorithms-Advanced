namespace _01._Bitcoin_Miners
{
    using System;

    internal class Program
    {
        private static long[,] cache;

        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine()!);
            int k = int.Parse(Console.ReadLine()!);

            cache = new long[n + 1, k + 1];

            long combinations = Binom(n, k);

            Console.WriteLine(combinations);    
        }

        private static long Binom(int row, int col)
        {
            if (col == 0 || col == row)
            {
                return 1;
            }

            if (cache[row, col] > 0)
            {
                return cache[row, col];
            }

            var result = Binom(row - 1, col) + Binom(row - 1, col - 1);

            cache[row, col] = result;

            return result;
        }
    }
}
