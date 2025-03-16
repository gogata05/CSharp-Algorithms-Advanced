namespace _02._Chainalysis
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        private static Dictionary<char, List<char>> graph = new Dictionary<char, List<char>>();
        private static HashSet<char> visited = new HashSet<char>();

        static void Main(string[] args)
        {
            int linkCount = int.Parse(Console.ReadLine()!);

            for (int i = 0; i < linkCount; i++)
            {
                var line = Console.ReadLine()!.Split();

                var sender = char.Parse(line[0]);
                var reciever = char.Parse(line[1]);

                if (!graph.ContainsKey(sender))
                {
                    graph[sender] = new List<char>();
                }
                graph[sender].Add(reciever);

                if (!graph.ContainsKey(reciever))
                {
                    graph[reciever] = new List<char>();
                }
                graph[reciever].Add(sender);
            }

            int groupsCount = 0;

            foreach (var node in graph.Keys)
            {
                int elementsCount = DFS(node);

                if (elementsCount > 0)
                {
                    groupsCount++;
                }
            }

            Console.WriteLine(groupsCount);
        }

        private static int DFS(char node)
        {
            if (visited.Contains(node))
            {
                return 0;
            }

            visited.Add(node);
            int elCount = 1;

            foreach (var child in graph[node])
            {
               elCount += DFS(child);
            }

            return elCount;
        }
    }
}
