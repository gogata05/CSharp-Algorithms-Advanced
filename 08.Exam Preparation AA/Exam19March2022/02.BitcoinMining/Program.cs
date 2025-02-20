using System;
using System.Collections.Generic;

namespace _02.BitcoinMining
{
    public class Transaction
    {
        public string Hash { get; set; }

        public int Weight { get; set; }

        public int Value { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
    
    internal class Program
    {
        static void Main(string[] args)
        {
            var maxCapacity = 1000000;

            var transactionsCount = int.Parse(Console.ReadLine());

            var transactions = new List<Transaction>();

            for (int i = 0; i < transactionsCount; i++)
            {
                var transactionArgs = Console.ReadLine().Split();

                transactions.Add(new Transaction
                {
                    Hash = transactionArgs[0],
                    Weight = int.Parse(transactionArgs[1]),
                    Value = int.Parse(transactionArgs[2]),
                    From = transactionArgs[3],
                    To = transactionArgs[4]
                });
            }

            var dp = new int[transactions.Count + 1, maxCapacity + 1];
            var used = new bool[transactions.Count + 1, maxCapacity + 1];

            for (int row = 1; row < dp.GetLength(0); row++)
            {
                var itemIdx = row - 1;
                var item = transactions[itemIdx];

                for (int capacity = 1; capacity < dp.GetLength(1); capacity++)
                {
                    var excluding = dp[row - 1, capacity];

                    if (item.Weight > capacity)
                    {
                        dp[row, capacity] = excluding;
                        continue;
                    }

                    var including = item.Value + dp[row - 1, capacity - item.Weight];

                    if (including > excluding)
                    {
                        dp[row, capacity] = including;
                        used[row, capacity] = true;
                    }
                    else
                    {
                        dp[row, capacity] = excluding;
                    }
                }
            }

            var currentCapacity = maxCapacity;

            var totalWeight = 0;

            var usedHash = new SortedSet<string>();

            for (int row = dp.GetLength(0) - 1; row > 0; row--)
            {
                if (!used[row, currentCapacity])
                {
                    continue;
                }

                var transaction = transactions[row - 1];
                totalWeight += transaction.Weight;
                currentCapacity -= transaction.Weight;
                usedHash.Add(transaction.Hash);

                if (currentCapacity == 0)
                {
                    break;
                }
            }

            Console.WriteLine($"Total Size: {totalWeight}");
            Console.WriteLine($"Total Fees: {dp[transactions.Count, maxCapacity]}");
            Console.WriteLine(String.Join(Environment.NewLine, usedHash));
        }
    }
}
