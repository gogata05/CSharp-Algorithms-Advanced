using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace _01.BlackFriday
{
    public class Edge
    {
        public int First { get; set; }

        public int Second { get; set; }

        public int Weight { get; set; }
    }

    internal class Program
    {
        private static Dictionary<int, List<Edge>> graph;
        private static HashSet<int> forestNodes;
        private static List<Edge> forestEdges;

        static void Main(string[] args)
        {
            graph = new Dictionary<int, List<Edge>>();
            forestNodes = new HashSet<int>();
            forestEdges = new List<Edge>();

            var nodes = int.Parse(Console.ReadLine());
            var edges = int.Parse(Console.ReadLine());

            for (int i = 0; i < edges; i++)
            {
                var edgeData = Console.ReadLine()
                                .Split()
                                .Select(int.Parse)
                                .ToArray();

                var firstNode = edgeData[0];
                var secondNode = edgeData[1];

                if (!graph.ContainsKey(firstNode))
                {
                    graph.Add(firstNode, new List<Edge>());
                }

                if (!graph.ContainsKey(secondNode))
                {
                    graph.Add(secondNode, new List<Edge>());
                }

                var edge = new Edge
                {
                    First = firstNode,
                    Second = secondNode,
                    Weight = edgeData[2]
                };

                graph[firstNode].Add(edge);
                graph[secondNode].Add(edge);
            }

            foreach (var node in graph.Keys)
            {
                if (!forestNodes.Contains(node))
                {
                    Prim(node);
                }
            }

            var totalTime = 0;

            foreach (var edge in forestEdges)
            {
                totalTime += edge.Weight;
            }

            Console.WriteLine(totalTime);
        }

        private static void Prim(int startingNode)
        {
            forestNodes.Add(startingNode);

            var bag = new OrderedBag<Edge>(
                        Comparer<Edge>.Create((f, s) => f.Weight - s.Weight));

            bag.AddMany(graph[startingNode]);

            while (bag.Count > 0)
            {
                var minEdge = bag.RemoveFirst();

                var nonTreeNode = -1;

                if (forestNodes.Contains(minEdge.First) &&
                    !forestNodes.Contains(minEdge.Second))
                {
                    nonTreeNode = minEdge.Second;
                }

                if (forestNodes.Contains(minEdge.Second) &&
                    !forestNodes.Contains(minEdge.First))
                {
                    nonTreeNode = minEdge.First;
                }

                if (nonTreeNode == -1)
                {
                    continue;
                }

                forestNodes.Add(nonTreeNode);
                forestEdges.Add(minEdge);
                bag.AddMany(graph[nonTreeNode]);
            }
        }
    }
}
