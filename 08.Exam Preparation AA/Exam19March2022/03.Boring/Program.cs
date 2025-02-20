using System;
using System.Collections.Generic;

namespace TheBoringCompany
{
    class Program
    {
        static void Main(string[] args)
        {
            // Четене на входа от конзолата
            // Първият ред съдържа броя на районите (номерирани от 0 до n - 1)
            string inputLine = Console.ReadLine()?.Trim();
            int n = int.Parse(inputLine);

            // Вторият ред съдържа броя на връзките между районите
            inputLine = Console.ReadLine()?.Trim();
            int e = int.Parse(inputLine);

            // Третият ред съдържа броя на предварително свързаните райони към тунелната система
            inputLine = Console.ReadLine()?.Trim();
            int p = int.Parse(inputLine);

            // Списък за съхранение на всички връзки (край, краен район и цена)
            List<Edge> edges = new List<Edge>();

            // Четене на e връзки във формат: "{първи район} {втори район} {estimated cost}"
            for (int i = 0; i < e; i++)
            {
                inputLine = Console.ReadLine()?.Trim();
                string[] parts = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int district1 = int.Parse(parts[0]);
                int district2 = int.Parse(parts[1]);
                int cost = int.Parse(parts[2]);
                edges.Add(new Edge(district1, district2, cost));
            }

            // Булев масив за отбелязване на районите, които вече са свързани към тунелната система
            bool[] inNetwork = new bool[n];

            // Четене на p реда, които задават предварително свързаните райони
            // Всеки ред има формат: "{първи район} {втори район}"
            // И двата района от всеки ред се считат за свързани
            for (int i = 0; i < p; i++)
            {
                inputLine = Console.ReadLine()?.Trim();
                string[] parts = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int d1 = int.Parse(parts[0]);
                int d2 = int.Parse(parts[1]);
                inNetwork[d1] = true;
                inNetwork[d2] = true;
            }

            // Променлива за общата минимална цена за свързване на всички райони към тунелната система
            int totalBudget = 0;

            // Метод за проверка дали всички райони са свързани към тунелната система
            bool AllConnected(bool[] network)
            {
                foreach (bool connected in network)
                {
                    if (!connected)
                        return false;
                }
                return true;
            }

            // Изпълнение на алгоритъма тип Прим:
            // Докато не са свързани всички райони, търсим най-евтината връзка,
            // която свързва район, който вече е в мрежата, с район, който не е в мрежата.
            while (!AllConnected(inNetwork))
            {
                int minCost = int.MaxValue;
                int candidateDistrict = -1; // районът, който ще бъде свързан

                // Обхождаме всички връзки
                foreach (var edge in edges)
                {
                    // Проверяваме дали точно един от двата района от връзката е вече в мрежата
                    bool firstIn = inNetwork[edge.District1];
                    bool secondIn = inNetwork[edge.District2];

                    if (firstIn && !secondIn)
                    {
                        if (edge.Cost < minCost)
                        {
                            minCost = edge.Cost;
                            candidateDistrict = edge.District2;
                        }
                    }
                    else if (!firstIn && secondIn)
                    {
                        if (edge.Cost < minCost)
                        {
                            minCost = edge.Cost;
                            candidateDistrict = edge.District1;
                        }
                    }
                }

                // Ако намерим валидна връзка, добавяме новия район към мрежата и акумулираме цената
                if (candidateDistrict != -1)
                {
                    inNetwork[candidateDistrict] = true;
                    totalBudget += minCost;
                }
                else
                {
                    // Ако не намерим връзка, някой район не може да бъде свързан (но според условието това не би трябвало да се случи)
                    break;
                }
            }

            // Извеждаме крайния резултат
            Console.WriteLine("Minimum budget: " + totalBudget);
        }
    }

    // Клас, представящ връзка между два района с дадена цена
    class Edge
    {
        public int District1 { get; set; }
        public int District2 { get; set; }
        public int Cost { get; set; }

        public Edge(int d1, int d2, int cost)
        {
            District1 = d1;
            District2 = d2;
            Cost = cost;
        }
    }
}