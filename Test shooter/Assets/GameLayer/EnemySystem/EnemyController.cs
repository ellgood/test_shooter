using System;
using System.Collections.Generic;

using CommonLayer.ResourceSystem.Impl;
using CommonLayer.ResourceSystem.Interface;
using GameLayer.SpawnSystem;
using Zenject;

namespace GameLayer.EnemySystem
{
    public class EnemyController : IEnemyController, IInitializable, IDisposable
    {
        private readonly EnemyFactory _enemyFactory;

        private readonly Dictionary<int, IEnemy> _hashToEnemy = new Dictionary<int, IEnemy>();
        private readonly IResourcesController _resourcesController;
    
        private readonly ISpawnManager _spawnManager;


        public EnemyController(
            EnemyFactory enemyFactory,
            IResourcesController resourcesController,
         
            ISpawnManager spawnManager)
        {
            _enemyFactory = enemyFactory;
            _resourcesController = resourcesController;
            _spawnManager = spawnManager;
        }

        public void Dispose()
        {
            foreach (var kvp in _hashToEnemy) kvp.Value.Destroy();
            _hashToEnemy.Clear();
        }
        
        public bool TryGetEnemy(int hitHash, out IDamagedEnemy damagedEnemy)
        {
            if (!_hashToEnemy.TryGetValue(hitHash, out var enemy))
            {
                damagedEnemy = default;
                return false;
            }

            damagedEnemy = enemy;
            return true;
        }

        public bool CheckAlive(int hitHash)
        {
            var enemy = _hashToEnemy[hitHash];
            if (enemy.IsAlive) return true;
            enemy.Destroy();
            _hashToEnemy.Remove(hitHash);
            return false;
        }

        public void Initialize()
        {
            var enemyData = _resourcesController.GetData<EnemyResourcesData>().Content;

            while (_spawnManager.TryGetFreeSpawnPoint(SpawnPointFlags.Enemy, out var point))
            {
                var enemy = _enemyFactory.Create(point.Position, enemyData);
                _hashToEnemy[enemy.Hash] = enemy;
            }
        }
    }
}