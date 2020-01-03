using System.Collections;
using UnityEngine;

namespace WorldAPI.Tasks.Generic
{
    public class WaitTask<T> : Task<T> where T : ITaskRunner<T>
    {
        private readonly float delay;

        public WaitTask(float delay)
        {
            this.delay = delay;
        }

        protected override IEnumerator Run()
        {
            yield return new WaitForSeconds(delay);
        }
    }
}
