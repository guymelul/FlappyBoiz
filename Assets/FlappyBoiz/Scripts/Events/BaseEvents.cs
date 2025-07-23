namespace FlappyBoiz.Scripts.Events
{
    public abstract class BaseEmptyEvent<T>
        where T : BaseEmptyEvent<T>, new()
    {
        public static readonly T Empty = new T();
    }
}