using System.Collections;

namespace WorldAPI.Tasks
{
    public abstract class Task<TTarget>
    {
        public TTarget Target;

        public bool IsRunning = false;
        public bool IsComplete = false;

        protected abstract IEnumerator Run();

        public IEnumerator Start()
        {
            IsRunning = true;

            yield return Run();

            IsRunning = false;
            IsComplete = true;
        }

        public void Register(TTarget target)
        {
            Target = target;
        }

        public override string ToString()
        {
            return "A task.";
        }
    }
}