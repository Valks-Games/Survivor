using System.Collections.Generic;
using UnityEngine;

namespace WorldAPI.Tasks
{
    public partial class TaskedBehaviour<TType> : MonoBehaviour, ITaskRunner<TType> where TType : TaskedBehaviour<TType>
    {
        public List<Task<TType>> TaskQueue { get; } = new List<Task<TType>>();
        public Task<TType> CurrentTask { get; private set; } = null;

        public void TaskLoop()
        {
            if (CurrentTask == null || CurrentTask.IsComplete)
            {
                if (TaskQueue.Count < 1)
                    return;

                CurrentTask = TaskQueue[0];
                TaskQueue.RemoveAt(0);
                return;
            }

            if (!CurrentTask.IsRunning)
                StartCoroutine(CurrentTask.Start());
        }

        public void ClearTasks()
        {
            TaskQueue.Clear();
        }

        public void QueueTask(params Task<TType>[] tasks)
        {
            foreach (Task<TType> task in tasks)
                task.Register(this as TType);

            TaskQueue.AddRange(tasks);
        }

        public void AssignTask(params Task<TType>[] tasks)
        {
            ClearTasks();
            QueueTask(tasks);
        }
    }

    public partial class TaskedBehaviour<TType> : MonoBehaviour, ITaskRunner<TType> where TType : TaskedBehaviour<TType>
    {
        public static TaskedBehaviour<TType> New(string name = "TaskedBehaviour")
        {
            return Derive(name).AddComponent<TaskedBehaviour<TType>>();
        }

        protected static GameObject Derive(string name = "TaskedBehaviour")
        {
            return new GameObject(name);
        }
    }
}