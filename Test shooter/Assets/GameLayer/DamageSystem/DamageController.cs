using CommonLayer.DataContext.Statistics;
using CommonLayer.ResourceSystem.Config.Interfaces;
using GameLayer.CharacterSystem.CharacterMovementSystem;
using GameLayer.EnemySystem;
using UnityEngine;

namespace GameLayer.DamageSystem
{
    public class DamageController : IDamageController
    {
        private readonly IEnemyController _enemyController;
        private readonly IScoreDataContext _scoreCtx;
        private readonly ICharacterMovementControl _movementControl;

        public DamageController(IEnemyController enemyController, IScoreDataContext scoreCtx, ICharacterMovementControl movementControl)
        {
            _enemyController = enemyController;
            _scoreCtx = scoreCtx;
            _movementControl = movementControl;
        }

        public void CheckOfExpDamage(Collider collider, Vector3 center, float damageMtp, IBulletConfig bulletConfig)
        {
            var hash = collider.GetHashCode();
            if (!_enemyController.TryGetEnemy(hash, out var enemy)) return;

            var sitDownFactor = _movementControl.SitDownFlag ? 2 : 1;
            var pos = collider.transform.position;
            float dist = Vector3.Distance(pos, center);
            var ratio = Mathf.Clamp01( 1 - dist/ bulletConfig.ExplosionRadius );
            var damage = bulletConfig.Damage * ratio * damageMtp * sitDownFactor;
            
            enemy.TakeDamage(damage);
            enemy.AddExplosionForce(bulletConfig.Force,center,bulletConfig.ExplosionRadius);

            if (!_enemyController.CheckAlive(hash))
            {
                _scoreCtx.AddScore(2); 
            }
        }

        public void CheckOfSimpleDamage(Collider collider, Vector3 normal, float damageMtp, IBulletConfig bulletConfig)
        {
            var hash = collider.GetHashCode();
            if (!_enemyController.TryGetEnemy(hash, out var enemy)) return;
            enemy.TakeDamage(bulletConfig.Damage * damageMtp);
            enemy.AddForce(bulletConfig.Force, -normal);
            if (!_enemyController.CheckAlive(hash))
            {
                _scoreCtx.AddScore(1); 
            }
        }
    }
}