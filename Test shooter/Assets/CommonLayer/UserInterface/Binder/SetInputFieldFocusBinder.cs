using TMPro;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(TMP_InputField))]
    public sealed class SetInputFieldFocusBinder : SetFocusBinder
    {
        private TMP_InputField _inputField;

        protected override void OnSetTarget()
        {
            base.OnSetTarget();
            _inputField.ActivateInputField();
        }

        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
        }
    }
}