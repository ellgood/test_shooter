using UnityEngine;

namespace CommonLayer.UserInterface.Binder.BoolBinders
{
    [RequireComponent(typeof(Behaviour))]
    public sealed class InteractionStateBinder : BoolBinderBase
    {
        [SerializeField]
        private Behaviour _interactionComponent;

        protected override void OnStateChanged(bool state)
        {
            _interactionComponent.enabled = state;
        }

        private void Awake()
        {
            if (_interactionComponent == null)
            {
                _interactionComponent = GetComponent<Behaviour>();
            }
        }
    }
}