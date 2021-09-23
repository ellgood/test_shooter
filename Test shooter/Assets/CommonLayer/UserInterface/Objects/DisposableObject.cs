using System;
using System.Runtime.CompilerServices;

namespace CommonLayer.UserInterface.Objects
{
    public class DisposableObject : IDisposableObject
    {
        #region IDisposable Implementation

        public void Dispose()
        {
            if (IsDisposed || IsDisposing)
            {
                return;
            }

            IsDisposing = true;

            try
            {
                OnDispose();
            }
            finally
            {
                GC.SuppressFinalize(this);

                IsDisposed = true;
                IsDisposing = false;
            }
        }

        #endregion

        #region IDisposableStatus Implementation

        public bool IsDisposed { get; private set; }

        public bool IsDisposing { get; private set; }

        public bool IsActive => !(IsDisposed || IsDisposing);

        #endregion

        protected virtual void OnDispose() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                ThrowDisposed();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void ThrowIfDisposedDebugOnly()
        {
            if (IsDisposed)
            {
#if DEBUG
                ThrowDisposed();
#else
                UnityEngine.Debug.LogException(new ObjectDisposedException($"{GetType().FullName}", ToString()));
#endif
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowDisposed()
        {
            throw new ObjectDisposedException($"{GetType().FullName}", ToString());
        }
    }
}