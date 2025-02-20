using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());
            List<Box> boxes = new List<Box>();

            for (int i = 0; i < n; i++)
            {
                int[] dimensions = Console.ReadLine().Split().Select(int.Parse).ToArray();
                boxes.Add(new Box(dimensions[0], dimensions[1], dimensions[2]));
            }

            List<Box> maxStack = GetMaxStack(boxes);
            foreach (var box in maxStack)
            {
                Console.WriteLine($"{box.Width} {box.Depth} {box.Height}");
            }
        }

        static List<Box> GetMaxStack(List<Box> boxes)
        {
            boxes = boxes.OrderBy(b => b.Width).ThenBy(b => b.Depth).ThenBy(b => b.Height).ToList();
            int[] dp = new int[boxes.Count];
            int[] prev = new int[boxes.Count];

            for (int i = 0; i < boxes.Count; i++)
            {
                dp[i] = boxes[i].Height;
                prev[i] = -1;
            }

            int maxHeight = 0;
            int maxIndex = 0;

            for (int i = 1; i < boxes.Count; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (boxes[i].Width > boxes[j].Width && boxes[i].Depth > boxes[j].Depth && boxes[i].Height > boxes[j].Height)
                    {
                        if (dp[i] < dp[j] + boxes[i].Height)
                        {
                            dp[i] = dp[j] + boxes[i].Height;
                            prev[i] = j;
                        }
                    }
                }

                if (dp[i] > maxHeight)
                {
                    maxHeight = dp[i];
                    maxIndex = i;
                }
            }

            List<Box> result = new List<Box>();
            while (maxIndex != -1)
            {
                result.Add(boxes[maxIndex]);
                maxIndex = prev[maxIndex];
            }

            result.Reverse();
            return result;
        }

        class Box
        {
            public int Width { get; set; }
            public int Depth { get; set; }
            public int Height { get; set; }

            public Box(int width, int depth, int height)
            {
                Width = width;
                Depth = depth;
                Height = height;
            }
        }
    }
}
