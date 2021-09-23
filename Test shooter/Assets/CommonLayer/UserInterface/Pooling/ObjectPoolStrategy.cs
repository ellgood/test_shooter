using System;

namespace CommonLayer.UserInterface.Pooling
{
    public sealed class ObjectPoolStrategy<T> : ObjectPoolBase<T>
        where T : class
    {
        private readonly Func<T> _construct;
        private readonly Action<T> _beforeReturn;
        private readonly Action<T> _beforeRent;
        private readonly Action<T> _cleanup;

        public ObjectPoolStrategy(Func<T> construct, Action<T> beforeReturn = null, Action<T> beforeRent = null,
            Action<T> cleanup = null)
        {
            _construct = construct;

            _beforeReturn = beforeReturn;
            _beforeRent = beforeRent;
            _cleanup = cleanup;
        }

        protected override T CreateInstance()
        {
            return _construct();
        }

        protected override void OnBeforeRent(T instance)
        {
            _beforeRent?.Invoke(instance);
        }

        protected override void OnAfterReturn(T instance)
        {
            _beforeReturn?.Invoke(instance);
        }

        protected override void OnClear(T instance)
        {
            _cleanup?.Invoke(instance);
        }
    }
}