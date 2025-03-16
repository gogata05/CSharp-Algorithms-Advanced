namespace _03._Path_Finder
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        static void Main(string[] args)
        {
            int nodeCount = int.Parse(Console.ReadLine()!);

            var graph = new List<int>[nodeCount];

            for (int i = 0; i < nodeCount; i++)
            {
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    graph[i] = new List<int>();
                }
                else
                {
                    var children = input.Split().Select(int.Parse).ToList();
                    graph[i] = children;
                }
            }

            int pathsToCheckCount = int.Parse(Console.ReadLine()!);

            for (int i = 0; i < pathsToCheckCount; i++)
            {
                var pathToCheck = Console.ReadLine()!.Split().Select(int.Parse).ToArray();
                bool isPathCorrect = true;

                for (int idx = 1; idx < pathToCheck.Length; idx++)
                {
                    var parent = pathToCheck[idx - 1];
                    var node = pathToCheck[idx];

                    if (!graph[parent].Contains(node))
                    {
                        isPathCorrect = false;
                        break;
                    }
                }

                string result = isPathCorrect ? "yes" : "no";
                Console.WriteLine(result);
            }
        }
    }
}
