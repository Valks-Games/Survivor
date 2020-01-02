using System.Collections;
using UnityEngine;

namespace WorldAPI.Tasks.Generic
{
    public class WaitTask<T> : Task<T> where T : ITaskRunner<T>
    {
        private readonly float _delay;

        public WaitTask(float delay)
        {
            _delay = delay;
        }

        protected override IEnumerator Run()
        {
            yield return new WaitForSeconds(_delay);
        }
    }
}
