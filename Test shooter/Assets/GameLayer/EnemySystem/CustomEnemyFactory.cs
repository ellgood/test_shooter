using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Data;
using UnityEngine;
using Zenject;

namespace GameLayer.EnemySystem
{
    public sealed class CustomEnemyFactory : IFactory<Vector3,EnemyData,IEnemy>
    {
        private const string ParentSlotKey = "[Enemies]";
        private readonly DiContainer _container;
        private GameObject _parent;

        public CustomEnemyFactory(DiContainer container)
        {
            _container = container;
        }
        public IEnemy Create(Vector3 position, EnemyData data)
        {
            if (_parent == null)
            {
                _parent = new GameObject(ParentSlotKey);
                Object.DontDestroyOnLoad(_parent);
            }

            var gameObject = _container.InstantiatePrefab(data.Prefab, position, Quaternion.identity, _parent.transform);
            gameObject.AddComponent<Rigidbody>();
            return _container.Instantiate<Enemy>(new object[] { gameObject ,data.Config});
        }
    }
}