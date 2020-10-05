using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ThreadPool.Pool
{
    public class TaskQueue : ITaskQueue
    {
        private const int DefaultThreadCount = 15;
        private Queue<TaskDelegate> _taskQueue = new Queue<TaskDelegate>();
        public Thread[] ThreadQueue;
        private readonly Object _queueSync = new object();
        private bool _isActive = true;

        public TaskQueue()
        {
            ThreadQueue = new Thread[DefaultThreadCount];
        }

        public TaskQueue(int threadCount)
        {
            ThreadQueue = new Thread[threadCount];
            for (int i = 0; i < ThreadQueue.Length; i++)
            {
                ThreadQueue[i] = new Thread(new ThreadStart(taskSelectionQueue));
                ThreadQueue[i].Name = "Thread #" + i;
                ThreadQueue[i].Start();
            }
        }

        private void taskSelectionQueue()
        {
            while (_isActive)
            {
                TaskDelegate taskDelegate = null;
                bool taskReady = false;
                
                lock (_queueSync)
                {
                    if (_taskQueue.Count == 0)
                    {
                        Monitor.Wait(_queueSync);
                    }
                    else
                    {
                        taskReady = true;
                        taskDelegate = _taskQueue.Dequeue();
                        Monitor.Pulse(_queueSync);
                    }
                }
                if (taskReady)
                    taskDelegate();
            }
        }
        
        public void EnqueueTask(TaskDelegate task)
        {
            lock (_queueSync)
            {
                _taskQueue.Enqueue(task);
                Monitor.Pulse(_queueSync);
            }
        }

        public void Dispose()
        {
            while (true)
            {
                Thread.Sleep(200);
                lock (_queueSync)
                {
                    if (_taskQueue.Count == 0) break;
                }
            }
            
            lock (_queueSync)
            {
                _isActive = false;
                Monitor.PulseAll(_queueSync);
            }
        }
    }
}