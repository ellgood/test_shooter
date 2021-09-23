using System.Collections.Generic;
using System.Linq;
using CommonLayer.UserInterface.Pooling;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommonLayer.UserInterface.Layering
{
    public sealed class LayerManager : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _layersContainer;

        [SerializeField]
        private RectTransform _reservedContainer;

        [SerializeField]
        private GameObject _layerTemplate;

        private readonly Dictionary<int, UiLayer> _layerDictionary = new Dictionary<int, UiLayer>();
        private UnityPool _pool;
        public Rect CanvasRect => _layersContainer.rect;

        private void Awake()
        {
            Assert.IsNotNull(_layersContainer, "Layer container was not set");
            Assert.IsNotNull(_reservedContainer, "Reserved container was not set");

            _pool = new UnityPool(_layerTemplate, _layersContainer, _reservedContainer);
        }

        public UiLayer GetLayer(int order)
        {
            if (_layerDictionary.TryGetValue(order, out UiLayer layer))
            {
                return layer;
            }

            layer = CreateLayer(order);
            _layerDictionary.Add(order, layer);

            SortByOrder();

            return layer;
        }

        private UiLayer CreateLayer(int order)
        {
            GameObject obj = _pool.Rent("layer_" + order);

            var rect = obj.GetComponent<RectTransform>();

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;

            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var canvas = obj.GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = order;

            var layerContainers = obj.GetComponent<LayerContainers>();

            UiLayer layer;
            if (layerContainers)
            {
                layer = new UiLayer(order, layerContainers.NormalContainer ? layerContainers.NormalContainer : rect,
                                    layerContainers.SafeContainer ? layerContainers.SafeContainer : rect);
            }
            else
            {
                layer = new UiLayer(order, rect, rect);
            }

            return layer;
        }

        private void SortByOrder()
        {
            foreach (UiLayer layer in _layerDictionary.Values.OrderBy(v => v.Order))
            {
                layer.Container.SetAsLastSibling();
            }
        }
    }
}