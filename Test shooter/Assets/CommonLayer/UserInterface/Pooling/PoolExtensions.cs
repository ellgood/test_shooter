using System;

namespace CommonLayer.UserInterface.Pooling
{
    public static class PoolExtensions
    {
        public static ActionInitPool<TArg, T> WithArg<TArg, T>(this ObjectPoolBase<T> pool, Action<T, TArg> initializer)
            where T : class
        {
            return new ActionInitPool<TArg, T>(pool, initializer);
        }
    }
}