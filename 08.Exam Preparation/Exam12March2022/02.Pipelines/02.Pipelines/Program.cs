using System;
using System.Collections.Generic;

namespace _02.Pipelines
{
    internal class Program
    {
        private static bool[,] graph;
        private static int[] parent;

        static void Main(string[] args)
        {
            var countOfAgents = int.Parse(Console.ReadLine());
            var countOfPipelines = int.Parse(Console.ReadLine());

            var agents = new string[countOfAgents + 1];
            var pipelines = new string[countOfPipelines + 1];

            for (int i = 1; i <= countOfAgents; i++)
            {
                agents[i] = Console.ReadLine();
            }

            for (int i = 1; i <= countOfPipelines; i++)
            {
                pipelines[i] = Console.ReadLine();
            }

            var nodes = countOfAgents + countOfPipelines + 2;

            graph = new bool[nodes, nodes];

            for (int person = 1; person <= countOfAgents; person++)
            {
                graph[0, person] = true;
            }

            for (int task = countOfAgents + 1; task <= countOfAgents + countOfPipelines; task++)
            {
                graph[task, nodes - 1] = true;
            }

            for (int agent = 0; agent < countOfAgents; agent++)
            {
                var agentPipelines = Console.ReadLine().Split(", ");
                var currentAgent = Array.IndexOf(agents, agentPipelines[0]);

                for (int pipeline = 1; pipeline < agentPipelines.Length; pipeline++)
                {
                    var currentPipeline = Array.IndexOf(pipelines, agentPipelines[pipeline]);

                    graph[currentAgent, countOfAgents + currentPipeline] = true;
                }
            }


            //for (int row = 0; row < graph.GetLength(0); row++)
            //{
            //    for (int col = 0; col < graph.GetLength(1); col++)
            //    {
            //        var result = graph[row, col] ? "Y" : "N";
            //        Console.Write($"{result} ");
            //    }

            //    Console.WriteLine();
            //}

            var source = 0;
            var target = nodes - 1;

            parent = new int[nodes];
            Array.Fill(parent, -1);

            while (BFS(source, target))
            {
                var node = target;

                while (parent[node] != -1)
                {
                    var prev = parent[node];

                    graph[prev, node] = false;
                    graph[node, prev] = true;

                    node = prev;
                }
            }

            //for (int row = 0; row < graph.GetLength(0); row++)
            //{
            //    for (int col = 0; col < graph.GetLength(1); col++)
            //    {
            //        var x = graph[row, col] ? "Y" : "N";
            //        Console.Write($"{x} ");
            //    }

            //    Console.WriteLine();
            //}

            for (int task = countOfAgents + 1; task <= countOfAgents + countOfPipelines; task++)
            {
                for (int idx = 0; idx < graph.GetLength(1); idx++)
                {
                    if (graph[task, idx])
                    {
                        Console.WriteLine($"{agents[idx]} - {pipelines[task - countOfAgents]}");
                    }
                }
            }
        }

        private static bool BFS(int source, int target)
        {
            var visited = new bool[graph.GetLength(0)];
            var queue = new Queue<int>();

            visited[source] = true;
            queue.Enqueue(source);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                for (int child = 0; child < graph.GetLength(1); child++)
                {
                    if (!visited[child]
                        && graph[node, child])
                    {
                        parent[child] = node;
                        visited[child] = true;
                        queue.Enqueue(child);
                    }
                }
            }

            return visited[target];
        }
    }
}
