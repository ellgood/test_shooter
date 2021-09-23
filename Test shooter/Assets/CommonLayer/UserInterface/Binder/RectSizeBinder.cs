using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(RectTransform))]
    public class RectSizeBinder : ViewBinderBase
    {
        [SerializeField]
        private string _sizeKey;

        [SerializeField]
        private RectTransform _targetTransform;

        private IDisposable _signalSourceObserver;

        protected override void OnBind(BindContext bindContext)
        {
            _signalSourceObserver = bindContext.Bind(_sizeKey, Observer.Create<Vector2>(SizeChanged));
        }

        protected override void OnUnbind()
        {
            _signalSourceObserver.Dispose();
        }

        private void Awake()
        {
            if (_targetTransform == null)
            {
                _targetTransform = GetComponent<RectTransform>();
            }
        }

        private void SizeChanged(Vector2 size)
        {
            _targetTransform.sizeDelta = size;
        }
    }
}