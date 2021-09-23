namespace CommonLayer.UserInterface.Binder.Image
{
    public sealed class FilledImageWithDeltaBinder : FillItemBinder
    {
        private void Awake()
        {
            _image = GetComponent<UnityEngine.UI.Image>();
        }
        
        private UnityEngine.UI.Image _image;

        protected override void OnRefreshValue(float value)
        {
            _image.fillAmount = 1f - value;
        }
    }
}