using System;
using System.Collections.Generic;
using CommonLayer.UserInterface.Objects;
using CommonLayer.UserInterface.Pooling.AutoTrimming;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommonLayer.UserInterface.Pooling
{
    public abstract class FacadeObjectPoolBase<T, TFacade> : DisposableObject, IObjectPool<TFacade>
        where T : TFacade
        where TFacade : class
    {
        private readonly Stack<T> _poolStack = new Stack<T>();

#if DEBUG || POOL_SAFERENTED_BYHASH
        private readonly HashSet<T> _rentedObjects = new HashSet<T>();
#else
        private int _rentedCount = 0;
#endif

        private AutoTrimOption _autoTrim;

        private IPoolTrimmer _trimmer;

        public AutoTrimOption AutoTrimOption
        {
            get => _autoTrim;
            set
            {
                if (value.Enabled && !_autoTrim.Enabled)
                {
                    _trimmer = new PoolTrimmer(this);
                }

                if (value.Enabled)
                {
                    _trimmer.TrimRatio = value.TrimRatio;
                    _trimmer.MinSize = value.MinSize;
                    _trimmer.TrimDelay = value.TrimDelay;
                    _trimmer.CallOnBeforeRent = value.CallOnBeforeRent;
                }
                else if (_autoTrim.Enabled)
                {
                    _trimmer.Dispose();
                    _trimmer = null;
                }

                _autoTrim = value;
            }
        }

        public virtual bool IsReady { get; } = true;

        /// <summary>
        ///     Current pooled object count.
        /// </summary>
        public int Count => _poolStack.Count;

        public int RentedCount
        {
            get
            {
#if DEBUG || POOL_SAFERENTED_BYHASH
                return _rentedObjects.Count;
#else
                return _rentedCount;
#endif
            }
        }

        public int TotalCount => Count + RentedCount;

        /// <summary>
        ///     Limit of instance count.
        /// </summary>
        protected int MaxPoolCount => int.MaxValue;

        #region IObjectPool Implementation

        /// <summary>
        ///     Clear pool.
        /// </summary>
        public void Clear(bool callOnBeforeRent = false)
        {
            while (_poolStack.Count != 0)
            {
                T instance = _poolStack.Pop();
                if (callOnBeforeRent)
                {
                    OnBeforeRent(instance);
                }

                OnClear(instance);
            }
        }

        public bool CanShrink(float instanceCountRatio, int minSize)
        {
            var size = (int) (_poolStack.Count * Mathf.Clamp01(instanceCountRatio));
            size = Mathf.Max(minSize, size);

            return _poolStack.Count > size;
        }

        /// <summary>
        ///     Trim pool instances.
        /// </summary>
        /// <param name="instanceCountRatio">0.0f = clear all ~ 1.0f = live all.</param>
        /// <param name="minSize">Min pool count.</param>
        /// <param name="callOnBeforeRent">If true, call OnBeforeRent before OnClear.</param>
        public void Shrink(float instanceCountRatio, int minSize, bool callOnBeforeRent = false)
        {
            var size = (int) (_poolStack.Count * Mathf.Clamp01(instanceCountRatio));
            size = Mathf.Max(minSize, size);

            while (_poolStack.Count > size)
            {
                T instance = _poolStack.Pop();
                if (callOnBeforeRent)
                {
                    OnBeforeRent(instance);
                }

                OnClear(instance);
            }
        }

        #endregion

        #region IObjectPool<TFacade> Implementation

        /// <summary>
        ///     Get instance from pool.
        /// </summary>
        public TFacade Rent()
        {
            ThrowIfDisposedDebugOnly();

            re_rent:

            T instance;

            if (_poolStack.Count > 0)
            {
                instance = _poolStack.Pop();
            }
            else
            {
                instance = CreateInstance();
                PostCreate(instance);
            }

            if (!IsValid(instance))
            {
                Debug.LogError("Pool contains invalid object");
                goto re_rent;
            }

#if DEBUG || POOL_SAFERENTED_BYHASH
            if (!_rentedObjects.Add(instance))
            {
                throw new ArgumentException("Rent already rented instance");
            }
#else
            _rentedCount++;
#endif
            OnBeforeRent(instance);
            return instance;
        }

        /// <summary>
        ///     Return instance to pool.
        /// </summary>
        public void Return(TFacade item)
        {
            Assert.IsNotNull(item);

            ThrowIfDisposedDebugOnly();

            var instance = (T) item;

            if (!IsValid(instance))
            {
#if DEBUG
                throw new ArgumentException($"Returned object \"{typeof(TFacade).Name}\" to pool is invalid", nameof(item));
#else
                Debug.Log($"Returned object to pool is invalid, {typeof(TFacade).Name}");
                return;
#endif
            }

#if DEBUG || POOL_SAFERENTED_BYHASH
            if (!_rentedObjects.Remove(instance))
            {
                throw new ArgumentException("Returned object is unknown", nameof(item));
            }
#else
            _rentedCount++;
#endif

            if (_poolStack.Count + 1 == MaxPoolCount)
            {
                // Reached Max PoolSize
                OnAfterReturn(instance);
                OnClear(instance);
            }
            else
            {
                _poolStack.Push(instance);
                OnAfterReturn(instance);
            }

            _trimmer?.OnReturn();
        }

        #endregion

        protected virtual void PostCreate(T instance) { }

        protected virtual bool IsValid(TFacade target)
        {
            return true;
        }

        /// <summary>
        ///     CreateLocal instance when needed.
        /// </summary>
        protected abstract T CreateInstance();

        /// <summary>
        ///     Called before return to pool, useful for set active object(it is default behavior).
        /// </summary>
        protected abstract void OnBeforeRent(T instance);

        /// <summary>
        ///     Called before return to pool, useful for set inactive object(it is default behavior).
        /// </summary>
        protected abstract void OnAfterReturn(T instance);

        /// <summary>
        ///     Called when clear or disposed, useful for destroy instance or other finalize method.
        /// </summary>
        protected abstract void OnClear(T instance);

        #region IDisposable Support

        protected override void OnDispose()
        {
            Clear();
        }

        #endregion
    }
}