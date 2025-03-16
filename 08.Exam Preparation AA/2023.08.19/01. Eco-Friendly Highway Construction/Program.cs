using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Edge
    {
        public string From { get; set; }
        public string To { get; set; }
        public long Cost { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var edges = new List<Edge>();

            for (int i = 0; i < n; i++)
            {
                var input = Console.ReadLine().Split();
                var from = input[0];
                var to = input[1];
                var cost = long.Parse(input[2]);
                
                if (input.Length > 3)
                {
                    cost += long.Parse(input[3]);
                }

                edges.Add(new Edge { From = from, To = to, Cost = cost });
            }

            var parent = new Dictionary<string, string>();
            var totalCost = 0L;

            edges = edges.OrderBy(e => e.Cost).ToList();

            foreach (var edge in edges)
            {
                var fromRoot = FindRoot(edge.From, parent);
                var toRoot = FindRoot(edge.To, parent);

                if (fromRoot != toRoot)
                {
                    parent[fromRoot] = toRoot;
                    totalCost += edge.Cost;
                }
            }

            Console.WriteLine($"Total cost of building highways: {totalCost}");
        }

        private static string FindRoot(string node, Dictionary<string, string> parent)
        {
            if (!parent.ContainsKey(node))
            {
                parent[node] = node;
            }

            while (node != parent[node])
            {
                parent[node] = parent[parent[node]];
                node = parent[node];
            }

            return node;
        }
    }
}
