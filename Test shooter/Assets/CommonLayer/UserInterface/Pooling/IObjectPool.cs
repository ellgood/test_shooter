using System;


namespace CommonLayer.UserInterface.Pooling
{
    public interface IObjectPool : IDisposable
    {
        bool IsDisposed { get; }

        /// <summary>
        ///     Clear pool.
        /// </summary>
        void Clear(bool callOnBeforeRent = false);

        /// <summary>
        ///     Trim pool instances.
        /// </summary>
        /// <param name="instanceCountRatio">0.0f = clear all ~ 1.0f = live all.</param>
        /// <param name="minSize">Min pool count.</param>
        /// <param name="callOnBeforeRent">If true, call OnBeforeRent before OnClear.</param>
        void Shrink(float instanceCountRatio, int minSize, bool callOnBeforeRent = false);

        bool CanShrink(float instanceCountRatio, int minSize);
    }
    public interface IObjectPool<TFacade> : IObjectPool
        where TFacade : class
    {
        /// <summary>
        ///     Get instance from pool.
        /// </summary>
        TFacade Rent();

        /// <summary>
        ///     Return instance to pool.
        /// </summary>
        void Return(TFacade item);
    }
}