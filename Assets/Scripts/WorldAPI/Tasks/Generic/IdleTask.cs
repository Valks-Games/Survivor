using System.Collections;
using UnityEngine;

namespace WorldAPI.Tasks.Generic
{
    public class IdleTask<T> : Task<T> where T : ITaskRunner<T>
    {
        private readonly float tick;

        public IdleTask(float tick = 0.1f)
        {
            tick = tick;
        }

        protected override IEnumerator Run()
        {
            yield return new WaitForSeconds(tick);

            if (Target.TaskQueue.Count < 1)
                Target.QueueTask(new IdleTask<T>(tick));
        }
    }
}
