using System;
using CommonLayer.DataContext;
using CommonLayer.DataContext.Settings;
using CommonLayer.DataContext.Statistics;
using CommonLayer.ResourceSystem.Interface;
using CommonLayer.SaveLoadSystem;
using CommonLayer.SceneControllers;
using CommonLayer.UserInterface.Managers;
using UnityEngine;
using Zenject;

namespace CommonLayer
{
    [CreateAssetMenu(fileName = "CommonInstaller", menuName = "Installers/Common")]
    public sealed class CommonInstaller : ScriptableObjectInstaller<CommonInstaller>
    {
        public override void InstallBindings()
        {
       
            Container.Bind<SaveLoadManager>().ToSelf().AsSingle();
            Container.Bind<ISaveLoadManager>().To<SaveLoadManager>().FromResolve();

            Container.Bind<ResourcesController>().ToSelf().AsSingle();
            Container.Bind<IResourcesController>().To<ResourcesController>().FromResolve();
       
            SceneControllersInstaller.Install(Container);
            
            Container.Bind<RootDataContext>().ToSelf().AsSingle().NonLazy();
            Container.Bind<IRootDataContext>().To<RootDataContext>().FromResolve();
            
            Container.Bind<ScoreDataContext>().ToSelf().AsSingle();
            Container.Bind<IScoreDataContext>().To<ScoreDataContext>().FromResolve();
            
            Container.Bind<CharacterSettingsDataContext>().ToSelf().AsSingle();
            Container.Bind<ICharacterSettingsDataContext>().To<CharacterSettingsDataContext>().FromResolve();
            Container.Bind<ICharacterLookSettingsInfo>().To<CharacterSettingsDataContext>().FromResolve();
            Container.Bind<ICharacterMoveSettingsInfo>().To<CharacterSettingsDataContext>().FromResolve();

            Container.Bind<IMainUiManager>().To<MainUiManager>().AsSingle();
            Container.Bind<IUiManager>().To<UiManager>().AsSingle().CopyIntoAllSubContainers();
            Container.Bind<IDisposable>().To<IUiManager>().FromResolve().CopyIntoAllSubContainers();
        }
    }
}