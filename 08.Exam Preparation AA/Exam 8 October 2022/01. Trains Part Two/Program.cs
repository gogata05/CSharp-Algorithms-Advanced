using System;
using System.Collections.Generic;

namespace TrainsPartTwo
{
    class Program
    {
        static void Main(string[] args)
        {

            // 1. Прочитаме брой депа (n)
            int n = int.Parse(Console.ReadLine());

            // 2. Прочитаме брой релсови пътища (m)
            int m = int.Parse(Console.ReadLine());

            // 3. Прочитаме начално (start) и крайно (end) депо
            //    Използваме Split с char масив и StringSplitOptions.RemoveEmptyEntries, 
            //    за да избегнем проблеми с .NET 3.1
            string[] seTokens = Console.ReadLine()
                .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            int start = int.Parse(seTokens[0]);
            int end = int.Parse(seTokens[1]);

            // 4. Създаваме речник (adjacency list) за графа: depо -> List<(neighbor, distance)>
            var graph = new Dictionary<int, List<(int, int)>>();

            // Инициално добавяме всички депа в речника дори без съседи,
            // за да покрием случай, че някое депо не се среща в релсовите пътища
            for (int depot = 0; depot < n; depot++)
            {
                graph[depot] = new List<(int, int)>();
            }

            // 5. Четем m линии с формат {a} {b} {distance} и пълним графа (неориентиран)
            for (int i = 0; i < m; i++)
            {
                string[] tokens = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int a = int.Parse(tokens[0]);
                int b = int.Parse(tokens[1]);
                int dist = int.Parse(tokens[2]);

                // Добавяме a->b
                graph[a].Add((b, dist));
                // Добавяме и b->a (неориентиран път)
                graph[b].Add((a, dist));
            }

            // 6. Проверка на редки случаи
            //    - Ако n = 0, нямаме никакви депа -> не можем да намерим път
            if (n == 0)
            {
                Console.WriteLine("[WARN]: Нямаме никакви депа!");
                // Отпечатваме нещо по заданието? В условието няма пример за такава ситуация.
                // Ще отпечатаме просто:
                Console.WriteLine("");
                Console.WriteLine(0);
                return;
            }

            //    - Ако start или end не е в диапазона [0..n-1], нямаме валидно депо
            if (start < 0 || start >= n || end < 0 || end >= n)
            {
                Console.WriteLine("[ERROR]: Невалидно начално или крайно депо!");
                // Според условието не е казано какво да правим, просто няма път.
                Console.WriteLine("");
                Console.WriteLine(0);
                return;
            }

            //    - Ако start == end, тогава най-късият път е самият start,
            //      с дистанция 0.
            if (start == end)
            {
                // Отпечатваме "start" като път, и 0 като разстояние
                Console.WriteLine(start);
                Console.WriteLine(0);
                return;
            }

            //    - Ако m == 0, няма релсови пътища, значи няма връзка (освен ако start == end)
            //      но по-горе го проверихме. Тук очевидно няма път.
            if (m == 0)
            {
                // Няма път
                Console.WriteLine("[INFO]: Няма връзка между депата!");
                Console.WriteLine("");
                Console.WriteLine(0);
                return;
            }

            // 7. Подготвяме се да приложим Дейкстра
            var distances = new int[n];      // distances[v] = най-кратко разстояние от start до v
            var visited = new bool[n];       // дали сме посетили върха v (final)
            var parents = new int[n];        // за да реконструираме пътя

            // Инициализираме
            for (int i = 0; i < n; i++)
            {
                distances[i] = int.MaxValue;
                parents[i] = -1;
            }

            // разстоянието до start е 0
            distances[start] = 0;

            // Използваме приоритетна опашка (SortedSet), за да държим (distance, node)
            // .NET 3.1 няма вградена PriorityQueue, затова ползваме SortedSet
            var pq = new SortedSet<(int dist, int node)>(Comparer<(int dist, int node)>.Create((a, b) =>
            {
                // Сравняваме първо по dist
                int cmp = a.dist.CompareTo(b.dist);
                if (cmp == 0)
                {
                    // Ако дистанциите са еднакви, сравняваме по node
                    return a.node.CompareTo(b.node);
                }
                return cmp;
            }));

            // Добавяме стартовия връх
            pq.Add((0, start));

            // 8. Алгоритъм на Дейкстра
            while (pq.Count > 0)
            {
                var current = pq.Min; // вадим най-малкия по дистанция
                pq.Remove(current);

                int currentNode = current.node;
                int currentDist = current.dist;

                if (visited[currentNode])
                {
                    // Ако вече е посетен, пропускаме
                    continue;
                }
                visited[currentNode] = true;

                // Ако стигнем до end, можем да прекратим
                if (currentNode == end)
                {
                    break;
                }

                // Обхождаме всички съседи на currentNode
                foreach (var (neighbor, distance) in graph[currentNode])
                {
                    if (!visited[neighbor])
                    {
                        long newDist = (long)currentDist + distance; 
                        // ползваме long, за да избегнем преливане, макар по условие 0..10000
                        // при 10000 ребра по 10000 макс е 100 милиона, пасва в int, но сме предпазливи.

                        if (newDist < distances[neighbor])
                        {
                            distances[neighbor] = (int)newDist;
                            parents[neighbor] = currentNode;
                            pq.Add((distances[neighbor], neighbor));
                        }
                    }
                }
            }

            // 9. Проверяваме дали имаме път до end
            if (distances[end] == int.MaxValue)
            {
                // Няма път
                Console.WriteLine("[INFO]: Не е намерен път до крайното депо!");
                Console.WriteLine("");
                Console.WriteLine(0);
                return;
            }

            // 10. Реконструираме пътя (end -> ... -> start)
            var pathStack = new Stack<int>();
            int nodeToReconstruct = end;
            while (nodeToReconstruct != -1)
            {
                pathStack.Push(nodeToReconstruct);
                nodeToReconstruct = parents[nodeToReconstruct];
            }

            // 11. Отпечатваме пътя на един ред
            Console.WriteLine(string.Join(" ", pathStack));

            // 12. Отпечатваме общото разстояние
            Console.WriteLine(distances[end]);

       
        }
    }
}
