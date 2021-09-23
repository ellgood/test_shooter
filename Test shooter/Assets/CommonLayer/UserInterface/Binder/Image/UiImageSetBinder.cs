using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Image
{
    [RequireComponent(typeof(UnityEngine.UI.Image))]
    public sealed class UiImageSetBinder : ViewBinderBase
    {
        [SerializeField]
        private string _spriteIdxKey;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private UnityEngine.UI.Image _image;

        [SerializeField]
        private Sprite[] _sprites;
        
        protected override void OnBind(BindContext bindContext)
        {
            if (string.IsNullOrWhiteSpace(_spriteIdxKey))
            {
                return;
            }

            bindContext.Bind(_spriteIdxKey, Observer.Create<int>(SpriteIdxChanged)).AddTo(_disposable);
        }
        
        protected override void OnUnbind()
        {
            _disposable.Clear();
            _image.sprite = null;
        }
        
        private void Awake()
        {
            _image = GetComponent<UnityEngine.UI.Image>();
        }

        private void SpriteIdxChanged(int idx)
        {
            if (idx < 0 || idx >= _sprites.Length)
            {
                return;
            }
            
            _image.sprite = _sprites[idx];
        }
    }
}