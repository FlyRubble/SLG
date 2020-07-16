
namespace SLG
{
    namespace Event
    {
        public delegate void Action();
        public delegate void Action<T>(T t);
        public delegate void Action<T, U>(T t, U u);
        public delegate void Action<T, U, V>(T t, U u, V v);
        public delegate void Action<T, U, V, W>(T t, U u, V v, W w);
    }
}
