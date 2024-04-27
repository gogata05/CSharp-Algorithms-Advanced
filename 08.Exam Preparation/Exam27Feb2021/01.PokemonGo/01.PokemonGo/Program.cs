using System;
using System.Collections.Generic;

namespace _01.PokemonGo
{
    public class Street
    {
        public string Name { get; set; }

        public int CountOfPokemons { get; set; }

        public int RequiredFuel { get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var maxFuelCapacity = int.Parse(Console.ReadLine());

            var streets = new List<Street>();

            while (true)
            {
                var line = Console.ReadLine();

                if (line == "End")
                {
                    break;
                }

                var streetParts = line.Split(", ");

                streets.Add(new Street
                {
                    Name = streetParts[0],
                    CountOfPokemons = int.Parse(streetParts[1]),
                    RequiredFuel = int.Parse(streetParts[2])
                }); 
            }

            var dp = new int[streets.Count + 1, maxFuelCapacity + 1];
            var used = new bool[streets.Count + 1, maxFuelCapacity + 1];

            for (int row = 1; row < dp.GetLength(0); row++)
            {
                var streetIdx = row - 1;
                var street = streets[streetIdx];

                for (int capacity = 1; capacity < dp.GetLength(1); capacity++)
                {
                    var excluding = dp[row - 1, capacity];

                    if (street.RequiredFuel > capacity)
                    {
                        dp[row, capacity] = excluding;
                        continue;
                    }

                    var including = street.CountOfPokemons + dp[row - 1, capacity - street.RequiredFuel];

                    if (including > excluding)
                    {
                        dp[row, capacity] = including;
                        used[row, capacity] = true;
                    }
                    else
                    {
                        dp[row, capacity] = excluding;
                    }
                }
            }

            var currentCapacity = maxFuelCapacity;

            var totalWeight = 0;

            var streetsPassed = new SortedSet<string>();

            for (int row = dp.GetLength(0) - 1; row > 0; row--)
            {
                if (!used[row, currentCapacity])
                {
                    continue;
                }

                var street = streets[row - 1];
                totalWeight += street.RequiredFuel;
                currentCapacity -= street.RequiredFuel;
                streetsPassed.Add(street.Name);

                if (currentCapacity == 0)
                {
                    break;
                }
            }

            if (streetsPassed.Count == 0)
            {
                Console.WriteLine($"Total Pokemon caught -> 0");
                Console.WriteLine($"Fuel Left -> {maxFuelCapacity}");
                return;
            }

            Console.WriteLine(String.Join(" -> ", streetsPassed));
            Console.WriteLine($"Total Pokemon caught -> {dp[streets.Count, maxFuelCapacity]}");
            Console.WriteLine($"Fuel Left -> {maxFuelCapacity - totalWeight}");
        }
    }
}
