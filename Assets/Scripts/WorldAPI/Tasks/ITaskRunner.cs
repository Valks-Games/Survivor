using System.Collections.Generic;

namespace WorldAPI.Tasks
{
    public interface ITaskRunner<TType> where TType : ITaskRunner<TType>
    {
        public List<Task<TType>> TaskQueue { get; }
        public Task<TType> CurrentTask { get; }

        public void QueueTask(params Task<TType>[] task);

        public void ClearTasks();

        public void AssignTask(params Task<TType>[] task);

        public void TaskLoop();
    }
}
