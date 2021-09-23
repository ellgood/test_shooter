using UnityEngine;

namespace CommonLayer.UserInterface.Pooling.Distribution
{
    public class UnityComponentDPool<T, TStrategies> : UnityComponentPool<T>
        where T : Component
        where TStrategies : IPoolStrategies
    {
        private readonly TStrategies _distributor;

        public UnityComponentDPool(string poolTag, T original, TStrategies distributor, Transform rentRoot = null,
            Transform poolRoot = null) : base(poolTag,original, rentRoot, poolRoot)
        {
            _distributor = distributor;
        }

        public UnityComponentDPool(T original, TStrategies distributor, Transform rentRoot = null,
            Transform poolRoot = null) : base(original, rentRoot, poolRoot)
        {
            _distributor = distributor;
        }

        protected override T CreateInstance()
        {
            T instance = base.CreateInstance();

            if (_distributor is IPoolCreateInstanceStrategy<T> createInstanceStrategy)
            {
                createInstanceStrategy.PoolCreateInstance(instance);
            }

            return instance;
        }

        protected override void OnClear(T instance)
        {
            try
            {
                if (_distributor is IPoolDisposeInstanceStrategy<T> distributor)
                {
                    distributor.PoolDisposeInstance(instance);
                }
            }
            finally
            {
                base.OnClear(instance);
            }
        }

        protected override void OnBeforeRent(T instance)
        {
            try
            {
                if (_distributor is IPoolRendStrategy<T> distributor)
                {
                    distributor.PoolBeforeRent(instance);
                }
            }
            finally
            {
                base.OnBeforeRent(instance);
            }
        }

        protected override void OnAfterReturn(T instance)
        {
            try
            {
                if (_distributor is IPoolReturnStrategy<T> distributor)
                {
                    distributor.PoolBeforeReturn(instance);
                }
            }
            finally
            {
                base.OnAfterReturn(instance);
            }
        }
    }
}