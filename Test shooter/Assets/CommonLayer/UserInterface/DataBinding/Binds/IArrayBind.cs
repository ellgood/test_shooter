using System;
using UniRx;

namespace CommonLayer.UserInterface.DataBinding.Binds
{
    public interface IArrayBind<T> : IReadOnlyReactiveCollection<T>, IDisposable { }
}