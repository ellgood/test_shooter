using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Config;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;
using Zenject;


namespace GameLayer.EnemySystem
{
    public sealed class EnemyFactory : PlaceholderFactory<Vector3,EnemyData,IEnemy>
    {
    }
}