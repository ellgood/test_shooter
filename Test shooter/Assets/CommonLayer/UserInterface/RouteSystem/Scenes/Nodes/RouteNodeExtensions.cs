using System;
using CommonLayer.UserInterface.Presenter;

namespace CommonLayer.UserInterface.RouteSystem.Scenes.Nodes
{
    public static class RouteNodeExtensions
    {
        public static IUiSceneController AddRoute<TPresenter>(this IUiSceneController uiScene, string routeKey, Action<INode> childRoutes)
            where TPresenter : PresenterBase
        {
            return CommonAddRouteTo<TPresenter, IUiSceneController>(uiScene, routeKey, childRoutes);
        }

        public static INode AddRoute<TPresenter>(this INode node, string routeKey, Action<INode> childRoutes)
            where TPresenter : PresenterBase
        {
            return CommonAddRouteTo<TPresenter, INode>(node, routeKey, childRoutes);
        }

        public static INode AddSubRoute<TPresenter>(this INode node, string routeKey, Action<INode> childRoutes)
            where TPresenter : PresenterBase
        {
            INode child = node.ToSub<TPresenter>(routeKey);
            childRoutes?.Invoke(child);

            return node;
        }

        private static TConfigurator CommonAddRouteTo<TPresenter, TConfigurator>(
            this TConfigurator configurator,
            string routeKey,
            Action<INode> childRoutes)
            where TPresenter : PresenterBase
            where TConfigurator : IConfigurator
        {
            INode child = configurator.AddRoute<TPresenter>(routeKey);
            childRoutes?.Invoke(child);

            return configurator;
        }
    }
}