using System;
using System.Collections.Generic;
using System.Linq;

namespace _1._Cable_Merchant
{
    public class Program
    {
        private static List<int> prices;
        private static int[] bestPrices;
        static void Main(string[] args)
        {
            prices = new List<int> { 0 };
            prices.AddRange(Console.ReadLine()
                .Split()
                .Select(int.Parse));
            int connectorPrice = int.Parse(Console.ReadLine());
            bestPrices = new int[prices.Count];
            for (int length = 1; length < prices.Count; length++)
            {
                int bestPrice = CutCable(length, connectorPrice);
                Console.Write(bestPrice+" ");
            }
        }

        private static int CutCable(int length, int connectorPrice)
        {
            if (length==0)
            {
                return 0;
            }
            if (bestPrices[length]!=0)
            {
                return bestPrices[length];
            }
            int bestPrice = prices[length];
            for (int i = 1; i < length; i++)
            {
                int currentPrice = prices[i] + CutCable(length - i,connectorPrice)-2*connectorPrice;
                if (currentPrice>bestPrice)
                {
                    bestPrice = currentPrice;
                }
            }
            bestPrices[length] = bestPrice;
            return bestPrice;
        }
    }
}
