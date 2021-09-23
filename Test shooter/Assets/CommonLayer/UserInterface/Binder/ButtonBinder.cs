using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CommonLayer.UserInterface.Binder
{
    public class ButtonBinder : ViewBinderBase
    {
        [SerializeField]
        public string _triggerKey;

        private Button _button;

        private bool _isInitiated;
        private IDisposable _signalSourceObserver;
        private uint _triggerId;

        protected override void OnBind(BindContext bindContext)
        {
            if (string.IsNullOrWhiteSpace(_triggerKey))
            {
                return;
            }

            if (!_isInitiated)
            {
                _triggerId = BindContext.GetHashKey(_triggerKey);
                _isInitiated = true;
            }

            _signalSourceObserver = bindContext.Trigger(_triggerId, _button.OnClickAsObservable());
        }

        protected override void OnUnbind()
        {
            _signalSourceObserver.Dispose();
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }
    }
}