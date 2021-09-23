using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class AnchoredPositionBinder : ViewBinderBase
    {
        [SerializeField]
        private string _key;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private RectTransform _parentRectTransform;

        private RectTransform _rectTransform;

        protected override void OnBind(BindContext bindContext)
        {
            _parentRectTransform = _rectTransform.parent.GetComponent<RectTransform>();

            if (string.IsNullOrWhiteSpace(_key))
            {
                throw new Exception("Incorrect color binder key.");
            }

            bindContext.Bind(_key, Observer.Create<Vector2>(OnPositionChanged)).AddTo(_disposable);
        }

        protected override void OnUnbind()
        {
            _disposable.Clear();
        }

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnPositionChanged(Vector2 position)
        {
            Vector2 scaledParent = Vector2.Scale(_parentRectTransform.sizeDelta, _rectTransform.pivot);
            Vector2 newPosition = position - scaledParent;
            _rectTransform.anchoredPosition = newPosition / _parentRectTransform.lossyScale;
        }
    }
}