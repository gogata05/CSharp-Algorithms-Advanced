namespace _03._The_Story_Telling
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        private static Dictionary<string, List<string>> storyGraph = new Dictionary<string, List<string>>();
        private static HashSet<string> visited = new HashSet<string>();
        private static HashSet<string> cycles = new HashSet<string>();

        static void Main(string[] args)
        {
            var line = Console.ReadLine()!.Split(" ->", StringSplitOptions.RemoveEmptyEntries);

            while (line[0] != "End")
            {
                string node = line[0].Trim();

                if (line.Length > 1)
                {
                    var children = line[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                    storyGraph.Add(node, children);
                }
                else
                {
                    storyGraph.Add(node, new List<string>());
                }

                line = Console.ReadLine()!.Split(" ->", StringSplitOptions.RemoveEmptyEntries);
            }

            Stack<string> sortedStory = new Stack<string>();

            try
            {
                foreach (var chapter in storyGraph.Keys)
                {
                    SortStory(chapter, sortedStory);
                }

                Console.WriteLine(string.Join(" ", sortedStory));
            }
            catch (InvalidOperationException ioe)
            {
                Console.WriteLine(ioe.Message);
            }
        }

        private static void SortStory(string node, Stack<string> sortedStory)
        {
            if (cycles.Contains(node))
            {
                throw new InvalidOperationException("Invalid sorting");
            }

            if (visited.Contains(node))
            {
                return;
            }

            visited.Add(node);
            cycles.Add(node);

            foreach (var childChapter in storyGraph[node])
            {
                SortStory(childChapter, sortedStory);
            }

            sortedStory.Push(node);
            cycles.Remove(node);
        }
    }
}
