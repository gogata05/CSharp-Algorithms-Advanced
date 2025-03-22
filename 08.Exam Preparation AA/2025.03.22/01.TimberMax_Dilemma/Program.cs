namespace _01.TimberMax_Dilemma
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] prices = Console.ReadLine().Split().Select(int.Parse).ToArray();
            int k = int.Parse(Console.ReadLine());

            int[] dp = new int[k + 1];

            for (int i = 1; i <= k; i++)
            {
                for (int j = 1; j <= Math.Min(i, prices.Length); j++)
                {
                    dp[i] = Math.Max(dp[i], prices[j - 1] + dp[i - j]);
                }
            }

            Console.WriteLine(dp[k]);
        }
    }
}
