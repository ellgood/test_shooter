namespace CommonLayer.UserInterface.Pooling.Distribution
{
    public interface IPoolInstanceStrategy<in T> : IPoolCreateInstanceStrategy<T>,
        IPoolDisposeInstanceStrategy<T>
    {
    }
}