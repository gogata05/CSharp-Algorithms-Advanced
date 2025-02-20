using System;
using System.Collections.Generic;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = int.Parse(Console.ReadLine());
            int M = int.Parse(Console.ReadLine());
            List<int>[] adj = new List<int>[N + 1];
            for (int i = 1; i <= N; i++) adj[i] = new List<int>();
            for (int i = 1; i <= N; i++)
            {
                string line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var p in parts) adj[i].Add(int.Parse(p));
                }
            }
            bool[] visited = new bool[N + 1];
            int c = 0;
            for (int i = 1; i <= N; i++)
            {
                if (!visited[i])
                {
                    c++;
                    Queue<int> q = new Queue<int>();
                    q.Enqueue(i);
                    visited[i] = true;
                    while (q.Count > 0)
                    {
                        int u = q.Dequeue();
                        foreach (var v in adj[u])
                        {
                            if (!visited[v])
                            {
                                visited[v] = true;
                                q.Enqueue(v);
                            }
                        }
                    }
                }
            }
            if (c > 1) { Console.WriteLine(0); return; }
            for (int i = 1; i <= N; i++)
            {
                visited = new bool[N + 1];
                int cc = 0;
                for (int j = 1; j <= N; j++)
                {
                    if (j == i) continue;
                    if (!visited[j])
                    {
                        cc++;
                        Queue<int> qq = new Queue<int>();
                        qq.Enqueue(j);
                        visited[j] = true;
                        while (qq.Count > 0)
                        {
                            int u = qq.Dequeue();
                            foreach (var v in adj[u])
                            {
                                if (v == i) continue;
                                if (!visited[v])
                                {
                                    visited[v] = true;
                                    qq.Enqueue(v);
                                }
                            }
                        }
                    }
                }
                if (cc == M) { Console.WriteLine(i); return; }
            }
            Console.WriteLine(0);
        }
    }
}
