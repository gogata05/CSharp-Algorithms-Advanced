using System;
using System.Collections.Generic;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            int m = int.Parse(Console.ReadLine());
            string[] parts = Console.ReadLine().Split();
            int source = int.Parse(parts[0]);
            int sink = int.Parse(parts[1]);
            MaxFlow flowSolver = new MaxFlow(n);
            for (int i = 0; i < m; i++)
            {
                string[] input = Console.ReadLine().Split();
                int u = int.Parse(input[0]);
                int v = int.Parse(input[1]);
                int capacity = int.Parse(input[2]);
                flowSolver.AddEdge(u, v, capacity);
            }
            Console.WriteLine(flowSolver.GetMaxFlow(source, sink));
        }
    }
    public class MaxFlow
    {
        int n;
        List<Edge>[] adj;
        int[] level;
        int[] ptr;
        public MaxFlow(int n)
        {
            this.n = n;
            adj = new List<Edge>[n];
            for (int i = 0; i < n; i++)
                adj[i] = new List<Edge>();
            level = new int[n];
            ptr = new int[n];
        }
        public void AddEdge(int u, int v, int cap)
        {
            adj[u].Add(new Edge(v, cap, adj[v].Count));
            adj[v].Add(new Edge(u, 0, adj[u].Count - 1));
        }
        bool Bfs(int s, int t)
        {
            for (int i = 0; i < level.Length; i++)
                level[i] = -1;
            level[s] = 0;
            Queue<int> q = new Queue<int>();
            q.Enqueue(s);
            while (q.Count > 0)
            {
                int u = q.Dequeue();
                foreach (Edge e in adj[u])
                {
                    if (level[e.to] < 0 && e.flow < e.cap)
                    {
                        level[e.to] = level[u] + 1;
                        q.Enqueue(e.to);
                    }
                }
            }
            return level[t] >= 0;
        }
        int Dfs(int u, int t, int pushed)
        {
            if (pushed == 0)
                return 0;
            if (u == t)
                return pushed;
            for (; ptr[u] < adj[u].Count; ptr[u]++)
            {
                Edge e = adj[u][ptr[u]];
                if (level[e.to] != level[u] + 1 || e.flow >= e.cap)
                    continue;
                int tr = Dfs(e.to, t, Math.Min(pushed, e.cap - e.flow));
                if (tr > 0)
                {
                    e.flow += tr;
                    adj[e.to][e.rev].flow -= tr;
                    return tr;
                }
            }
            return 0;
        }
        public int GetMaxFlow(int s, int t)
        {
            if (s == t) return 0;
            int flow = 0;
            while (Bfs(s, t))
            {
                for (int i = 0; i < ptr.Length; i++)
                    ptr[i] = 0;
                while (true)
                {
                    int pushed = Dfs(s, t, int.MaxValue);
                    if (pushed == 0)
                        break;
                    flow += pushed;
                }
            }
            return flow;
        }
    }
    public class Edge
    {
        public int to;
        public int cap;
        public int flow;
        public int rev;
        public Edge(int to, int cap, int rev)
        {
            this.to = to;
            this.cap = cap;
            this.rev = rev;
            flow = 0;
        }
    }
}
