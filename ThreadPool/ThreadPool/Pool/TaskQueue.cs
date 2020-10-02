using System;
using System.Threading;

namespace ThreadPool.Pool
{
    public class TaskQueue : ITaskQueue
    {
        private const int defaultThreadCount = 15;
        private int _threadCount;
        private Thread[] _threadQueue;

        public TaskQueue()
        {
            _threadCount = defaultThreadCount;
            _threadQueue = new Thread[_threadCount];
        }

        public TaskQueue(int threadCount)
        {
            _threadCount = threadCount;
            _threadQueue = new Thread[threadCount];
        }
        private static void ThreadProc()
        {
            Console.WriteLine("\nCurrent thread: {0}", Thread.CurrentThread.Name);

            Thread.Sleep(4000);
            Console.WriteLine("\nCurrent thread: {0}", Thread.CurrentThread.Name);
        }
        
        public void EnqueueTask(TaskDelegate task)
        {
             _threadQueue[0] = new Thread(new ThreadStart(ThreadProc));
             _threadQueue[0].Start();
             _threadQueue[0].Join();
            for (int i = 1; i < _threadCount; i++)
            {
                _threadQueue[i] = new Thread(new ThreadStart(ThreadProc));
                _threadQueue[i].Name = i.ToString();
                _threadQueue[i].Start();
            }
        }
    }
}