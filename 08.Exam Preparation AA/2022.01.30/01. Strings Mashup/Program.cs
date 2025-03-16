namespace _01._Strings_Mashup
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    internal class Program
    {
        private static List<string> variations;

        static void Main(string[] args)
        {
            var str = Console.ReadLine()!.ToCharArray().OrderBy(c => c).ToArray();
            int k = int.Parse(Console.ReadLine()!);

            variations = new List<string>();            

            FindVariations(0, string.Empty, str, k);

            Console.WriteLine(string.Join(Environment.NewLine, variations));
        }

        private static void FindVariations(int idx, string variation, char[] str, int varLength)
        {
            if (variation.Length >= varLength)
            {
                variations.Add(variation);
                return;
            }

            for (int i = idx; i < str.Length; i++)
            {
                var newVariation = variation + str[i];

                FindVariations(i, newVariation, str, varLength);
            }
        }
    }
}
