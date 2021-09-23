using CommonLayer.UserInterface.DataBinding;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CommonLayer.UserInterface.Binder
{
    [RequireComponent(typeof(UIBehaviour))]
    public class SelectableBinder : ViewBinderBase, ISelectHandler, IDeselectHandler
    {
        [SerializeField]
        private string _key;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private readonly ReactiveCommand<bool> _selectCommand = new ReactiveCommand<bool>();

        #region IDeselectHandler Implementation

        public void OnDeselect(BaseEventData eventData)
        {
            _selectCommand.Execute(false);
        }

        #endregion

        #region ISelectHandler Implementation

        public void OnSelect(BaseEventData eventData)
        {
            _selectCommand.Execute(true);
        }

        #endregion

        protected override void OnBind(BindContext bindContext)
        {
            if (!string.IsNullOrWhiteSpace(_key))
            {
                bindContext.Trigger(_key, _selectCommand).AddTo(_disposable);
            }
        }

        protected override void OnUnbind()
        {
            _disposable.Clear();
        }
    }
}