using System;
using UnityEngine;

namespace CommonLayer.UserInterface.Views
{
    [RequireComponent(typeof(ViewElement))]
    public abstract class ViewAnimationBase : MonoBehaviour
    {
        public abstract void HideAnimation(Action hiddenCallback);

        public abstract void ShowAnimation(Action showedCallback);

        public abstract void ResetAnimation(bool hidden = true);
    }
}