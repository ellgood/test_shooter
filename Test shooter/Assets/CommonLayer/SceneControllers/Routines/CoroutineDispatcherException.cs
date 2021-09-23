using System;

namespace CommonLayer.SceneControllers.Routines
{
    public sealed class CoroutineDispatcherException : Exception
    {
        public CoroutineDispatcherException(string msg, Exception inner) : base(msg, inner) { }
    }
}