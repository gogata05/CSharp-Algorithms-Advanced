using System;
using System.Collections.Generic;
using System.Linq;

namespace _4._Big_Trip
{
    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Weight { get; set; }

        public override string ToString()
        {
            return $"{this.From} {this.To} {this.Weight}";
        }

    }
    public class Program
    {
        private static List<Edge>[] graph;
        static void Main(string[] args)
        {
            int nodesCount = int.Parse(Console.ReadLine());
            int edgesCount = int.Parse(Console.ReadLine());
            graph = ReadGraph(nodesCount, edgesCount);
            int source = int.Parse(Console.ReadLine());
            int destination = int.Parse(Console.ReadLine());
            var distances = new double[graph.Length];
            var prev = new int[graph.Length];
            for (int node = 0; node < graph.Length; node++)
            {
                prev[node] = -1;
                distances[node] = double.NegativeInfinity;
            }
            distances[source] = 0;
            var sortedNodes = TopologicalSort();
            while (sortedNodes.Count > 0)
            {
                int node = sortedNodes.Pop();
                foreach (var edge in graph[node])
                {
                    double newDistance = distances[node] + edge.Weight;
                    if (newDistance > distances[edge.To])
                    {
                        prev[edge.To] = node;
                        distances[edge.To] = newDistance;
                    }
                }
            }
            Console.WriteLine(distances[destination]);
            Console.WriteLine(string.Join(" ",GetPath(prev,destination)));
        }

        private static Stack<int> GetPath(int[] prev, int node)
        {
            var path = new Stack<int>();
            while (node != -1)
            {
                path.Push(node);
                node = prev[node];
            }
            return path;
        }

        private static Stack<int> TopologicalSort()
        {
            var visited = new bool[graph.Length];
            var sorted = new Stack<int>();
            for (int node = 1; node < graph.Length; node++)
            {
                DFS(node,visited,sorted);
            }
            return sorted;
        }

        private static void DFS(int node, bool[] visited,Stack<int> sorted)
        {
            if (visited[node])
            {
                return;
            }
            visited[node] = true;
            foreach (var edge in graph[node])
            {
                DFS(edge.To, visited,sorted);
            }
            sorted.Push(node);
        }

        private static List<Edge>[] ReadGraph(int nodesCount, int edgesCount)
        {
            var result = new List<Edge>[nodesCount+1];
            for (int node = 0; node < result.Length; node++)
            {
                result[node] = new List<Edge>();
            }
            for (int i = 0; i < edgesCount; i++)
            {
                var edgeData = Console.ReadLine()
                    .Split()
                    .Select(int.Parse)
                    .ToArray();
                int from = edgeData[0];
                int to = edgeData[1];
                int weight = edgeData[2];
                result[from].Add(new Edge
                {
                    From = from,
                    To = to,
                    Weight = weight
                }
                    );
            }
            return result;
        }
    }
}
