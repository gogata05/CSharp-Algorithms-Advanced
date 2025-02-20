using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace _01._Tour_de_Sofia
{
    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Distance { get; set; }
    }
    public class Program
    {
        private static List<Edge>[] graph;
        static void Main(string[] args)
        {
            int nodes = int.Parse(Console.ReadLine());
            int edges = int.Parse(Console.ReadLine());
            int startNode = int.Parse(Console.ReadLine());
            graph = ReadGraph(nodes, edges);
            var distance = new double[nodes];
            for (int node = 0; node < distance.Length; node++)
            {
                distance[node] = double.PositiveInfinity;
            }
            var queue = new OrderedBag<int>(
                Comparer<int>.Create((f, s) => distance[f].CompareTo(distance[s])));
            foreach (var edge in graph[startNode])
            {
                queue.Add(edge.To);
                distance[edge.To] = edge.Distance;
            }
            var visitedNodes = new HashSet<int> { startNode};
            
            while (queue.Count>0)
            {
                int node = queue.RemoveFirst();
                visitedNodes.Add(node);

                if (node==startNode)
                {
                    break;
                }
                foreach (var edge in graph[node])
                {
                    int child = edge.To;
                    if (double.IsPositiveInfinity(distance[child]))
                    {
                        queue.Add(child);
                    }
                    double newDistance = distance[node] + edge.Distance;
                    if (newDistance<distance[child])
                    {
                        distance[child] = newDistance;
                        queue = new OrderedBag<int>(
                            queue,
                            Comparer<int>.Create((f, s) => distance[f].CompareTo(distance[s])));

                    }
                }
            }
            if (double.IsPositiveInfinity(distance[startNode]))
            {
                Console.WriteLine(visitedNodes.Count);
            }
            else
            {
                Console.WriteLine(distance[startNode]);
            }
        }

        private static List<Edge>[] ReadGraph(int nodes, int edges)
        {
            var result = new List<Edge>[nodes];
            for (int node = 0; node < result.Length; node++)
            {
                result[node] = new List<Edge>();
            }
            for (int i = 0; i < edges; i++)
            {
                var edgeData = Console.ReadLine()
                    .Split()
                    .Select(int.Parse)
                    .ToArray();
                int from = edgeData[0];
                int to = edgeData[1];
                int distance = edgeData[2];
                result[from].Add(new Edge
                {
                    From = from,
                    To = to,
                    Distance = distance
                }
                    );
            }
            return result;
        }
    }
}
