using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Impl
{
    [CreateAssetMenu(fileName = "BulletsResources", menuName = "DataBases/BulletsResources")]
    public sealed class BulletsResourcesData : ResourcesDataBase<BulletsData>
    {
        [SerializeField] 
        private BulletsData bulletsData;

        public override BulletsData Content => bulletsData;
    }
}