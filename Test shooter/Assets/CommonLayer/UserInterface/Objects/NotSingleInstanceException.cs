using System;

namespace CommonLayer.UserInterface.Objects
{
    public sealed class NotSingleInstanceException<TSingletonType> : Exception
    {
        public NotSingleInstanceException() : base($"Instance of {typeof(TSingletonType).FullName} should be created once")
        {
            
        }
    }

    public sealed class NotSingleInstanceException : Exception
    {
        public NotSingleInstanceException(string message) : base(message)
        {
        }
    }
}