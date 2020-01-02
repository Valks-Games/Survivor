using System.Collections.Generic;

namespace WorldAPI.Tasks
{
    public interface ITaskRunner<TType> where TType : ITaskRunner<TType>
    {
        List<Task<TType>> TaskQueue { get; }
        Task<TType> CurrentTask { get; }

        void QueueTask(params Task<TType>[] task);

        void ClearTasks();

        void AssignTask(params Task<TType>[] task);

        void TaskLoop();
    }
}
