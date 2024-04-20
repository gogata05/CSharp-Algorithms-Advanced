using System;
using System.Collections.Generic;
using System.Linq;

namespace _1._Strongly_Connected_Components__SCC_
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
            Console.WriteLine("Strongly Connected Components:");
            while (sorted.Count>0)
            {
                int node = sorted.Pop();
                if (visited[node])
                {
                    continue;
                }
                var component = new Stack<int>();
                DFS(reversedGraph,node, visited,component);
                Console.WriteLine($"{{{string.Join(", ",component)}}}");
            }
        }
        /*
        private static void ReverseDFS(int node, bool[] visited,Stack<int> component)
        {
            if (visited[node])
            {
                return;
            }
            visited[node] = true;
            foreach (var child in reversedGraph[node])
            {
                DFS(child, visited, component);
            }
            component.Push(node);
        }
        */
        private static Stack<int> TopologicalSorting()
        {
            var result = new Stack<int>();
            var visited = new bool[graph.Length];
            for (int node = 0; node < graph.Length; node++)
            {
                DFS(graph,node,visited, result);
            }
            return result;
        }

        private static void DFS(List<int>[] source,int node,bool[]visited, Stack<int> stack)
        {
            if (visited[node])
            {
                return;
            }
            visited[node] = true;
            foreach (var child in source[node])
            {
                DFS(source, child, visited, stack);
            }
            stack.Push(node);
        }

        private static (List<int>[] original, List<int>[] reversed) ReadGraph(int nodesCount, int linesCount)
        {
            var result = new List<int>[nodesCount];
            var reversed = new List<int>[nodesCount];
            for (int node = 0; node < nodesCount; node++)
            {
                result[node] = new List<int>();
                reversed[node] = new List<int>();
            }
            for (int i = 0; i < linesCount; i++)
            {
                var data = Console.ReadLine()
                                    .Split(", ")
                                    .Select(int.Parse)
                                    .ToArray();
                var node = data[0];
                for (int j = 1; j < data.Length; j++)
                {
                    var child = data[j];
                    result[node].Add(child);
                    reversed[child].Add(node);
                }
            }
            return (result, reversed);
        }
    }
}
