using System;

namespace CommonLayer.UserInterface.Objects
{
    public interface IDisposableStatus
    {
        bool IsDisposed { get; }
        bool IsDisposing { get; }
        bool IsActive { get; }
    }
    
    public interface IDisposableObject
        : IDisposable, IDisposableStatus
    { }
}