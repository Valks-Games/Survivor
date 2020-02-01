public abstract class GameTask<T> : GameAPI.Tasks.GameTask<T> where T : GameEntity<T>
{
    public readonly string Name;

    public GameTask(string name) =>
        Name = name;
}