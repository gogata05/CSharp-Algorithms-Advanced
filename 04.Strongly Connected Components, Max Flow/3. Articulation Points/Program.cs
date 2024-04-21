using System;
using System.Collections.Generic;
using System.Linq;

namespace _3._Articulation_Points
{
    public class Program
    {
        private static List<int>[] graph;
        private static int[] depths;
        private static int[] lowpoint;
        private static int[] parent;
        private static bool[] visited;
        private static List<int> articulationPoints;
        static void Main(string[] args)
        {
            int nodesCount = int.Parse(Console.ReadLine());
            int linesCount = int.Parse(Console.ReadLine());
            graph = ReadGraph(nodesCount, linesCount);
            depths = new int[nodesCount];
            lowpoint = new int[nodesCount];
            visited = new bool[nodesCount];
            parent = new int[nodesCount];
            articulationPoints = new List<int>();
            Array.Fill(parent, -1);
            for (int node = 0; node < graph.Length; node++)
            {
                if (!visited[node])
                {
                    FindArticulationPoint(node, 1);
                }
            }
            Console.WriteLine($"Articulation points: {string.Join(", ",articulationPoints)}");
        }

        private static void FindArticulationPoint(int node, int depth)
        {
            visited[node] = true;
            lowpoint[node] = depth;
            depths[node] = depth;
            int childCount = 0;
            bool isArticulationPoint = false;
            foreach (var child in graph[node])
            {
                if (!visited[child])
                {
                    parent[child] = node;
                    childCount += 1;
                    FindArticulationPoint(child, depth + 1);

                    if (lowpoint[child] >= depth)
                    {
                        isArticulationPoint = true;

                    }
                    lowpoint[node] = Math.Min(lowpoint[node], lowpoint[child]);
                }
                else if (parent[node]!=child)
                {
                    lowpoint[node] = Math.Min(lowpoint[node], depths[child]);

                }
            }
            if ((parent[node]==-1&&childCount>1)||(parent[node]!=-1&&isArticulationPoint))
            {
                articulationPoints.Add(node);
            }
        }
        
        private static List<int>[] ReadGraph(int nodesCount, int linesCount)
        {
            var result = new List<int>[nodesCount];
            for (int node = 0; node < result.Length; node++)
            {
                result[node] = new List<int>();
            }
            for (int i = 0; i < linesCount; i++)
            {
                var parts = Console.ReadLine()
                    .Split(", ")
                    .Select(int.Parse)
                    .ToArray();
                var first = parts[0];
                for (int j = 1; j < parts.Length; j++)
                {
                    int second = parts[j];
                    result[first].Add(second);
                    result[second].Add(first);
                }
            }
            return result;
        }
    }
}
