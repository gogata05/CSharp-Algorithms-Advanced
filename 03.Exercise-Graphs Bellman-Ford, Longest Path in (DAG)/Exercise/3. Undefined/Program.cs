using System;
using System.Collections.Generic;
using System.Linq;

namespace _3._Undefined
{
    public class Edge
    {
        public int From { get; set; }
        public int To { get; set; }
        public int Weight { get; set; }

        public override string ToString()
        {
            return $"{this.From} {this.To} {this.Weight}";
        }

    }
    public class Program
    {
        private static List<Edge> edges;
        static void Main(string[] args)
        {
            int nodesCount = int.Parse(Console.ReadLine());
            int edgesCount = int.Parse(Console.ReadLine());
            edges = ReadGraph(edgesCount);
            int source = int.Parse(Console.ReadLine());
            int destination = int.Parse(Console.ReadLine());
            var distances = new double[nodesCount+1];
            var prev = new int[nodesCount+1];
            for (int node = 0; node < nodesCount+1; node++)
            {
                distances[node] = double.PositiveInfinity;
                prev[node] = -1;
            }
            distances[source] = 0;
            for (int i = 0; i < nodesCount-1; i++)
            {
                bool updated = false;
                foreach (var edge in edges)
                {
                    if (double.IsPositiveInfinity(distances[edge.From]))
                    {
                        continue;
                    }
                        double newDistance = distances[edge.From] + edge.Weight;
                        if (newDistance<distances[edge.To])
                        {
                            distances[edge.To] = newDistance;
                            prev[edge.To] = edge.From;
                            updated = true;
                        }
                    }
                    if (!updated)
                    {
                        break;
                    }
                }
            foreach (var edge in edges)
            {
                double newDistance = distances[edge.From] + edge.Weight;
                if (newDistance<distances[edge.To])
                {
                    Console.WriteLine("Undefined");
                    return;
                }
            }
            var path = GetPath(prev,destination);
            Console.WriteLine(string.Join(" ",path));
            Console.WriteLine(distances[destination]);
        }

        private static Stack<int> GetPath(int[] prev, int node)
        {
            var path = new Stack<int>();
            while (node!=-1)
            {
                path.Push(node);
                node = prev[node];
            }
            return path;
        }

        private static List<Edge> ReadGraph(int edgesCount)
        {
            var result = new List<Edge>();
            for (int i = 0; i < edgesCount; i++)
            {
                var edgeData = Console.ReadLine()
                    .Split()
                    .Select(int.Parse)
                    .ToArray();
                int first = edgeData[0];
                int second = edgeData[1];
                int weight = edgeData[2];
                result.Add(new Edge
                {
                    From = first,
                    To = second,
                    Weight = weight
                }
                    );
            }
            return result;
        }
    }
}
