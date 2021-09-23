using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;
using Zenject;

namespace GameLayer.EnemySystem
{
    public class EnemySystemInstaller : Installer<EnemySystemInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<EnemyController>().AsSingle();
            Container.BindFactory<Vector3,EnemyData,IEnemy, EnemyFactory>().FromFactory<CustomEnemyFactory>();
        }
    }
}