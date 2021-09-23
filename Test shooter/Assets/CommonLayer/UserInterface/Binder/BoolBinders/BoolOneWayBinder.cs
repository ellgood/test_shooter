using UnityEngine;
using UnityEngine.Events;

namespace CommonLayer.UserInterface.Binder.BoolBinders
{
    public sealed class BoolOneWayBinder : BoolBinderBase
    {
        [SerializeField]
        private UnityEvent<bool> _boolEvent;

        protected override void OnStateChanged(bool state)
        {
            _boolEvent.Invoke(state);
        }
    }
}