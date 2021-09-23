namespace CommonLayer.UserInterface.Pooling.Distribution
{
    public interface IPoolDisposeInstanceStrategy<in T> : IPoolStrategies
    {
        void PoolDisposeInstance(T instance);
    }
}