using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolEmergencyPlan
{
    class Program
    {
        public class Edge
        {
            public int To { get; set; }      // Идентификатор на съседната стая
            public int CostSeconds { get; set; } // Времето (в секунди) за придвижване

            public Edge(int to, int costSeconds)
            {
                To = to;
                CostSeconds = costSeconds;
            }
        }

        static void Main(string[] args)
        {
            string inputLine = Console.ReadLine()?.Trim();
            int n = int.Parse(inputLine);

            inputLine = Console.ReadLine()?.Trim();
            var exitRoomsParts = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<int> exits = new HashSet<int>();
            foreach (var part in exitRoomsParts)
            {
                exits.Add(int.Parse(part));
            }

           
            inputLine = Console.ReadLine()?.Trim();
            int c = int.Parse(inputLine);

           
            Dictionary<int, List<Edge>> adjacencyList = new Dictionary<int, List<Edge>>();
            for (int i = 0; i < n; i++)
            {
                adjacencyList[i] = new List<Edge>();
            }

           
            for (int i = 0; i < c; i++)
            {
                inputLine = Console.ReadLine()?.Trim();
                // Пример: "1 5 04:10" или "2 4 07:00"
                // Някои задачи дават разделители със запетаи, но тук условието казва, че са с интервал
                string[] parts = inputLine.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int roomA = int.Parse(parts[0]);
                int roomB = int.Parse(parts[1]);
                int costSeconds = ParseTimeToSeconds(parts[2]); // mm:ss -> seconds

                // Двупосочни връзки
                adjacencyList[roomA].Add(new Edge(roomB, costSeconds));
                adjacencyList[roomB].Add(new Edge(roomA, costSeconds));
            }

          
            inputLine = Console.ReadLine()?.Trim();
            int evacLimitSeconds = ParseTimeToSeconds(inputLine);

            

            int[] dist = new int[n];
            for (int i = 0; i < n; i++)
            {
                dist[i] = int.MaxValue;
            }

           
            var pq = new SortedSet<(int cost, int room)>(Comparer<(int cost, int room)>.Create((a, b) =>
            {
                
                int cmp = a.cost.CompareTo(b.cost);
                if (cmp == 0)
                {
                    // Ако cost е равен, сравняваме по room, за да избегнем дублиране в SortedSet
                    return a.room.CompareTo(b.room);
                }
                return cmp;
            }));

           
            foreach (int exitRoom in exits)
            {
                dist[exitRoom] = 0;
                pq.Add((0, exitRoom));
            }

           
            while (pq.Count > 0)
            {
                var current = pq.Min;
                pq.Remove(current);

                int currentCost = current.cost;
                int currentRoom = current.room;

                
                if (currentCost > dist[currentRoom])
                {
                    continue;
                }

              
                foreach (var edge in adjacencyList[currentRoom])
                {
                    int nextRoom = edge.To;
                    int nextCost = currentCost + edge.CostSeconds;
                    if (nextCost < dist[nextRoom])
                    {
                        
                        dist[nextRoom] = nextCost;
                        pq.Add((nextCost, nextRoom));
                    }
                }
            }

           

            for (int i = 0; i < n; i++)
            {
                // Пропускаме стаите, които са изходи (примерите не ги показват в изхода)
                if (exits.Contains(i))
                {
                    continue;
                }

                if (dist[i] == int.MaxValue)
                {
                    Console.WriteLine($"Unreachable {i} (N/A)");
                }
                else
                {
                    // Форматираме времето като hh:mm:ss
                    string formattedTime = FormatSeconds(dist[i]);
                    if (dist[i] <= evacLimitSeconds)
                    {
                        Console.WriteLine($"Safe {i} ({formattedTime})");
                    }
                    else
                    {
                        Console.WriteLine($"Unsafe {i} ({formattedTime})");
                    }
                }
            }
        }

     
        private static int ParseTimeToSeconds(string timeStr)
        {
            // timeStr е във формат "mm:ss"
            var parts = timeStr.Split(':');
            int mm = int.Parse(parts[0]);
            int ss = int.Parse(parts[1]);
            return mm * 60 + ss;
        }

      
        private static string FormatSeconds(int totalSeconds)
        {
            int hh = totalSeconds / 3600;
            int rem = totalSeconds % 3600;
            int mm = rem / 60;
            int ss = rem % 60;
            return $"{hh:D2}:{mm:D2}:{ss:D2}";
        }
    }
}