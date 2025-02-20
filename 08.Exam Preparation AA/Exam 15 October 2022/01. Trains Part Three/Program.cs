using System;
using System.Collections.Generic;

public class Program
{
    public class Edge
    {
        public int To { get; set; }
        public int Capacity { get; set; }
        public Edge Reverse { get; set; }

        // Добавяме конструктор за по-бърза инициализация
        public Edge(int to, int capacity)
        {
            To = to;
            Capacity = capacity;
        }
    }

    public static void Main()
    {
        // Read input values
        int n = int.Parse(Console.ReadLine());
        int m = int.Parse(Console.ReadLine());
        int[] sourceSink = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
        int source = sourceSink[0];
        int sink = sourceSink[1];

        // Initialize adjacency list for graph
        List<Edge>[] graph = new List<Edge>[n];
        for (int i = 0; i < n; i++)
            graph[i] = new List<Edge>();

        // Build residual graph
        for (int i = 0; i < m; i++)
        {
            int[] tube = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
            int from = tube[0];
            int to = tube[1];
            int capacity = tube[2];

            Edge forward = new Edge(to, capacity);
            Edge reverse = new Edge(from, 0);
            forward.Reverse = reverse;
            reverse.Reverse = forward;
            
            graph[from].Add(forward);
            graph[to].Add(reverse);
        }

        // Добавяме проверка за изолирани върхове
        if (source < 0 || source >= n || sink < 0 || sink >= n)
        {
            Console.WriteLine("Invalid source or sink node");
            return;
        }

        int maxFlow = 0;
        
        // Edmonds-Karp algorithm implementation
        while (true)
        {
            int[] parent = new int[n];
            for (int i = 0; i < n; i++) parent[i] = -1;
            Edge[] pathEdges = new Edge[n];
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            parent[source] = source;

            // BFS to find augmenting path
            while (queue.Count > 0 && parent[sink] == -1)
            {
                int u = queue.Dequeue();
                foreach (Edge e in graph[u])
                {
                    if (parent[e.To] == -1 && e.Capacity > 0)
                    {
                        parent[e.To] = u;
                        pathEdges[e.To] = e;
                        queue.Enqueue(e.To);
                    }
                }
            }

            // No more augmenting paths found
            if (parent[sink] == -1) break;

            // Find minimum residual capacity
            int minFlow = int.MaxValue;
            for (int v = sink; v != source; v = parent[v])
                minFlow = Math.Min(minFlow, pathEdges[v].Capacity);

            // Update residual capacities
            for (int v = sink; v != source; v = parent[v])
            {
                Edge edge = pathEdges[v];
                edge.Capacity -= minFlow;
                edge.Reverse.Capacity += minFlow;
            }

            maxFlow += minFlow;
        }

        Console.WriteLine(maxFlow);
    }
}