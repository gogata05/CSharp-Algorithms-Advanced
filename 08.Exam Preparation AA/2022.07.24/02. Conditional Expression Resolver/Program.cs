namespace _02._Conditional_Expression_Resolver
{
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            var input = Console.ReadLine()!.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var result = SolveExpression(input, 0);

            Console.WriteLine(result);
        }

        private static string SolveExpression(string[] input, int idx)
        {
            if (int.TryParse(input[idx], out int result))
            {
                return input[idx];
            }

            if (input[idx] == "t")
            {
                return SolveExpression(input, idx + 2);
            }

            int condSymbols = 0;
            for (int i = idx + 3; i < input.Length; i++)
            {
                if (input[i] == "?")
                {
                    condSymbols++;
                }
                else if (input[i] == ":")
                {
                    condSymbols--;
                    if (condSymbols < 0)
                    {
                        return SolveExpression(input, i + 1);
                    }
                }
            }

            throw new InvalidOperationException() ;
        }
    }
}
