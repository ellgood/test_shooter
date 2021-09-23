using CommonLayer.ResourceSystem.Config;
using CommonLayer.ResourceSystem.Config.Interfaces;
using UnityEngine;

namespace GameLayer.EnemySystem
{
    public sealed class Enemy : IEnemy
    {
        private readonly GameObject _gameObject;
        private readonly Collider _collider;
        private readonly Rigidbody _rigidbody;
        private float _health;

        public Enemy(GameObject gameObject, IEnemyConfig config)
        {
            _gameObject = gameObject;
            Config = config;
            if (_gameObject.TryGetComponent(out Collider collider))
            {
                _collider = collider;
                Hash = _collider.GetHashCode();
            }

            if (_gameObject.TryGetComponent(out Rigidbody rigidbody)) _rigidbody = rigidbody;

            _health = Config.Health.Value;
        }
        
        public IEnemyConfig Config { get; }
        
        public int Hash { get; }
        
        public bool IsAlive => _health > 0;
        public void AddForce(float force, Vector3 normal)
        {
            if (force <= 0)
            {
                return;
            }
            _rigidbody.AddForce(normal*force);
        }

        public void Destroy()
        {
            Object.Destroy(_gameObject);
        }

        public void TakeDamage(float damage)
        {
            if (_health >= damage)
                _health -= damage;
            else
                _health = 0;
        }

        public void AddExplosionForce(float force, Vector3 centerPos, float radius)
        {
            _rigidbody.AddExplosionForce(force,centerPos,radius);
        }
    }
}