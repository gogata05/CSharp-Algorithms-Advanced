using System;
using System.Linq;

namespace _2._Battle_Points
{
    public class Program
    {
        static void Main(string[] args)
        {
            var requiredEnergy = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();
            var battlePoints = Console.ReadLine()
                .Split()
                .Select(int.Parse)
                .ToArray();
            int enemies = requiredEnergy.Length;
            int initialEnergy = int.Parse(Console.ReadLine());
            var dp = new int[enemies + 1,initialEnergy+1];
            for (int row = 1; row < dp.GetLength(0); row++)
            {
                int enemyIdx = row - 1;
                int enemyEnergy = requiredEnergy[enemyIdx];
                int enemyBattlePoints = battlePoints[enemyIdx];
                for (int energy = 1; energy < dp.GetLength(1); energy++)
                {
                    int skip = dp[row - 1, energy];
                    if (enemyEnergy>energy)
                    {
                        dp[row, energy] = skip;
                        continue;
                    }
                    int take = enemyBattlePoints + dp[row - 1, energy - enemyEnergy];
                    dp[row, energy] = Math.Max(skip, take);

                }
            }
            Console.WriteLine(dp[enemies,initialEnergy]);
        }
    }
}
