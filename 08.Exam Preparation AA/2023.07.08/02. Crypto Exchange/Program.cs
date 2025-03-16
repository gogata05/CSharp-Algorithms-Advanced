namespace _02._Crypto_Exchange
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        private static Dictionary<string, List<string>> assetsGraph = new Dictionary<string, List<string>>();
        private static HashSet<string> visited = new HashSet<string>();
        private static Dictionary<string, string> parents;

        static void Main(string[] args)
        {
            int edgesCount = int.Parse(Console.ReadLine()!);

            for (int i = 0; i < edgesCount; i++)
            {
                var input = Console.ReadLine()!.Split(" - ");

                var firstAsset = input[0];
                var secondAsset = input[1];

                if(!assetsGraph.ContainsKey(firstAsset))
                {
                    assetsGraph.Add(firstAsset, new List<string>());
                }

                if(!assetsGraph.ContainsKey(secondAsset))
                {
                    assetsGraph.Add(secondAsset, new List<string>());
                }

                assetsGraph[firstAsset].Add(secondAsset);
                assetsGraph[secondAsset].Add(firstAsset);
            }

            var task = Console.ReadLine()!.Split(" -> ");

            var startAsset = task[0];
            var endAsset = task[1];

            if(!assetsGraph.ContainsKey(startAsset) || !assetsGraph.ContainsKey(endAsset))
            {
                Console.WriteLine(-1);
                return;
            }

            int tradersCount = FindCountOfTradersToReachEnd(startAsset, endAsset);

            Console.WriteLine(tradersCount);
        }

        private static int FindCountOfTradersToReachEnd(string startAsset, string endAsset)
        {
            parents = new Dictionary<string, string>();
            var queue = new Queue<string>();
            queue.Enqueue(startAsset);
            visited.Add(startAsset);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                if(node == endAsset)
                {
                    return GetPathCount(node);
                }

                foreach (var child in assetsGraph[node])
                {
                    if (!visited.Contains(child))
                    {
                        queue.Enqueue(child);
                        visited.Add(child);
                        parents.Add(child, node);
                    }
                }
            }

            return -1;
        }

        private static int GetPathCount(string node)
        {
            int result = 0;

            while (parents.ContainsKey(node))
            {
                result++;
                node = parents[node];
            }

            return result;
        }
    }
}
