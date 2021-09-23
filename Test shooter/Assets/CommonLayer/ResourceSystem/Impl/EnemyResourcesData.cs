using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Impl
{
    [CreateAssetMenu(fileName = "EnemyResources", menuName = "DataBases/EnemyResources")]
    public sealed class EnemyResourcesData : ResourcesDataBase<EnemyData>
    {
        [SerializeField]
        private EnemyData enemyData;
        public override EnemyData Content => enemyData;
    }
}