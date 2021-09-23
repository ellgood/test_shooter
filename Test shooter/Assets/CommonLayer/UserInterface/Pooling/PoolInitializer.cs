namespace CommonLayer.UserInterface.Pooling
{
    public abstract class PoolInitializer<TArg, T> where T : class
    {
        private readonly ObjectPoolBase<T> _objectPool;

        public PoolInitializer(ObjectPoolBase<T> objectPool)
        {
            _objectPool = objectPool;
        }

        public T Rent(TArg arg)
        {
            T rentedObject = _objectPool.Rent();
            OnBeforeRent(rentedObject, arg);

            return rentedObject;
        }

        public void Return(T value)
        {
            _objectPool.Return(value);
        }

        protected abstract void OnBeforeRent(T obj, TArg arg);
    }
}