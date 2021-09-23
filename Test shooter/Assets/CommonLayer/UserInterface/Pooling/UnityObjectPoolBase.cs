using CommonLayer.UserInterface.Objects;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommonLayer.UserInterface.Pooling
{
    public abstract class UnityObjectPoolBase<T> : ObjectPoolBase<T>
        where T : Object
    {
        private readonly T _original;

        protected UnityObjectPoolBase() { }

        protected UnityObjectPoolBase(T original)
        {
            Assert.IsNotNull(original);
            _original = original;
        }

        public override bool IsReady => _original != null;

        protected override bool IsValid(T target)
        {
            return target;
        }

        protected override T CreateInstance()
        {
            return Object.Instantiate(_original);
        }

        protected override void OnClear(T instance)
        {
            if (!instance)
            {
                return;
            }

            switch (instance)
            {
                case GameObject go:
                {
                    go.SafeDestroy();
                    
                    break;
                }
                case Component component:
                {
                    component.gameObject.SafeDestroy();
                    
                    break;
                }
                default:
                {
                    instance.SafeDestroy();
                    
                    break;
                }
            }
        }
    }
}