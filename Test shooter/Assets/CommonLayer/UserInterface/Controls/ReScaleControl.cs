using CommonLayer.UserInterface.Utility;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommonLayer.UserInterface.Controls
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class ReScaleControl : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _refResolution;

        private int _height;

        private RectTransform _rect;
        private int _width;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            Assert.IsTrue(_rect != null);
        }
        private void OnEnable()
        {
            _height = Screen.height;
            _width = Screen.width;
            _rect.ReScaleHeightRect(_height, _width, _refResolution);
        }
    }
}