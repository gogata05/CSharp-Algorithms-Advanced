using System;
using System.Collections.Generic;

namespace _3._Longest_String_Chain
{
    class Program
    {
        static void Main(string[] args)
        {
            var words = Console.ReadLine()
                .Split();
            var len = new int[words.Length];
            var parent = new int[words.Length];
            int longestStringChain = 0;
            int lastIdx = 0;
            for (int current = 0; current < words.Length; current++)
            {
                len[current] = 1;
                parent[current] = -1;
                string currentWord = words[current];
                for (int prev = current-1; prev >= 0; prev--)
                {
                    string prevWord = words[prev];
                    if (currentWord.Length>prevWord.Length&&
                        len[prev]+1>=len[current])
                    {
                        len[current] = len[prev] + 1;
                        parent[current] = prev;
                    }
                }
                if (len[current]>longestStringChain)
                {
                    lastIdx = current;
                    longestStringChain = len[current];
                }
            }
            var stringChain = new Stack<string>();
            while (lastIdx!=-1)
            {
                stringChain.Push(words[lastIdx]);
                lastIdx = parent[lastIdx];
            }
            Console.WriteLine(string.Join(" ",stringChain));
        }
    }
}
