namespace _03._Guards
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        private static List<int>[] graph;
        private static bool[] visited;

        static void Main(string[] args)
        {
            int nodeCount = int.Parse(Console.ReadLine()!);
            int edgeCount = int.Parse(Console.ReadLine()!);

            graph = new List<int>[nodeCount + 1];

            for (int i = 0; i < graph.Length; i++)
            {
                graph[i] = new List<int>();
            }

            for (int i = 0; i < edgeCount; i++)
            {
                var line = Console.ReadLine()!.Split().Select(int.Parse).ToArray();

                int start = line[0];
                int end = line[1];

                graph[start].Add(end);
            }

            int startNode = int.Parse(Console.ReadLine()!);
            var notReachableNodes = new List<int>();

            for (int i = 1; i <= nodeCount; i++)
            {
                visited = new bool[nodeCount + 1];

                if (IsNotReachable(startNode, i))
                {
                    notReachableNodes.Add(i);
                }
            }

            Console.WriteLine(string.Join(" ", notReachableNodes));
        }

        private static bool IsNotReachable(int node, int endNode)
        {
            if (node == endNode) return false;

            if (visited[node]) return true;

            visited[node] = true;

            foreach (var child in graph[node])
            {
                if (!IsNotReachable(child, endNode)) return false;
            }

            return true;
        }
    }
}
