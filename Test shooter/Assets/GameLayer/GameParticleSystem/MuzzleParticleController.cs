using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Interface;

namespace GameLayer.GameParticleSystem
{
    public sealed class MuzzleParticleController : ParticleControllerBase,IMuzzleParticleController
    {
        public MuzzleParticleController(
            IResourcesController resourcesController, 
            ParticleFactory particleFactory) : base(resourcesController, particleFactory)
        {
        }
        
        protected override string ParentSlotKey => "[MuzzleParticle]";

        protected override ParticleType ParticleType => ParticleType.Muzzle;
    }
}