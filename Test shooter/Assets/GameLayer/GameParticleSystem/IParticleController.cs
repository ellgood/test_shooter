using UnityEngine;

namespace GameLayer.GameParticleSystem
{
    public interface IExplodeParticleController : IParticleController
    {
        
    }
    
    public interface ISimpleParticleController : IParticleController
    {
        
    }
    
    public interface IMuzzleParticleController : IParticleController
    {
        
    }
    
    public interface IParticleController
    {
        void Play(Vector3 position,Vector3 normal);
    }
}