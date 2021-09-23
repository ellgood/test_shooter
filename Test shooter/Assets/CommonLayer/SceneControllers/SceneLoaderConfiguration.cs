using CommonLayer.ResourceSystem;
using UnityEngine;

namespace CommonLayer.SceneControllers
{
    [CreateAssetMenu(menuName = "Config/Scene", fileName = "SceneLoader")]
    public sealed class SceneLoaderConfiguration : ResourcesDataBase<SceneLoaderData>
    {
        [SerializeField]
        private SceneLoaderData _content;

        public override SceneLoaderData Content => _content;
    }
}