using CommonLayer.UserInterface.Objects;
using UnityEngine;

namespace CommonLayer.UserInterface.Pooling.AutoTrimming
{
    public sealed class PoolTrimmer : DisposableObject, IPoolTrimmer
    {
        private readonly IObjectPool _objectPool;

        private float _delayPoint = float.MinValue;
        private float _trimDelay;
        private float _trimRatio = 1;
        private int _minSize;

        public PoolTrimmer(IObjectPool objectPool)
        {
            _objectPool = objectPool;
            PoolTrimManager.Instance.Add(this);
        }

        private float CurrentTime => Time.unscaledTime;

        #region ICleanUpEvent Implementation

        protected override void OnDispose()
        {
            PoolTrimManager.Instance.Remove(this);
        }

        #endregion

        #region IPoolTrimmer Implementation

        public float TrimDelay
        {
            get => _trimDelay;
            set
            {
                ThrowIfDisposedDebugOnly();
                _trimDelay = Mathf.Clamp(value, 0, float.MaxValue);
            }
        }

        public float TrimRatio
        {
            get => _trimRatio;
            set
            {
                ThrowIfDisposedDebugOnly();
                _trimRatio = Mathf.Clamp01(value);
            }
        }

        public int MinSize
        {
            get => _minSize;
            set
            {
                ThrowIfDisposedDebugOnly();
                _minSize = Mathf.Clamp(value, 0, int.MaxValue);
            }
        }

        public bool CallOnBeforeRent { get; set; } = true;

        public void OnReturn()
        {
            ThrowIfDisposedDebugOnly();
            _delayPoint = CurrentTime + _trimDelay;
        }

        public void Check()
        {
            ThrowIfDisposedDebugOnly();
            if (_delayPoint > CurrentTime)
            {
                return;
            }

            _objectPool.Shrink(_trimRatio, _minSize, CallOnBeforeRent);

            if (_objectPool.CanShrink(_trimRatio, _minSize))
            {
                _delayPoint = CurrentTime + _trimDelay;
            }
            else
            {
                _delayPoint = float.MaxValue;
            }
        }

        #endregion
    }
}