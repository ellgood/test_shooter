
using System;

namespace CommonLayer.UserInterface.Objects
{
    /// <summary>
    ///     The disposable action.
    /// </summary>
    public sealed class DisposableAction : IDisposable
    {
        private Action _dispose;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DisposableAction" /> class.
        /// </summary>
        /// <param name="dispose">
        ///     The dispose.
        /// </param>
        public DisposableAction(Action dispose)
        {
            _dispose = dispose ?? throw new ArgumentNullException(nameof(dispose));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DisposableAction" /> class.
        /// </summary>
        /// <param name="construct">
        ///     The construct.
        /// </param>
        /// <param name="dispose">
        ///     The dispose.
        /// </param>
        public DisposableAction(Action construct, Action dispose)
        {
            if (construct == null)
            {
                throw new ArgumentNullException(nameof(construct));
            }

            if (dispose == null)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            construct();

            _dispose = dispose;
        }

        #region IDisposable Implementation

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            if (_dispose == null)
            {
                return;
            }

            try
            {
                _dispose();
            }
            finally
            {
                _dispose = null;
            }
        }

        #endregion
    }
}