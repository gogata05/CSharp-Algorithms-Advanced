using System;
using System.Collections.Generic;
using System.Linq;

namespace _2._Cheap_Town_Tour
{
    public class Edge
    {
        public int First { get; set; }
        public int Second { get; set; }
        public int Weight { get; set; }

        public override string ToString()
        {
            return $"{this.First} {this.Second} {this.Weight}";
        }

    }
    public class Program
    {
        private static List<Edge> edges;
        static void Main(string[] args)
        {
            int nodesCount = int.Parse(Console.ReadLine());
            int edgesCount = int.Parse(Console.ReadLine());
            edges = ReadGraph(edgesCount);
            var sortedEdges = edges
                .OrderBy(e => e.Weight)
                .ToList();
            var root = new int[nodesCount];
            for (int node = 0; node < nodesCount; node++)
            {
                root[node] = node;
            }
            int totalCost = 0;
            foreach (var edge in sortedEdges)
            {
                var firstRoot = GetRoot(edge.First, root);
                var secondRoot = GetRoot(edge.Second, root);
                if (firstRoot!=secondRoot)
                {
                    root[firstRoot] = secondRoot;
                    totalCost += edge.Weight;
                }
            }
            Console.WriteLine("Total cost: {0}",totalCost);
        }

        private static int GetRoot(int node, int[] root)
        {
            while (node!=root[node])
            {
                node = root[node];
            }
            return node;
        }

        private static List<Edge> ReadGraph(int edgesCount)
        {
            var result = new List<Edge>();
            for (int i = 0; i < edgesCount; i++)
            {
                var edgeData = Console.ReadLine()
                    .Split(new[] { " - " },StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToArray();
                int first = edgeData[0];
                int second = edgeData[1];
                int weight = edgeData[2];
                result.Add(new Edge
                {
                    First = first,
                    Second = second,
                    Weight = weight
                }
                    );
            }
            return result;
        }
    }
}
