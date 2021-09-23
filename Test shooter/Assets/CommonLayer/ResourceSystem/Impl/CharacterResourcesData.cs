using CommonLayer.ResourceSystem.Data;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Impl
{
    [CreateAssetMenu(fileName = "CharacterResources", menuName = "DataBases/CharacterResources")]
    public sealed class CharacterResourcesData : ResourcesDataBase<CharacterData>
    {
        [SerializeField]
        private CharacterData characterData;
        public override CharacterData Content => characterData;
    }
}