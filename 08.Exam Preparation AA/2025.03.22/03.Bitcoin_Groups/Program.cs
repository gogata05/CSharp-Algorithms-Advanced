namespace _03.Bitcoin_Groups
{
    class Program
    {
        static void Main(string[] args)
        {
            int w = int.Parse(Console.ReadLine());
            int t = int.Parse(Console.ReadLine());

            List<int>[] graph = new List<int>[w];
            List<(int, int)> allTransactions = new List<(int, int)>();

            for (int i = 0; i < w; i++)
            {
                graph[i] = new List<int>();
            }

            for (int i = 0; i < t; i++)
            {
                string[] transaction = Console.ReadLine().Split();
                int sender = int.Parse(transaction[0]);
                int receiver = int.Parse(transaction[1]);

                graph[sender].Add(receiver);
                allTransactions.Add((sender, receiver));
            }

            List<List<int>> stronglyConnectedComponents = FindStronglyConnectedComponents(graph, w);

            List<int> largestComponent = stronglyConnectedComponents.OrderByDescending(c => c.Count).First();

            HashSet<int> largestComponentSet = new HashSet<int>(largestComponent);

            foreach (var transaction in allTransactions)
            {
                if (largestComponentSet.Contains(transaction.Item1) && largestComponentSet.Contains(transaction.Item2))
                {
                    Console.WriteLine($"{transaction.Item1} -> {transaction.Item2}");
                }
            }
        }

        static List<List<int>> FindStronglyConnectedComponents(List<int>[] graph, int n)
        {
            bool[] visited = new bool[n];
            Stack<int> finishOrder = new Stack<int>();

            for (int i = 0; i < n; i++)
            {
                if (!visited[i])
                {
                    DFS1(graph, i, visited, finishOrder);
                }
            }

            List<int>[] transposedGraph = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                transposedGraph[i] = new List<int>();
            }

            for (int i = 0; i < n; i++)
            {
                foreach (int neighbor in graph[i])
                {
                    transposedGraph[neighbor].Add(i);
                }
            }

            Array.Fill(visited, false);

            List<List<int>> stronglyConnectedComponents = new List<List<int>>();

            while (finishOrder.Count > 0)
            {
                int vertex = finishOrder.Pop();

                if (!visited[vertex])
                {
                    List<int> component = new List<int>();
                    DFS2(transposedGraph, vertex, visited, component);
                    stronglyConnectedComponents.Add(component);
                }
            }

            return stronglyConnectedComponents;
        }

        static void DFS1(List<int>[] graph, int vertex, bool[] visited, Stack<int> finishOrder)
        {
            visited[vertex] = true;

            foreach (int neighbor in graph[vertex])
            {
                if (!visited[neighbor])
                {
                    DFS1(graph, neighbor, visited, finishOrder);
                }
            }

            finishOrder.Push(vertex);
        }

        static void DFS2(List<int>[] graph, int vertex, bool[] visited, List<int> component)
        {
            visited[vertex] = true;
            component.Add(vertex);

            foreach (int neighbor in graph[vertex])
            {
                if (!visited[neighbor])
                {
                    DFS2(graph, neighbor, visited, component);
                }
            }
        }
    }
}
