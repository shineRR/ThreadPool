using System;
using ThreadPool.Pool;

namespace ThreadPool
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TaskQueue taskQueue = new TaskQueue(5);
            
            taskQueue.EnqueueTask(delegate
            {
                for (int i = 0; i < 1000000; i++)
                {
                    Console.WriteLine(i);
                }
            });
        }
    }
}