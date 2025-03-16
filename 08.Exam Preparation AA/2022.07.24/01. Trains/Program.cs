namespace _01._Trains
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        static void Main(string[] args)
        {
            var arrivals = Console.ReadLine()!
                .Split()
                .Select(float.Parse)
                .OrderBy(x => x)
                .ToArray();
            var departures = Console.ReadLine()!
                .Split()
                .Select(float.Parse)
                .OrderBy(x => x)
                .ToArray();

            int platformsNeeded = CalcNeededPlatforms(arrivals, departures);

            Console.WriteLine(platformsNeeded);
        }

        private static int CalcNeededPlatforms(float[] arrivals, float[] departures)
        {
            int arrIdx = 0;
            int depIdx = 0;
            int totalPlatforms = 0;
            int takenPlatforms = 0;

            while (arrIdx < arrivals.Length)
            {
                var arrTime = arrivals[arrIdx];
                var depTime = departures[depIdx];

                if (arrTime < depTime)
                {
                    takenPlatforms++;
                    arrIdx++;
                    totalPlatforms = Math.Max(totalPlatforms, takenPlatforms);
                }
                else
                {
                    depIdx++;
                    takenPlatforms--;
                }
            }

            return totalPlatforms;
        }
    }
}
