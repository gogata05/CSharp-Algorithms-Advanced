using System;
using System.Collections.Generic;
namespace ConsoleApp1
{
    class Program
    {
        class Edge
        {
            public int U;
            public int V;
            public long Weight;
            public Edge(int u, int v, long weight) { U = u; V = v; Weight = weight; }
        }
        static void Main(string[] args)
        {
            int r = int.Parse(Console.ReadLine());
            List<Edge> edges = new List<Edge>();
            Dictionary<string, int> map = new Dictionary<string, int>();
            int id = 0;
            for (int i = 0; i < r; i++)
            {
                string[] parts = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (!map.ContainsKey(parts[0])) { map[parts[0]] = id; id++; }
                if (!map.ContainsKey(parts[1])) { map[parts[1]] = id; id++; }
                long cost = long.Parse(parts[2]);
                long env = parts.Length == 4 ? long.Parse(parts[3]) : 0;
                long weight = cost + env;
                edges.Add(new Edge(map[parts[0]], map[parts[1]], weight));
            }
            string startUser = Console.ReadLine();
            string destUser = Console.ReadLine();
            int start = map[startUser];
            int dest = map[destUser];
            int V = id;
            long[] best = new long[V];
            int[] hops = new int[V];
            for (int i = 0; i < V; i++) { best[i] = -1; hops[i] = int.MaxValue; }
            best[start] = 0; hops[start] = 0;
            for (int i = 1; i < V; i++)
            {
                foreach (var edge in edges)
                {
                    if (best[edge.U] != -1)
                    {
                        long candidate = best[edge.U] + edge.Weight;
                        int candidateHops = hops[edge.U] + 1;
                        if (candidate > best[edge.V])
                        {
                            best[edge.V] = candidate;
                            hops[edge.V] = candidateHops;
                        }
                        else if (candidate == best[edge.V] && candidateHops < hops[edge.V])
                        {
                            hops[edge.V] = candidateHops;
                        }
                    }
                }
            }
            Console.WriteLine($"({best[dest]}, {hops[dest]})");
        }
    }
}
