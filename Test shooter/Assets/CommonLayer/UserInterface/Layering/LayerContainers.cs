using UnityEngine;

namespace CommonLayer.UserInterface.Layering
{
    [RequireComponent(typeof(Canvas))]
    public sealed class LayerContainers : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _normalContainer;

        [SerializeField]
        private RectTransform _safeContainer;

        public RectTransform NormalContainer => _normalContainer;

        public RectTransform SafeContainer => _safeContainer;

        private void Awake()
        {
            if (!NormalContainer)
            {
                _normalContainer = GetComponent<RectTransform>();
            }

            if (!SafeContainer)
            {
                _safeContainer = NormalContainer;
            }
        }
    }
}