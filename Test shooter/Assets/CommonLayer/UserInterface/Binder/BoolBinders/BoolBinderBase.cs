using System;
using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;


namespace CommonLayer.UserInterface.Binder.BoolBinders
{
    public abstract class BoolBinderBase : ViewBinderBase
    {
        [SerializeField]
        private string _boolKey;

        [SerializeField]
        private bool _invert;

        private bool _deactivate;

        private IDisposable _singleDisposable;

        protected sealed override void OnBind(BindContext bindContext)
        {
            _singleDisposable = bindContext.Bind(_boolKey, Observer.Create<bool>(StateChanged));

            if (!_deactivate)
            {
                return;
            }
            
            _singleDisposable.Dispose();
            _deactivate = false;
        }

        protected sealed override void OnUnbind()
        {
            if (_singleDisposable == null)
            {
                _deactivate = true;
            }
            else
            {
                _singleDisposable.Dispose();
            }
        }

        protected abstract void OnStateChanged(bool state);

        private void StateChanged(bool state)
        {
            state ^= _invert;
            OnStateChanged(state);
        }
    }
}