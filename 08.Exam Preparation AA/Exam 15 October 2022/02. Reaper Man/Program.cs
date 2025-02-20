using System;
using System.Collections.Generic;
using System.Linq;

namespace ReaperMan
{
    class Program
    {
        static void Main(string[] args)
        {

            // 1. Прочитаме n - брой "блокирани" пътища (взаимно изключващи се)
            //    В тази задача обаче n не се използва директно в логиката, 
            //    освен, че знаем, че ако има дублиращ се път, последният го блокира предишния.
            int n = int.Parse(Console.ReadLine());

            // 2. Прочитаме m - общият брой пътища
            int m = int.Parse(Console.ReadLine());

            // 3. Прочитаме начална и крайна позиция
            // ПРОМЕНЕНО: Подаваме new[] {' '} + StringSplitOptions.RemoveEmptyEntries
            string[] startEndTokens = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int start = int.Parse(startEndTokens[0]);
            int end = int.Parse(startEndTokens[1]);

            // Използваме речник с ключ (from, to) за да пазим САМО последно прочетените дублиращи пътища
            var edgesDict = new Dictionary<(int, int), int>();

            // 4. Прочитаме m реда с формата {from} {to} {distance}
            for (int i = 0; i < m; i++)
            {
                // ПРОМЕНЕНО: Подаваме new[] {' '} + StringSplitOptions.RemoveEmptyEntries
                string[] lineParts = Console.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                int from = int.Parse(lineParts[0]);
                int to = int.Parse(lineParts[1]);
                int distance = int.Parse(lineParts[2]);

                // Ако вече има (from, to), заместваме старата стойност с новата (т.е. блокираме старата и ползваме новата)
                edgesDict[(from, to)] = distance;
            }

            // Създаваме adjacency list, за да изпълним Дейкстра
            // Структура: graph[node] = List<(neighbor, cost)>
            var graph = new Dictionary<int, List<(int neighbor, int cost)>>();

            // Инициализираме речника за всеки възможен връх
            // Тук не знаем общ брой върхове, но можем да вземем макс id от всичко, 
            // или просто да пълним речника динамично
            foreach (var kvp in edgesDict)
            {
                (int from, int to) = kvp.Key;
                if (!graph.ContainsKey(from))
                {
                    graph[from] = new List<(int, int)>();
                }

                // Добавяме съседа
                graph[from].Add((to, kvp.Value));

                // За да е насочен граф, оставяме само from->to. 
                // Ако е двупосочен, бихме добавили и to->from. Задачата не го уточнява ясно, но 
                // примерите изглеждат насочени. Ако беше нужно, щяхме да добавим още:
                // if (!graph.ContainsKey(to)) graph[to] = new List<(int, int)>();
                // graph[to].Add((from, kvp.Value));
            }

            // За да приложим алгоритъма на Дейкстра, дефинираме масиви за дистанции и родители
            // Ще си вземем всички уникални върхове от ключовете на graph
            // (възможно е някои върхове да не са "from", но да се появяват само като "to" – 
            //  за коректност можем да добавим и тях)
            var allNodes = new HashSet<int>();
            // Добавяме от речника
            foreach (var kvp in graph)
            {
                allNodes.Add(kvp.Key);
                foreach (var neigh in kvp.Value)
                {
                    allNodes.Add(neigh.neighbor);
                }
            }
            
            // Масив за разстояния – по подразбиране int.MaxValue
            var distances = new Dictionary<int, int>();
            // Масив за родители – за да реконструираме пътя
            var parents = new Dictionary<int, int>();
            foreach (var node in allNodes)
            {
                distances[node] = int.MaxValue;
                // Няма валиден родител -> -1
                parents[node] = -1;
            }

            // Инициализираме старта
            if (!distances.ContainsKey(start))
            {
                // Ако няма такъв връх, не можем да намерим път
                Console.WriteLine($"[ERROR]: Невалиден стартов връх ({start}).");
                return;
            }

            distances[start] = 0;

            // Мин-купа (приоритетна опашка). В .NET 3.1 можем да си я направим с SortedSet 
            // или да използваме проста List + сортиране, но оптимално е да направим така:
            var priorityQueue = new SortedSet<(int dist, int node)>(Comparer<(int dist, int node)>.Create((x, y) =>
            {
                int result = x.dist.CompareTo(y.dist);
                if (result == 0)
                {
                    // Ако разстоянията са равни, сравняваме по node за да няма дублиране
                    result = x.node.CompareTo(y.node);
                }
                return result;
            }));

            // Добавяме началния връх в priorityQueue
            priorityQueue.Add((0, start));

            // Алгоритъм на Дейкстра
            while (priorityQueue.Count > 0)
            {
                var current = priorityQueue.Min; // взимаме най-малкия
                priorityQueue.Remove(current);

                int currentNode = current.node;
                int currentDist = current.dist;

                if (currentDist > distances[currentNode])
                {
                    // Този запис е остарял
                    continue;
                }

                // Ако стигнем до end, може да прекратим – намерили сме най-краткия път
                if (currentNode == end)
                {
                    break;
                }

                // Обхождаме съседите
                if (!graph.ContainsKey(currentNode))
                {
                    // Ако текущият връх няма наследници, прескачаме
                    continue;
                }

                foreach (var (neighbor, cost) in graph[currentNode])
                {
                    if (distances[currentNode] == int.MaxValue)
                    {
                        // Ако текущият е недостижим, не можем да подобрим нищо
                        break;
                    }

                    int newDistance = distances[currentNode] + cost;
                    if (newDistance < distances[neighbor])
                    {
                        distances[neighbor] = newDistance;
                        parents[neighbor] = currentNode;

                        priorityQueue.Add((newDistance, neighbor));
                    }
                }
            }

            // Проверяваме дали имаме достъп до end
            if (!distances.ContainsKey(end) || distances[end] == int.MaxValue)
            {
                Console.WriteLine("[INFO]: Няма път от началния до крайния връх!");
                return;
            }

            // Реконструираме пътя чрез родителите
            var path = new Stack<int>();
            int nodeToReconstruct = end;
            while (nodeToReconstruct != -1)
            {
                path.Push(nodeToReconstruct);
                nodeToReconstruct = parents[nodeToReconstruct];
            }

            // 5. Отпечатваме пътя
            Console.WriteLine(string.Join(" ", path) + " ");

            // 6. Отпечатваме общото разстояние
            Console.WriteLine(distances[end]);
        }
    }
}
