using TMPro;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.InputField
{
    public sealed class InputFieldTextBinder : InputFieldBinderBase<string>
    {
        [SerializeField]
        private TMP_InputField.ContentType _contentType;

        protected override TMP_InputField.ContentType ContentType => _contentType;

        protected override string ToText(string value)
        {
            return value;
        }

        protected override string FromText(string value)
        {
            return value;
        }
    }
}