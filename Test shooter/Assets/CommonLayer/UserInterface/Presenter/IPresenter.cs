using System;

namespace CommonLayer.UserInterface.Presenter
{
    public interface IPresenter : IDisposable
    {
        string ViewKey { get; }
    }
}