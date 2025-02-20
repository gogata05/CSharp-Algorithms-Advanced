using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        // Клас, описващ връзката (Edge) между два тумора
        public class Edge
        {
            public int From { get; set; }    // Начален тумор
            public int To { get; set; }      // Краен тумор
            public int Length { get; set; }  // Дължина на връзката

            public Edge(int from, int to, int length)
            {
                From = from;
                To = to;
                Length = length;
            }
        }

        // Клас Union-Find (Disjoint Set) за обединяване на компоненти и избягване на цикли
        public class UnionFind
        {
            private int[] parent;  // Родител на всеки елемент
            private int[] rank;    // Ранг, използван за оптимизация на обединяването

            public UnionFind(int size)
            {
                parent = new int[size];
                rank = new int[size];
                // Инициализация: всеки елемент е сам себе си родител
                for (int i = 0; i < size; i++)
                {
                    parent[i] = i;
                    rank[i] = 1;
                }
            }

            // Метод за намиране на кореновия родител (с оптимизация чрез path compression)
            public int Find(int x)
            {
                if (parent[x] != x)
                {
                    parent[x] = Find(parent[x]);
                }
                return parent[x];
            }

            // Обединяване на два елемента, ако принадлежат на различни компоненти
            public bool Union(int x, int y)
            {
                int rootX = Find(x);
                int rootY = Find(y);

                if (rootX == rootY)
                {
                    // Вече са свързани
                    return false;
                }

                // Обединяване по ранг за оптимизация
                if (rank[rootX] > rank[rootY])
                {
                    parent[rootY] = rootX;
                }
                else if (rank[rootX] < rank[rootY])
                {
                    parent[rootX] = rootY;
                }
                else
                {
                    parent[rootY] = rootX;
                    rank[rootX]++;
                }
                return true;
            }
        }

        static void Main(string[] args)
        {
            // Четене на броя на туморите
            int n;
            string input = Console.ReadLine();
            if (!int.TryParse(input, out n) || n < 0)
            {
                Console.Error.WriteLine("Невалиден брой тумори.");
                Console.WriteLine("0");
                return;
            }

            // Четене на броя на връзките (edges)
            int m;
            input = Console.ReadLine();
            if (!int.TryParse(input, out m) || m < 0)
            {
                Console.Error.WriteLine("Невалиден брой връзки.");
                Console.WriteLine("0");
                return;
            }

            // Ако няма достатъчен брой тумори или връзки, извеждаме 0
            if (n <= 1 || m == 0)
            {
                Console.WriteLine("0");
                return;
            }

            // Списък за съхранение на всички ръбове
            List<Edge> edges = new List<Edge>();
            int maxIndexFound = n - 1;

            // Четене и валидиране на всеки ръб от входа
            for (int i = 0; i < m; i++)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.Error.WriteLine($"Празен ред на входа на ред {i + 1}, пропускаме.");
                    continue;
                }

                // Разделяне на входния ред по интервали и табулации
                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3)
                {
                    Console.Error.WriteLine($"Невалиден формат на ред {i + 1}: {line}");
                    continue;
                }

                int from, to, length;
                if (!int.TryParse(parts[0], out from) ||
                    !int.TryParse(parts[1], out to) ||
                    !int.TryParse(parts[2], out length) ||
                    from < 0 || to < 0 || length < 0)
                {
                    Console.Error.WriteLine($"Невалидни данни на ред {i + 1}: {line}");
                    continue;
                }

                // Актуализиране на максималния намерен индекс, за да избегнем излизане извън обхвата
                if (from > maxIndexFound) maxIndexFound = from;
                if (to > maxIndexFound) maxIndexFound = to;

                // Добавяне на ръба в списъка
                edges.Add(new Edge(from, to, length));
            }

            // Коригиране на общия брой елементи, ако са подадени индекси по-големи от първоначално зададените
            int actualN = Math.Max(n, maxIndexFound + 1);

            // Сортиране на ръбовете по дължина във възходящ ред
            edges = edges.OrderBy(e => e.Length).ToList();

            // Инициализиране на Union-Find структурата
            UnionFind uf = new UnionFind(actualN);
            List<Edge> selectedEdges = new List<Edge>();
            long totalLengthUsed = 0;

            // Избор на ръбовете, които формират минимално покриващо дърво (MST) без цикли
            foreach (var edge in edges)
            {
                if (uf.Union(edge.From, edge.To))
                {
                    selectedEdges.Add(edge);
                    totalLengthUsed += edge.Length;
                }
            }

            // Извеждане на избраните ръбове във формата "от кой до кой"
            foreach (var edge in selectedEdges)
            {
                Console.WriteLine($"{edge.From} {edge.To}");
            }

            // Извеждане на тоталната дължина на избраните ръбове
            Console.WriteLine(totalLengthUsed);
        }
    }
}