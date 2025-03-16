using System;
using System.Collections.Generic;

namespace TravelOptimizer
{
    class Program
    {
        // Клас-структура за съхранение на информация за ребро (съседен град и цена)
        public class Edge
        {
            public int Neighbor { get; set; }
            public int Cost { get; set; }

            public Edge(int neighbor, int cost)
            {
                Neighbor = neighbor;
                Cost = cost;
            }
        }

        // Клас-структура за състоянието в приоритетната опашка
        // cost – текуща цена; city – град; usedEdges – колко ребра сме ползвали досега
        public class State : IComparable<State>, IEquatable<State>
        {
            public int Cost { get; set; }
            public int City { get; set; }
            public int UsedEdges { get; set; }

            public State(int cost, int city, int usedEdges)
            {
                Cost = cost;
                City = city;
                UsedEdges = usedEdges;
            }

            // 1. Подобрено CompareTo с "tie-break"
            public int CompareTo(State other)
            {
                int compare = this.Cost.CompareTo(other.Cost);
                if (compare != 0)
                {
                    return compare;
                }

                compare = this.City.CompareTo(other.City);
                if (compare != 0)
                {
                    return compare;
                }

                // Ако и градът е същият, гледаме usedEdges
                return this.UsedEdges.CompareTo(other.UsedEdges);
            }

            // 2. Правилно Equals и GetHashCode, за да сме последователни с CompareTo
            public bool Equals(State other)
            {
                if (other == null) return false;
                return this.Cost == other.Cost
                    && this.City == other.City
                    && this.UsedEdges == other.UsedEdges;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as State);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + Cost.GetHashCode();
                    hash = hash * 23 + City.GetHashCode();
                    hash = hash * 23 + UsedEdges.GetHashCode();
                    return hash;
                }
            }
        }

        static void Main(string[] args)
        {
            // 1. Четене на броя на ребрата (e)
            string inputLine = Console.ReadLine()?.Trim();
            int e = int.Parse(inputLine);

            // 2. Четене на e реда с ребра във формат "{start}, {end}, {weight}"
            var adjacencyList = new Dictionary<int, List<Edge>>();

            void EnsureCity(int city)
            {
                if (!adjacencyList.ContainsKey(city))
                {
                    adjacencyList[city] = new List<Edge>();
                }
            }

            for (int i = 0; i < e; i++)
            {
                inputLine = Console.ReadLine()?.Trim();
                // Пример: "0, 1, 15"
                string[] parts = inputLine.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                int startCity = int.Parse(parts[0].Trim());
                int endCity = int.Parse(parts[1].Trim());
                int cost = int.Parse(parts[2].Trim());

                // Добавяме двупосочна връзка
                EnsureCity(startCity);
                EnsureCity(endCity);

                adjacencyList[startCity].Add(new Edge(endCity, cost));
                adjacencyList[endCity].Add(new Edge(startCity, cost));
            }

            // 3. Четене на стартов град
            inputLine = Console.ReadLine()?.Trim();
            int start = int.Parse(inputLine);

            // 4. Четене на дестинация
            inputLine = Console.ReadLine()?.Trim();
            int destination = int.Parse(inputLine);

            // 5. Четене на k – колко междинни спирки
            inputLine = Console.ReadLine()?.Trim();
            int k = int.Parse(inputLine);

            // Уверяваме се, че start и destination съществуват в речника
            EnsureCity(start);
            EnsureCity(destination);

            // dist[(city, usedEdges)] = най-ниската цена за стигане до city с usedEdges използвани ребра
            var dist = new Dictionary<(int city, int usedEdges), int>();
            var predecessor = new Dictionary<(int city, int usedEdges), (int parentCity, int parentEdges)>();

            // Инициализираме dist с ∞
            foreach (var kvp in adjacencyList)
            {
                int city = kvp.Key;
                for (int usedEdges = 0; usedEdges <= k + 1; usedEdges++)
                {
                    dist[(city, usedEdges)] = int.MaxValue;
                }
            }

            // Начална позиция: (start, 0 ребра) с цена 0
            dist[(start, 0)] = 0;

            // Приоритетна опашка (SortedSet), която използва State.CompareTo
            var priorityQueue = new SortedSet<State>();
            priorityQueue.Add(new State(0, start, 0));

            // Алгоритъм
            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Min; // Най-малката цена
                priorityQueue.Remove(current);

                int currCost = current.Cost;
                int currCity = current.City;
                int used = current.UsedEdges;

                // Ако вече имаме по-добра цена, пропускаме
                if (dist[(currCity, used)] < currCost)
                {
                    continue;
                }

                // Разглеждаме съседи, ако можем да ползваме още ръбове
                if (used < k + 1)
                {
                    foreach (var edge in adjacencyList[currCity])
                    {
                        int nextCity = edge.Neighbor;
                        int nextCost = currCost + edge.Cost;
                        int nextUsed = used + 1;

                        if (nextCost < dist[(nextCity, nextUsed)])
                        {
                            dist[(nextCity, nextUsed)] = nextCost;
                            predecessor[(nextCity, nextUsed)] = (currCity, used);

                            // Добавяме в опашката
                            priorityQueue.Add(new State(nextCost, nextCity, nextUsed));
                        }
                    }
                }
            }

            // Търсим минимална цена за destination при 0..k+1 ръба
            int minCost = int.MaxValue;
            int bestEdgesUsed = -1;

            for (int usedEdges = 0; usedEdges <= k + 1; usedEdges++)
            {
                if (dist[(destination, usedEdges)] < minCost)
                {
                    minCost = dist[(destination, usedEdges)];
                    bestEdgesUsed = usedEdges;
                }
            }

            // Ако не сме намерили валиден път
            if (minCost == int.MaxValue)
            {
                Console.WriteLine("There is no such path.");
                return;
            }

            // Възстановяване на пътя
            var path = new List<int>();
            int currN = destination;
            int currU = bestEdgesUsed;

            while (true)
            {
                path.Add(currN);
                if (currN == start && currU == 0)
                {
                    break;
                }

                var prev = predecessor[(currN, currU)];
                currN = prev.parentCity;
                currU = prev.parentEdges;
            }

            path.Reverse();
            Console.WriteLine(string.Join(" ", path));
        }
    }
}