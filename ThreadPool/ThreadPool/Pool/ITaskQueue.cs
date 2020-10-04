using System;

namespace ThreadPool.Pool
{
    public delegate void TaskDelegate();
    public interface ITaskQueue : IDisposable
    {
        void EnqueueTask(TaskDelegate task);
    }
}