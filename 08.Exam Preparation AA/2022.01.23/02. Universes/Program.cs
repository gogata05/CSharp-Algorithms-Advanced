namespace _02._Universes
{
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        private static Dictionary<string, List<string>> planetsGraph = new Dictionary<string, List<string>>();
        private static HashSet<string> visited = new HashSet<string>();

        static void Main(string[] args)
        {
            int linksCount = int.Parse(Console.ReadLine()!);

            for (int i = 0; i < linksCount; i++)
            {
                var link = Console.ReadLine()!.Split(" - ");

                var first = link[0];
                var second = link[1];

                if (!planetsGraph.ContainsKey(first))
                {
                    planetsGraph.Add(first, new List<string>());
                }
                planetsGraph[first].Add(second);

                if (!planetsGraph.ContainsKey(second))
                {
                    planetsGraph.Add(second, new List<string>());
                }
                planetsGraph[second].Add(first);
            }

            int universesCount = 0;

            foreach (var planet in planetsGraph.Keys)
            {
                if (visited.Contains(planet))
                {
                    continue;
                }

                TraverseUniverse(planet);

                universesCount++;
            }

            Console.WriteLine(universesCount);
        }

        private static void TraverseUniverse(string planet)
        {
            if (visited.Contains(planet))
            {
                return;
            }

            visited.Add(planet);

            foreach (var linkedPlanet in planetsGraph[planet])
            {
                TraverseUniverse(linkedPlanet);
            }
        }
    }
}
