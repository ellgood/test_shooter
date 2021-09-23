using System;

namespace CommonLayer.DataContext
{
    public interface IDataContext
    {
        event Action<StatusStep> ContextStatusChanged;
        bool IsInitialized { get; }
        bool IsLoaded { get; }
        bool IsInitializing { get; }

        void Load();

        void Reset();
    }
}