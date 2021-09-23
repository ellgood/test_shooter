using TMPro;

namespace CommonLayer.UserInterface.Binder.InputField
{
    public sealed class InputFieldNumberBinder : InputFieldBinderBase<int>
    {
        protected override TMP_InputField.ContentType ContentType => TMP_InputField.ContentType.IntegerNumber;

        protected override int FromText(string value)
        {
            return int.Parse(value);
        }

        protected override string ToText(int value)
        {
            return value.ToString();
        }
    }
}