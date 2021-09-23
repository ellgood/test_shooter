using System;
using CommonLayer.UserInterface.DataBinding;
using CommonLayer.UserInterface.DataBinding.Binds;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CommonLayer.UserInterface.Binder
{
    public sealed class HoldTriggerBinder : ViewBinderBase, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField]
        private string _pointerHoldKey;

        private readonly ReactiveProperty<bool> _holdPointerProperty = new ReactiveProperty<bool>();

        private IDisposable _signalSourceObserver;

        #region IPointerDownHandler Implementation

        public void OnPointerDown(PointerEventData eventData)
        {
            _holdPointerProperty.Value = true;
        }

        #endregion

        #region IPointerUpHandler Implementation

        public void OnPointerUp(PointerEventData eventData)
        {
            _holdPointerProperty.Value = false;
        }

        #endregion

        protected override void OnBind(BindContext bindContext)
        {
            _signalSourceObserver = bindContext.Bind(_pointerHoldKey, _holdPointerProperty, RelationType.OneWay);
        }

        protected override void OnUnbind()
        {
            _signalSourceObserver.Dispose();
        }
    }
}