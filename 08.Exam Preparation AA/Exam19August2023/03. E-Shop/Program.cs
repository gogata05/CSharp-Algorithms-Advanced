using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Item
    {
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Value { get; set; }
        public HashSet<string> Dependencies { get; set; } = new HashSet<string>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var items = new Dictionary<string, Item>();

            for (int i = 0; i < n; i++)
            {
                var tokens = Console.ReadLine().Split();
                items[tokens[0]] = new Item
                {
                    Name = tokens[0],
                    Weight = int.Parse(tokens[1]),
                    Value = int.Parse(tokens[2])
                };
            }

            var p = int.Parse(Console.ReadLine());
            var graph = new Dictionary<string, HashSet<string>>();

            for (int i = 0; i < p; i++)
            {
                var tokens = Console.ReadLine().Split();
                if (!graph.ContainsKey(tokens[0]))
                    graph[tokens[0]] = new HashSet<string>();
                if (!graph.ContainsKey(tokens[1]))
                    graph[tokens[1]] = new HashSet<string>();

                graph[tokens[0]].Add(tokens[1]);
                graph[tokens[1]].Add(tokens[0]);
            }

            foreach (var item in items.Keys.ToList())
            {
                if (graph.ContainsKey(item))
                {
                    var visited = new HashSet<string>();
                    DFS(item, graph, visited);
                    visited.Remove(item);
                    items[item].Dependencies = visited;
                }
            }

            var capacity = int.Parse(Console.ReadLine());
            var bestValue = 0;
            HashSet<string> bestCombination = new HashSet<string>();

            var combinations = GetAllCombinations(items.Keys.ToList());
            foreach (var combination in combinations)
            {
                var currentItems = new HashSet<string>(combination);
                foreach (var item in combination)
                {
                    foreach (var dep in items[item].Dependencies)
                        currentItems.Add(dep);
                }

                var totalWeight = currentItems.Sum(x => items[x].Weight);
                var totalValue = currentItems.Sum(x => items[x].Value);

                if (totalWeight <= capacity && totalValue > bestValue)
                {
                    bestValue = totalValue;
                    bestCombination = currentItems;
                }
            }

            foreach (var item in bestCombination.OrderBy(x => x))
            {
                Console.WriteLine(item);
            }
        }

        static List<List<string>> GetAllCombinations(List<string> items)
        {
            var result = new List<List<string>>();
            for (int i = 1; i <= items.Count; i++)
            {
                GetCombinations(items, i, new List<string>(), 0, result);
            }
            return result;
        }

        static void GetCombinations(List<string> items, int length, List<string> current, int start, List<List<string>> result)
        {
            if (current.Count == length)
            {
                result.Add(new List<string>(current));
                return;
            }

            for (int i = start; i < items.Count; i++)
            {
                current.Add(items[i]);
                GetCombinations(items, length, current, i + 1, result);
                current.RemoveAt(current.Count - 1);
            }
        }

        static void DFS(string node, Dictionary<string, HashSet<string>> graph, HashSet<string> visited)
        {
            if (visited.Contains(node))
                return;

            visited.Add(node);
            if (graph.ContainsKey(node))
            {
                foreach (var child in graph[node])
                {
                    DFS(child, graph, visited);
                }
            }
        }
    }
}
