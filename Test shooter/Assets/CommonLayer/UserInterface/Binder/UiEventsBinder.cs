using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(UIBehaviour))]
    public class UiEventsBinder : ViewBinderBase
    {
        [SerializeField]
        public string _onPointerDownKey;

        [SerializeField]
        public string _onPointerUpKey;

        [SerializeField]
        public string _onPointerClickKey;
        
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private UIBehaviour _ui;

        protected override void OnBind(BindContext bindContext)
        {
            if (!string.IsNullOrWhiteSpace(_onPointerDownKey))
            {
                bindContext.Trigger(_onPointerDownKey, _ui.OnPointerDownAsObservable()).AddTo(_compositeDisposable);
            }

            if (!string.IsNullOrWhiteSpace(_onPointerUpKey))
            {
                bindContext.Trigger(_onPointerUpKey, _ui.OnPointerUpAsObservable()).AddTo(_compositeDisposable);
            }
            
            if (!string.IsNullOrWhiteSpace(_onPointerClickKey))
            {
                bindContext.Trigger(_onPointerClickKey, _ui.OnPointerClickAsObservable()).AddTo(_compositeDisposable);
            }
        }

        protected override void OnUnbind()
        {
            _compositeDisposable.Clear();
        }

        private void Awake()
        {
            _ui = GetComponent<UIBehaviour>();
        }
    }
}