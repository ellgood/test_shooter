using CommonLayer.UserInterface.DataBinding;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder
{
    public abstract class ViewBinderBase : MonoBehaviour
    {
        public BindContext Context { get; private set; }

        public bool IsBound { get; private set; }

        public virtual bool ActivateByState { get; } = true;

        protected virtual void OnEnable()
        {
            //DebugLog.Warning($"OnEnable({GetType().Name}, {name})");
            if (IsBound && ActivateByState)
            {
                OnBind(Context);
            }
        }

        protected virtual void OnDisable()
        {
            //DebugLog.Warning($"OnDisable({GetType().Name}, {name})");
            if (IsBound && ActivateByState)
            {
                OnUnbind();
            }
        }

        protected abstract void OnBind(BindContext bindContext);

        protected abstract void OnUnbind();

        internal void Bind(BindContext bindContext)
        {
            //DebugLog.Warning($"Bind({GetType().Name}, {name})");
            if (IsBound)
            {
                Unbind();
            }

            Context = bindContext;
            if (bindContext == null)
            {
                return;
            }

            if (!ActivateByState || isActiveAndEnabled)
            {
                OnBind(bindContext);
            }

            IsBound = true;
        }

        internal void Unbind()
        {
            //DebugLog.Warning($"Unbind({GetType().Name}, {name})");
            if (!IsBound)
            {
                return;
            }

            if (!ActivateByState || isActiveAndEnabled)
            {
                OnUnbind();
            }

            Context = null;
            IsBound = false;
        }
    }
}