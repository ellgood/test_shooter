using UnityEngine;

namespace GameLayer.GameParticleSystem
{
    public interface IGameParticle
    {
        ParticleType Type { get; }
        ParticleSystem ParticleSystem { get; }
        Transform Transform { get; }
        
        GameObject Instance { get; }
        void SetActive(bool state);
    }
}