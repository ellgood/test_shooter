namespace CommonLayer.UserInterface.Pooling.Distribution
{
    public interface IPoolRendStrategy<in T> : IPoolStrategies
    {
        void PoolBeforeRent(T instance);
    }
}