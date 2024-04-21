using System;
using System.Collections.Generic;
using System.Linq;

namespace _1._Electrical_Substation_Network
{
    public class Program
    {
        private static List<int>[] graph;
        private static List<int>[] reversedGraph;
        private static Stack<int> sorted;
        static void Main(string[] args)
        {
            int nodesCount = int.Parse(Console.ReadLine());
            int linesCount = int.Parse(Console.ReadLine());
            (graph, reversedGraph) = ReadGraph(nodesCount, linesCount);
            sorted = TopologicalSorting();
            var visited = new bool[nodesCount];
            while (sorted.Count > 0)
            {
                int node = sorted.Pop();
                if (visited[node])
                {
                    continue;
                }
                var component = new Stack<int>();
                DFS(reversedGraph, node, visited, component);
                Console.WriteLine(string.Join(", ", component));
            }
        }

        private static Stack<int> TopologicalSorting()
        {
            var stack = new Stack<int>();
            var visited = new bool[graph.Length];
            for (int node = 0; node < graph.Length; node++)
            {
                if (!visited[node])
                {
                    DFS(graph, node, visited, stack);
                }
            }
            return stack;
        }

        private static void DFS(List<int>[] targetGraph, int node, bool[] visited, Stack<int> stack)
        {
            if (visited[node])
            {
                return;
            }
            visited[node] = true;
            foreach (var child in targetGraph[node])
            {
                DFS(targetGraph, child, visited, stack);
            }
            stack.Push(node);
        }

        private static (List<int>[] original, List<int>[] reversedGraph) ReadGraph(int nodesCount, int linesCount)
        {
            var original = new List<int>[nodesCount];
            var reversed = new List<int>[nodesCount];
            for (int node = 0; node < nodesCount; node++)
            {
                original[node] = new List<int>();
                reversed[node] = new List<int>();
            }
            for (int i = 0; i < linesCount; i++)
            {
                var data = Console.ReadLine()
                                    .Split(", ")
                                    .Select(int.Parse)
                                    .ToArray();
                int node = data[0];
                for (int j = 1; j < data.Length; j++)
                {
                    int child = data[j];
                    original[node].Add(child);
                    reversed[child].Add(node);
                }
            }
            return (original, reversed);
        }
    }
}
