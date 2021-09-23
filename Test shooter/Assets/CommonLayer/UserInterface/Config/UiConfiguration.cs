using CommonLayer.ResourceSystem;
using CommonLayer.UserInterface.Layering;
using UnityEngine;

namespace CommonLayer.UserInterface.Config
{
    [CreateAssetMenu(menuName = "Config/UI", fileName = "UserInterface")]
    public sealed class UiConfiguration : ResourcesDataBase<ViewData>
    {
        [SerializeField]
        private LayerManager _layerManager;

        [SerializeField]
        private ViewData _viewsData;

        public LayerManager LayerManager => _layerManager;

        public override ViewData Content => _viewsData;
    }
}