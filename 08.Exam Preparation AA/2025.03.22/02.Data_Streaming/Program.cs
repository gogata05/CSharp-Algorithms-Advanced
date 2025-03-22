namespace _02.Data_Streaming
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            int c = int.Parse(Console.ReadLine());

            int[,] graph = new int[n, n];

            for (int i = 0; i < c; i++)
            {
                string[] parts = Console.ReadLine().Split();
                int from = int.Parse(parts[0]);
                int to = int.Parse(parts[1]);
                int capacity = int.Parse(parts[2]);

                graph[from, to] = capacity;
            }

            HashSet<int> blacklist = Console.ReadLine()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToHashSet();

            int source = int.Parse(Console.ReadLine());
            int destination = int.Parse(Console.ReadLine());

            int maxFlow = FordFulkerson(graph, source, destination, blacklist, n);

            Console.WriteLine(maxFlow);
        }

        static int FordFulkerson(int[,] graph, int source, int sink, HashSet<int> blacklist, int n)
        {
            int[,] residualGraph = new int[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    residualGraph[i, j] = graph[i, j];
                }
            }

            int[] parent = new int[n];
            int maxFlow = 0;

            while (BFS(residualGraph, source, sink, parent, blacklist, n))
            {
                int pathFlow = int.MaxValue;

                for (int v = sink; v != source; v = parent[v])
                {
                    int u = parent[v];
                    pathFlow = Math.Min(pathFlow, residualGraph[u, v]);
                }

                for (int v = sink; v != source; v = parent[v])
                {
                    int u = parent[v];
                    residualGraph[u, v] -= pathFlow;
                    residualGraph[v, u] += pathFlow;
                }

                maxFlow += pathFlow;
            }

            return maxFlow;
        }

        static bool BFS(int[,] residualGraph, int source, int sink, int[] parent, HashSet<int> blacklist, int n)
        {
            bool[] visited = new bool[n];
            Queue<int> queue = new Queue<int>();

            queue.Enqueue(source);
            visited[source] = true;
            parent[source] = -1;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();

                for (int v = 0; v < n; v++)
                {
                    if (!visited[v] && residualGraph[u, v] > 0 && !blacklist.Contains(v))
                    {
                        queue.Enqueue(v);
                        parent[v] = u;
                        visited[v] = true;
                    }
                }
            }

            return visited[sink];
        }
    }
}
