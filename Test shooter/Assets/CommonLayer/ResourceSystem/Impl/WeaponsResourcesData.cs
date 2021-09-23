using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Impl
{
    [CreateAssetMenu(fileName = "WeaponsResources", menuName = "DataBases/WeaponsResources")]
    public sealed class WeaponsResourcesData : ResourcesDataBase<WeaponsData>
    {
        [SerializeField]
        private WeaponsData weaponsData;
        public override WeaponsData Content => weaponsData;
    }
}