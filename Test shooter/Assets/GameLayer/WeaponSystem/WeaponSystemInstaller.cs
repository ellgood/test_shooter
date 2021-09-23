using System;
using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;
using Zenject;

namespace GameLayer.WeaponSystem
{
    public class WeaponSystemInstaller : Installer<WeaponSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<WeaponSceneController>().ToSelf().AsSingle();
            Container.Bind<IWeaponSceneController>().To<WeaponSceneController>().FromResolve();
            Container.Bind<IInitializable>().To<WeaponSceneController>().FromResolve();
            Container.Bind<IDisposable>().To<WeaponSceneController>().FromResolve();
            
            Container.BindFactory<Vector3,Transform,WeaponData,IWeapon, WeaponFactory>().FromFactory<CustomWeaponFactory>();
        }
    }
}