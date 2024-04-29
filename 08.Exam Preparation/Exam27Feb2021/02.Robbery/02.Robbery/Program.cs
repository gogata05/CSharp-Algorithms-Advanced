using System;
using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace _02.Robbery
{
    public class Edge
    {
        public int First { get; set; }

        public int Second { get; set; }

        public int Weigth { get; set; }
    }

    internal class Program
    {
        private static Dictionary<int, List<Edge>> edgesByNode;
        private static double[] distance;
        private static int[] parent;
        private static bool[] cameras;

        static void Main(string[] args)
        {
            var nodesCount = int.Parse(Console.ReadLine());
            var edgesCount = int.Parse(Console.ReadLine());

            edgesByNode = new Dictionary<int, List<Edge>>();

            for (int i = 0; i < edgesCount; i++)
            {
                var edgeArgs = Console.ReadLine()
                                .Split()
                                .Select(int.Parse)
                                .ToArray();

                var firstNode = edgeArgs[0];
                var secondNode = edgeArgs[1];

                var edge = new Edge
                {
                    First = firstNode,
                    Second = secondNode,
                    Weigth = edgeArgs[2]
                };

                if (!edgesByNode.ContainsKey(firstNode))
                {
                    edgesByNode.Add(firstNode, new List<Edge>());
                }

                if (!edgesByNode.ContainsKey(secondNode))
                {
                    edgesByNode.Add(secondNode, new List<Edge>());
                }

                edgesByNode[firstNode].Add(edge);
                edgesByNode[secondNode].Add(edge);
            }

            var cameraArgs = Console.ReadLine().Split();
            cameras = ReadCameras(nodesCount, cameraArgs);

            var startNode = int.Parse(Console.ReadLine());
            var endNode = int.Parse(Console.ReadLine());

            var biggestNode = edgesByNode.Keys.Max();

            distance = new double[biggestNode + 1];

            for (int node = 0; node < distance.Length; node++)
            {
                distance[node] = double.PositiveInfinity;
            }

            parent = new int[biggestNode + 1];
            //Array.Fill(parent, -1);

            for (int node = 0; node < parent.Length; node++)
            {
                parent[node] = -1;
            }

            distance[startNode] = 0;

            var bag = new OrderedBag<int>(Comparer<int>.Create((f, s) => (int)(distance[f] - distance[s])));
            bag.Add(startNode);

            while (bag.Count > 0)
            {
                var minNode = bag.RemoveFirst();

                if (double.IsPositiveInfinity(minNode))
                {
                    break;
                }

                if (minNode == endNode)
                {
                    break;
                }

                foreach (var edge in edgesByNode[minNode])
                {
                    var otherNode = edge.First == minNode
                                    ? edge.Second
                                    : edge.First;

                    if (cameras[otherNode])
                    {
                        continue;
                    }

                    if (double.IsPositiveInfinity(distance[otherNode]))
                    {
                        bag.Add(otherNode);
                    }

                    var newDistance = distance[minNode] + edge.Weigth;

                    if (newDistance < distance[otherNode])
                    {
                        parent[otherNode] = minNode;

                        distance[otherNode] = newDistance;

                        bag = new OrderedBag<int>(
                           bag,
                           Comparer<int>.Create((f, s) => (int)(distance[f] - distance[s])));

                    }
                }
            }

            Console.WriteLine(distance[endNode]);
        }

        private static bool[] ReadCameras(int nodesCount, string[] cameraArgs)
        {
            var result = new bool[nodesCount];

            for (int i = 0; i < cameraArgs.Length; i++)
            {
                var blackOrWhite = cameraArgs[i][1];

                if (blackOrWhite == 'b')
                {
                    result[i] = false;
                }
                else
                {
                    result[i] = true;
                }
            }

            return result;
        }
    }
}
