using WorldAPI.Tasks;

public abstract class GameTask<T> : Task<T> where T : GameEntity<T>
{
    public readonly string Name;

    public GameTask(string name)
    {
        Name = name;
    }
}