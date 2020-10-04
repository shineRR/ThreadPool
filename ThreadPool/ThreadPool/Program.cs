using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ThreadPool.Pool;

namespace ThreadPool
{
    internal class Program
    {
        static TaskQueue taskQueue = new TaskQueue(3);

        public static void Main(string[] args)
        {
            taskQueue.Dispose();
        }
    }
}