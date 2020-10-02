using System;

namespace ThreadPool.Pool
{
    public delegate void TaskDelegate();
    public interface ITaskQueue
    {
        void EnqueueTask(TaskDelegate task);
    }
}