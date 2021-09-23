using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(Scrollbar))]
    public class ScrollBarValueBinder : ViewBinderBase
    {
        [SerializeField]
        private string _key;

        [SerializeField]
        private Scrollbar _scrollBar;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        protected override void OnBind(BindContext bindContext)
        {
            if (!string.IsNullOrWhiteSpace(_key))
            {
                bindContext.Signal<Vector2>(_key, OnMove).AddTo(_disposable);
            }
        }

        protected override void OnUnbind()
        {
            _disposable.Clear();
        }

        private void Awake()
        {
            CheckTarget();
        }

        private void OnMove(Vector2 position)
        {
            CheckTarget();
            if (_scrollBar.direction == Scrollbar.Direction.BottomToTop || _scrollBar.direction == Scrollbar.Direction.TopToBottom)
            {
                _scrollBar.value = position.y;
            }
            else
            {
                _scrollBar.value = position.x;
            }
        }

        private void CheckTarget()
        {
            if (_scrollBar == null)
            {
                _scrollBar = gameObject.GetComponent<Scrollbar>();
            }
        }
    }
}