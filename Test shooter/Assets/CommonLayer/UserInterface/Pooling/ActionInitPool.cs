using System;

namespace CommonLayer.UserInterface.Pooling
{
    public sealed class ActionInitPool<TArg, T> : PoolInitializer<TArg, T> where T : class
    {
        private readonly Action<T, TArg> _initializer;

        public ActionInitPool(ObjectPoolBase<T> objectPool, Action<T, TArg> initializer) : base(objectPool)
        {
            _initializer = initializer;
        }

        protected override void OnBeforeRent(T obj, TArg arg)
        {
            _initializer.Invoke(obj, arg);
        }
    }
}