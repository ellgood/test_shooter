using UnityEngine;
using Zenject;

namespace GameLayer.GameParticleSystem
{
    public class CustomParticleFactory : IFactory<GameObject,ParticleType, Transform, IGameParticle>
    {
        private readonly DiContainer _container;

        public CustomParticleFactory(DiContainer container)
        {
            _container = container;
        }

        public IGameParticle Create(GameObject prefab,ParticleType type,Transform parent)
        {
            var instance = _container.InstantiatePrefab(prefab, parent);
            return _container.Instantiate<GameParticle>(new object[] {instance, type });
        }
    }
}