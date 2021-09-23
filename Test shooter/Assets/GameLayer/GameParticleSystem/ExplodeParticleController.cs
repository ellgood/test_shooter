using CommonLayer.ResourceSystem;
using CommonLayer.ResourceSystem.Interface;

namespace GameLayer.GameParticleSystem
{
    public sealed class ExplodeParticleController : ParticleControllerBase,IExplodeParticleController
    {
        public ExplodeParticleController(
            IResourcesController resourcesController, 
            ParticleFactory particleFactory) : base(resourcesController, particleFactory)
        {
        }

        protected override string ParentSlotKey => "[ExplodeParticle]";
        protected override ParticleType ParticleType => ParticleType.ExplodeHit;
    }
}