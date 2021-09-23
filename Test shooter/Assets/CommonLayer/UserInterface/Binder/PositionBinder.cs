using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(RectTransform))]
    public class PositionBinder : ViewBinderBase
    {
        [SerializeField]
        private string _positionKey;

        [SerializeField]
        private RectTransform _targetTransform;

        private IDisposable _signalSourceObserver;

        protected override void OnBind(BindContext bindContext)
        {
            _signalSourceObserver = bindContext.Bind(_positionKey, Observer.Create<Vector2>(PositionChanged));
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

        private void PositionChanged(Vector2 position)
        {
            _targetTransform.position = position;
        }
    }
}