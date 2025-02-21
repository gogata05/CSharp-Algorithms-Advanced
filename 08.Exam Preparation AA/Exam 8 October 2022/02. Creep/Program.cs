using System;
using System.Collections.Generic;
using System.Linq;
namespace ConsoleApp1
{
    class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Weight { get; set; }

        public override string ToString()
        {
            return $"{From} {To}";
        }
    }
   
    class Program
    {
        static void Main(string[] args)
        {
            var nodes = int.Parse(Console.ReadLine());
            var edges = int.Parse(Console.ReadLine());

            var graph = new Dictionary<int, Dictionary<int, int>>();

            for (int node = 0; node < nodes +1; node++)
            {
                graph.Add(node, new Dictionary<int, int>());
            }

            for (int i = 0; i < edges; i++)
            {
                var edgeData = Console.ReadLine().Split().Select(int.Parse).ToArray();

                var fromNode = edgeData[0];
                var toNode = edgeData[1];
                var weight = edgeData[2];

                graph[fromNode][toNode] = weight;
            }

            var sortedEdges = new List<Edge>();

            foreach (var node in graph)
            {
                foreach (var child in node.Value)
                {
                    var edge = new Edge
                    {
                        From = node.Key,
                        To = child.Key,
                        Weight = child.Value
                    };
                    sortedEdges.Add(edge);
                }
            }
            sortedEdges = sortedEdges.OrderBy(e => e.Weight).ToList();
            var mst = new List<Edge>();

            var parent = new int[nodes +1];
            for (int node = 0; node < parent.Length; node++)
            {
                parent[node] = node;
            }

            foreach (var edge in sortedEdges)
            {
                var firstNodeRoot = FindRoot(parent, edge.From);
                var secondNodeRoot = FindRoot(parent, edge.To);
                if (firstNodeRoot == secondNodeRoot)
                {
                    continue;
                }
                parent[firstNodeRoot] = secondNodeRoot;
                mst.Add(edge);
            }

            var sum = 0;
            foreach (var edge in mst)
            {
                Console.WriteLine(edge);
                sum += edge.Weight;
            }

            Console.WriteLine(sum);
        }


        private static int FindRoot(int[] parent, int node)
        {
           while (node != parent[node])
           {
            node = parent[node];
           }
           return node;
        }
    }
}
