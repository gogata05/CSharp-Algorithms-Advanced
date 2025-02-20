using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeDecoding
{
    class Program
    {
        static void Main(string[] args)
        {
            // Четене на първото съобщение (първи ред) и премахване на излишни интервали
            string line1 = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(line1))
            {
                return;
            }
            // Преобразуване на първото съобщение в масив от цели числа
            int[] message1 = Array.ConvertAll(line1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);

            // Четене на второто съобщение (втори ред) и премахване на излишни интервали
            string line2 = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(line2))
            {
                return;
            }
            // Преобразуване на второто съобщение в масив от цели числа
            int[] message2 = Array.ConvertAll(line2.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);

            // Изчисляване на дължините на съобщенията
            int n = message1.Length;
            int m = message2.Length;

            // Създаваме DP матрица с размер (n+1) x (m+1)
            // dp[i, j] съдържа дължината на LCS за message1[0..i-1] и message2[0..j-1]
            int[,] dp = new int[n + 1, m + 1];

            // Попълване на DP матрицата с класически алгоритъм за LCS
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Ако текущите елементи съвпадат, увеличаваме дължината на LCS с 1
                    if (message1[i - 1] == message2[j - 1])
                    {
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        // Ако не съвпадат, избираме по-голямата стойност от горната или лявата клетка
                        // При равенство избираме лявата клетка (dp[i, j-1]) за "дясно" решение
                        if (dp[i - 1, j] > dp[i, j - 1])
                        {
                            dp[i, j] = dp[i - 1, j];
                        }
                        else if (dp[i - 1, j] < dp[i, j - 1])
                        {
                            dp[i, j] = dp[i, j - 1];
                        }
                        else
                        {
                            dp[i, j] = dp[i, j - 1];
                        }
                    }
                }
            }

            // Дължината на LCS е в dp[n, m]
            int lcsLength = dp[n, m];

            // Реконструираме LCS чрез backtracking от долния десен ъгъл на матрицата
            List<int> lcs = new List<int>();
            int x = n, y = m;
            while (x > 0 && y > 0)
            {
                // Ако елементите съвпадат, добавяме ги към LCS и намаляваме и двата индекса
                if (message1[x - 1] == message2[y - 1])
                {
                    lcs.Add(message1[x - 1]);
                    x--;
                    y--;
                }
                else
                {
                    // При равни стойности, за да изберем "дясното" решение, ако dp[x-1, y] == dp[x, y-1] преминаваме наляво (y--)
                    if (dp[x - 1, y] == dp[x, y - 1])
                    {
                        y--;
                    }
                    else if (dp[x - 1, y] > dp[x, y - 1])
                    {
                        x--;
                    }
                    else
                    {
                        y--;
                    }
                }
            }

            // Тъй като LCS е събран в обратен ред, обръщаме го
            lcs.Reverse();

            // Извеждаме декодираното съобщение (всеки елемент с интервал)
            foreach (int num in lcs)
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();

            // Извеждаме дължината на намерената LCS
            Console.WriteLine(lcsLength);
        }
    }
}