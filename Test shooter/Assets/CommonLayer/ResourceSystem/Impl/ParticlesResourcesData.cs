using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Impl
{
    [CreateAssetMenu(fileName = "ParticlesResources", menuName = "DataBases/ParticlesResources")]
    public sealed class ParticlesResourcesData : ResourcesDataBase<ParticlesData>
    {
        [SerializeField] 
        private ParticlesData particlesData;

        public override ParticlesData Content => particlesData;
    }
}