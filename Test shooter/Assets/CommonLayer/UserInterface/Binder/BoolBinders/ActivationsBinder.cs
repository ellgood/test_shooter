using System;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.BoolBinders
{
    public sealed class ActivationsBinder : BoolBinderBase
    {
        [SerializeField]
        private BindableTarget[] _targets;

        protected override void OnStateChanged(bool state)
        {
            foreach (BindableTarget bindableTarget in _targets)
            {
                if (bindableTarget.Target)
                {
                    bindableTarget.Target.SetActive(state ^ bindableTarget.Invert);
                }
            }
        }

        [Serializable]
        public struct BindableTarget
        {
            public GameObject Target;

            public bool Invert;
        }
    }
}