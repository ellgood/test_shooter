using System;
using Zenject;

namespace CommonLayer.SceneControllers
{
    public static class SceneControllerExtensions
    {
        public static void BindSceneController<TContract, T>(this DiContainer container)
            where T : SceneControllerBase, IInitializable, TContract
        {
            BindSceneController<T>(container, typeof(TContract));
        }

        public static void BindSceneController<TContract1, TContract2, T>(this DiContainer container)
            where T : SceneControllerBase, IInitializable, TContract1, TContract2
        {
            BindSceneController<T>(container, typeof(TContract1), typeof(TContract2));
        }

        public static void BindSceneController<TContract1, TContract2, TContract3, T>(this DiContainer container)
            where T : SceneControllerBase, IInitializable, TContract1, TContract2, TContract3
        {
            BindSceneController<T>(container, typeof(TContract1), typeof(TContract2), typeof(TContract3));
        }

        public static void BindSceneController<T>(this DiContainer container, params Type[] contractTypes)
            where T : SceneControllerBase, IInitializable
        {
            Guid guid = BindController<T>(container);
            container.Bind(contractTypes).To<T>().FromResolve(guid);
        }

        public static void BindSceneController<T>(this DiContainer container)
            where T : SceneControllerBase, IInitializable
        {
            BindController<T>(container);
        }

        private static Guid BindController<T>(DiContainer container)
            where T : SceneControllerBase, IInitializable
        {
            var guid = Guid.NewGuid();

            container.Bind<T>().WithId(guid).AsSingle();

            container.Bind<ISceneController>().To<T>().FromResolve(guid);
            container.Bind<IInitializable>().To<T>().FromResolve(guid);

            container.BindInitializableExecutionOrder<T>(int.MinValue);

            return guid;
        }
    }
}