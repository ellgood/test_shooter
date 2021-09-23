using System;

namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    public interface IReactiveRoute
    {
        IObservable<RouteState> WhenStateChanged();

        IObservable<RouteState> WhenStateEntered(RouteState state);

        IObservable<RouteState> WhenStateExited(RouteState state);
    }
}