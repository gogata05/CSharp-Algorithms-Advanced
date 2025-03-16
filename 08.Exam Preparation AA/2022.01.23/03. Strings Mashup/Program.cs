namespace _03._Strings_Mashup
{
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            var chars = Console.ReadLine()!.ToUpper().ToCharArray();

            PrintMashups(chars, 0);
        }

        private static void PrintMashups(char[] chars, int idx)
        {
            if (idx >= chars.Length)
            {
                Console.WriteLine(string.Join("", chars));
                return;
            }

            PrintMashups(chars, idx + 1);

            var original = chars[idx];
            chars[idx] = char.Parse(chars[idx].ToString().ToLower());

            if(original != chars[idx])
            {
                PrintMashups(chars, idx + 1);
                chars[idx] = original;
            }
        }
    }
}
