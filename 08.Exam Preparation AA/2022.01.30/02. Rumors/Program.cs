namespace _02._Rumors
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        private static List<int>[] network;
        private static bool[] visited;
        private static int[] parent;

        static void Main(string[] args)
        {
            int peopleCount = int.Parse(Console.ReadLine()!);
            int connectionsCount = int.Parse(Console.ReadLine()!);

            network = new List<int>[peopleCount + 1];

            for (int i = 0; i < connectionsCount; i++)
            {
                var edge = Console.ReadLine()!.Split().Select(int.Parse).ToArray();

                var first = edge[0];
                var second = edge[1];

                if (network[first] == null)
                {
                    network[first] = new List<int>();
                }
                network[first].Add(second);

                if (network[second] == null)
                {
                    network[second] = new List<int>();
                }
                network[second].Add(first);
            }

            int startNode = int.Parse(Console.ReadLine()!);

            for (int i = 1; i < network.Length; i++)
            {
                if (startNode == i) continue;

                visited = new bool[network.Length];
                parent = new int[network.Length];

                var timeNeeded = CalcNeededTime(startNode, i);

                if (timeNeeded > 0)
                {
                    Console.WriteLine($"{startNode} -> {i} ({timeNeeded})");
                }
            }
        }

        private static int CalcNeededTime(int node, int endNode)
        {
            var queue = new Queue<int>();
            queue.Enqueue(node);
            visited[node] = true;

            while (queue.Count > 0)
            {
                var curNode = queue.Dequeue();

                if (curNode == endNode)
                {
                    return GetTime(curNode);
                }

                foreach (var child in network[curNode])
                {
                    if (!visited[child])
                    {
                        visited[child] = true;
                        parent[child] = curNode;
                        queue.Enqueue(child);
                    }
                }
            }

            return 0;
        }

        private static int GetTime(int curNode)
        {
            int time = 0;

            while (parent[curNode] > 0)
            {
                time++;
                curNode = parent[curNode];
            }

            return time;
        }
    }
}
