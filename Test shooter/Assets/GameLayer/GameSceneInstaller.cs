using CommonLayer.SceneControllers;
using GameLayer.CharacterSystem;
using GameLayer.DamageSystem;
using GameLayer.EnemySystem;
using GameLayer.GameCameraSystem;
using GameLayer.GameParticleSystem;
using GameLayer.SpawnSystem;
using GameLayer.WeaponSystem;
using UnityEngine;
using Zenject;

namespace GameLayer
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        [SerializeField]
        private SpawnPointsPreset spawnPreset;

        [SerializeField] 
        private GameCameraHolder gameCameraHolder;
        
        public override void InstallBindings()
        {
            Container.BindSceneController<GameSceneController>();

            Container.Bind<GameCameraHolder>().FromInstance(gameCameraHolder).AsSingle();
            Container.Bind<IGameCameraHolder>().To<GameCameraHolder>().FromResolve();
            
            Container.Bind<SpawnPointsPreset>().FromInstance(spawnPreset).AsSingle();
            Container.Bind<ISpawnPointsPreset>().To<SpawnPointsPreset>().FromResolve();
            
            Container.Bind<SpawnManager>().ToSelf().AsSingle();
            Container.Bind<ISpawnManager>().To<SpawnManager>().FromResolve();
            Container.Bind<IInitializable>().To<SpawnManager>().FromResolve();
            
            Container.Bind<DamageController>().ToSelf().AsSingle();
            Container.Bind<IDamageController>().To<DamageController>().FromResolve();
       
            ParticleSystemInstaller.Install(Container);
            EnemySystemInstaller.Install(Container);
            WeaponSystemInstaller.Install(Container);
            CharacterSystemInstaller.Install(Container);
        }
    }
}
