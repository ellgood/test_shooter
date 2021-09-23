namespace CommonLayer.UserInterface.Pooling
{
    public abstract class ObjectPoolBase<T> : FacadeObjectPoolBase<T, T>
        where T : class
    {
    }
}