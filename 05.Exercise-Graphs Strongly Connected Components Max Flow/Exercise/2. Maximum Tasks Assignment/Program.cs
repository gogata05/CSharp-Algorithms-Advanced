using System;
using System.Collections.Generic;

namespace _2._Maximum_Tasks_Assignment
{
    public class Program
    {
        private static int[,] graph;
        private static int[] parents;
        static void Main(string[] args)
        {
            int people = int.Parse(Console.ReadLine());
            int tasks = int.Parse(Console.ReadLine());
            
            graph = ReadGraph(people, tasks);
            int nodes = graph.GetLength(0);
            parents = new int[graph.GetLength(0)];
            Array.Fill(parents, -1);
            int start = 0;
            int target = graph.GetLength(0) - 1;
            while (BFS(start,target))
            {
                int node = target;
                while (node!=start)
                {
                    int parent = parents[node];
                    graph[parent, node] = 0;
                    graph[node, parent] = 1;
                    node = parent;
                }
            }
            for (int person = 1; person <= people; person++)
            {
                for (int task = people+1; task <= people+tasks; task++)
                {
                    if (graph[task,person]>0)
                    {
                        Console.WriteLine("{0}-{1}",(char)(64+person),task-people);
                    }
                }
            }
        }

        private static int[,] ReadGraph(int people, int tasks)
        {
            int nodes = people + tasks + 2;
            var result=new int[nodes, nodes];
            int start = 0;
            int target = nodes - 1;
            for (int person = 1; person <= people; person++)
            {
                result[start, person] = 1;
            }
            for (int task = people + 1; task <= people + tasks; task++)
            {
                result[task, target] = 1;
            }
            for (int person = 1; person <= people; person++)
            {
                string personTasks = Console.ReadLine();
                for (int task = 0; task < personTasks.Length; task++)
                {
                    if (personTasks[task] == 'Y')
                    {
                        result[person, people + 1 + task] = 1;
                    }
                }
            }
            return result;
        }

        private static bool BFS(int start, int target)
        {
            var visited = new bool[graph.GetLength(0)];
            var queue = new Queue<int>();
            visited[start] = true;
            queue.Enqueue(start);
            while (queue.Count>0)
            {
                int node = queue.Dequeue();
                if (node==target)
                {
                    return true;
                }
                for (int child = 0; child < graph.GetLength(1); child++)
                {
                    if (!visited[child]&&
                        graph[node,child]>0)
                    {
                        parents[child] = node;
                        visited[child] = true;
                        queue.Enqueue(child);
                    }
                }
            }
            return false;
        }
    }
}
