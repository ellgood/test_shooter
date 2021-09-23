using UnityEngine;

namespace CommonLayer.UserInterface.Layering
{
    public sealed class UiLayer
    {
        private readonly RectTransform _rect;
        private readonly RectTransform _safeRect;

        public UiLayer(int order, RectTransform rect, RectTransform safeRect)
        {
            Order = order;
            _rect = rect;
            _safeRect = safeRect;
        }

        public int Order { get; }

        public Transform Container => _rect;

        public Transform SafeZoneContainer => _safeRect;
    }
}