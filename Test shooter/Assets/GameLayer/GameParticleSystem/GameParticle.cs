using UnityEngine;

namespace GameLayer.GameParticleSystem
{
    public sealed class GameParticle : IGameParticle
    {
        public GameParticle(GameObject instance, ParticleType type)
        {
            Instance = instance;
            ParticleSystem = Instance.GetComponent<ParticleSystem>();
            Type = type;
            Transform = Instance.transform;
          
        }
        public ParticleSystem ParticleSystem { get; }
        public ParticleType Type { get; }
        public Transform Transform { get; }
        public GameObject Instance { get; }

        public void SetActive(bool state)
        {
            Instance.SetActive(state);
        }
    }
}