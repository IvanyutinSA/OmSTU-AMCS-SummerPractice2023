namespace Pool;
using SpaceBattleLib;


class Program
{
    static void Main(string[] args)
    {
        // Размер пула подхватывается через консольку
        // По умолчанию poolSize = 10
        int poolSize;
        try 
        {
            poolSize = int.Parse(args[0]);
        }
        catch
        {
            poolSize = 10;
        }
        
        Pool<Spaceship> spaceshipPool = new Pool<Spaceship>(poolSize);
        using (PoolGuard<Spaceship> guard = new PoolGuard<Spaceship>(spaceshipPool))
        {
            Spaceship spaceship = guard.Object;
        }
    }
}

public class Pool<T> where T : new()
{
    private readonly Queue<T> pool;

    public Pool(int size)
    {
        pool = new Queue<T>(size);
        for (int i = 0; i < size; i++)
        {
            pool.Enqueue(new T());
        }
    }

    public T GetObject()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            return new T();
        }
    }

    public void ReturnObject(T obj)
    {
        pool.Enqueue(obj);
    }
}

public class PoolGuard<T> : IDisposable where T : new()
{
    private readonly Pool<T> pool;
    private readonly T obj;

    public PoolGuard(Pool<T> pool)
    {
        this.pool = pool;
        obj = pool.GetObject();
    }

    public T Object => obj;

    public void Dispose()
    {
        pool.ReturnObject(obj);
    }
}
