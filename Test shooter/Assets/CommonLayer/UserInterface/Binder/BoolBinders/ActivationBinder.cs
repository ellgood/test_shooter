using UnityEngine;

namespace CommonLayer.UserInterface.Binder.BoolBinders
{
    public sealed class ActivationBinder : BoolBinderBase
    {
        [SerializeField]
        private GameObject _targetObject;

        public override bool ActivateByState { get; } = false;

        protected override void OnStateChanged(bool state)
        {
            _targetObject.SetActive(state);
        }

        private void Awake()
        {
            if (_targetObject == null)
            {
                _targetObject = gameObject;
            }
        }
    }
}