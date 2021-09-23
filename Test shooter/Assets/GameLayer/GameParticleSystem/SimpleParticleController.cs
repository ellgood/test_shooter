using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Interface;

namespace GameLayer.GameParticleSystem
{
    public sealed class SimpleParticleController : ParticleControllerBase,ISimpleParticleController
    {
        public SimpleParticleController(
            IResourcesController resourcesController, 
            ParticleFactory particleFactory) : base(resourcesController, particleFactory)
        {
        }
        protected override string ParentSlotKey => "[SimpleParticle]";
        protected override ParticleType ParticleType => ParticleType.SimpleHit;
    }
}