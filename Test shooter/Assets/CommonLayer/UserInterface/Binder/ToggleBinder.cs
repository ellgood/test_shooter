using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleBinder : ViewBinderBase
    {
        [SerializeField]
        private string _toggleKey;

        private IDisposable _signalSourceObserver;

        private Toggle _toggle;

        protected override void OnBind(BindContext bindContext)
        {
            _signalSourceObserver = bindContext.Bind(_toggleKey, _toggle.onValueChanged.AsObservable(), Observer.Create<bool>(OnValueChanged));
        }

        protected override void OnUnbind()
        {
            _signalSourceObserver.Dispose();
        }

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
        }

        private void OnValueChanged(bool value)
        {
            _toggle.isOn = value;
        }
    }
}