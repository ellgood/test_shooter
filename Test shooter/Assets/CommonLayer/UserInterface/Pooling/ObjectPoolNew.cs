namespace CommonLayer.UserInterface.Pooling
{
    public sealed class ObjectPoolNew<T> : ObjectPoolBase<T>
        where T : class, new()
    {
        protected override T CreateInstance()
        {
            return new T();
        }

        protected override void OnBeforeRent(T instance)
        {
        }

        protected override void OnAfterReturn(T instance)
        {
        }

        protected override void OnClear(T instance)
        {
        }
    }
}