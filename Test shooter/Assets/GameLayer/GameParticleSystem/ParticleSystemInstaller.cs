using System;
using UnityEngine;
using Zenject;

namespace GameLayer.GameParticleSystem
{
    public class ParticleSystemInstaller : Installer<ParticleSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<ExplodeParticleController>().ToSelf().AsSingle();
            Container.Bind<IExplodeParticleController>().To<ExplodeParticleController>().FromResolve();
            Container.Bind<IInitializable>().To<ExplodeParticleController>().FromResolve();
            Container.Bind<IDisposable>().To<ExplodeParticleController>().FromResolve();
            Container.Bind<ITickable>().To<ExplodeParticleController>().FromResolve();

            Container.Bind<SimpleParticleController>().ToSelf().AsSingle();
            Container.Bind<ISimpleParticleController>().To<SimpleParticleController>().FromResolve();
            Container.Bind<IInitializable>().To<SimpleParticleController>().FromResolve();
            Container.Bind<IDisposable>().To<SimpleParticleController>().FromResolve();
            Container.Bind<ITickable>().To<SimpleParticleController>().FromResolve();

            Container.Bind<MuzzleParticleController>().ToSelf().AsSingle();
            Container.Bind<IMuzzleParticleController>().To<MuzzleParticleController>().FromResolve();
            Container.Bind<IInitializable>().To<MuzzleParticleController>().FromResolve();
            Container.Bind<IDisposable>().To<MuzzleParticleController>().FromResolve();
            Container.Bind<ITickable>().To<MuzzleParticleController>().FromResolve();

            Container.BindFactory<GameObject, ParticleType, Transform, IGameParticle, ParticleFactory>()
                .FromFactory<CustomParticleFactory>();
        }
    }
}