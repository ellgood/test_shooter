using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(UIBehaviour))]
    public class SetFocusBinder : ViewBinderBase
    {
        [SerializeField]
        private string _key;

        [SerializeField]
        private GameObject _target;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        protected override void OnBind(BindContext bindContext)
        {
            if (!string.IsNullOrWhiteSpace(_key))
            {
                bindContext.Signal(_key, OnSetTarget).AddTo(_disposable);
            }
        }

        protected override void OnUnbind()
        {
            _disposable.Clear();
        }

        protected virtual void OnSetTarget()
        {
            CheckTarget();
            EventSystem.current.SetSelectedGameObject(_target);
        }

        private void CheckTarget()
        {
            if (_target == null)
            {
                _target = gameObject;
            }
        }
    }
}