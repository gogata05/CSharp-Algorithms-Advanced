namespace _02._Paths
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        private static List<int>[] graph;
        private static bool[] visited;
        private static int[] parent;

        static void Main(string[] args)
        {
            int nodeCount = int.Parse(Console.ReadLine()!);
            graph = new List<int>[nodeCount];


            for (int i = 0; i < nodeCount; i++)
            {
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    graph[i] = new List<int>();
                }
                else
                {
                    var children = input.Split().Select(int.Parse).ToList();
                    graph[i] = children;
                }
            }

            visited = new bool[nodeCount];
            parent = new int[nodeCount];
            Array.Fill(parent, -1);

            for (int i = 0; i < nodeCount - 1; i++)
            {
                FindPaths(i);
            }
        }

        private static void FindPaths(int node)
        {
            if (node == graph.Length - 1)
            {
                PrintPath(node);
                return;
            }

            if (visited[node])
            {
                return;
            }

            visited[node] = true;

            foreach (var child in graph[node])
            {
                parent[child] = node;

                FindPaths(child);

                parent[child] = -1;
            }

            visited[node] = false;
        }

        private static void PrintPath(int node)
        {
            var path = new Stack<int>();

            while (node >= 0)
            {
                path.Push(node);
                node = parent[node];
            }

            Console.WriteLine(string.Join(" ", path));
        }
    }
}
