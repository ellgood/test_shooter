using System;
using GameLayer.CharacterSystem.CharacterLookSystem;
using GameLayer.CharacterSystem.CharacterMovementSystem;
using GameLayer.CharacterSystem.CharacterWeaponsSystem;
using GameLayer.CharacterSystem.CharacterWeaponsSystem.ShootingSystem;
using UnityEngine;
using Zenject;

namespace GameLayer.CharacterSystem
{
    public sealed class CharacterSystemInstaller: Installer<CharacterSystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindFactory<GameObject,Vector3,ICharacter, CharacterFactory>().FromFactory<CustomCharacterFactory>();
            
            Container.Bind<GameCharacterController>().ToSelf().AsSingle();
            Container.Bind<IGameCharacterController>().To<GameCharacterController>().FromResolve();
            Container.Bind<IInitializable>().To<GameCharacterController>().FromResolve();
            Container.Bind<IDisposable>().To<GameCharacterController>().FromResolve();
            Container.Bind<ITickable>().To<GameCharacterController>().FromResolve();
            Container.Bind<IFixedTickable>().To<GameCharacterController>().FromResolve();
            
            Container.Bind<CharacterLookControl>().ToSelf().AsSingle();
            Container.Bind<ICharacterLookControl>().To<CharacterLookControl>().FromResolve();

            Container.Bind<CharacterMovementControl>().ToSelf().AsSingle();
            Container.Bind<ICharacterMovementControl>().To<CharacterMovementControl>().FromResolve();

            Container.Bind<CharacterWeaponSelectControl>().ToSelf().AsSingle();
            Container.Bind<ICharacterWeaponSelectControl>().To<CharacterWeaponSelectControl>().FromResolve();
            Container.Bind<ICharacterWeaponSelected>().To<CharacterWeaponSelectControl>().FromResolve();
            
            Container.Bind<CharacterWeaponShootingControl>().ToSelf().AsSingle();
            Container.Bind<ICharacterWeaponShootingControl>().To<CharacterWeaponShootingControl>().FromResolve();
            Container.Bind<IShootingSetWeaponProvider>().To<CharacterWeaponShootingControl>().FromResolve();
        }
    }
}