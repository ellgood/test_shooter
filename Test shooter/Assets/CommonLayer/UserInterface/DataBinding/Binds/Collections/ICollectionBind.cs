using System;

namespace CommonLayer.UserInterface.DataBinding.Binds.Collections
{
    public interface ICollectionBind : IDisposable
    {
        bool IsWriting { get; }
        bool IsReading { get; }
        bool IsActive { get; }
        int ActiveDepth { get; }
        bool IsLastInitiator { get; }
        bool IsLastInteractor { get; }
    }
}