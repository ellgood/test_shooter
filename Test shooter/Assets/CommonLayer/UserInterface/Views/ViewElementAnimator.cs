using System;
using CommonLayer.UserInterface.Views.StateMachines;
using UnityEngine;

namespace CommonLayer.UserInterface.Views
{
    [RequireComponent(typeof(Animator))]
    public sealed class ViewElementAnimator : ViewAnimationBase
    {
        [SerializeField]
        private readonly string _animatorBoolKey = "isShowing";

        [SerializeField]
        private readonly string _hidedTrigger = "hide";

        [SerializeField]
        private readonly string _showedTrigger = "show";

        private Action _hideCallback;

        private Action _showCallback;
        private UiStateMachineHiding _stateMachineHiding;

        private UiStateMachineShowing _stateMachineShowing;
        private Animator _viewAnimator;

        private void Awake()
        {
            _viewAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _stateMachineShowing = _viewAnimator.GetBehaviour<UiStateMachineShowing>();
            _stateMachineHiding = _viewAnimator.GetBehaviour<UiStateMachineHiding>();

            _stateMachineShowing.OnExit += OnShowingExit;
            _stateMachineHiding.OnExit += OnHidingExit;
        }

        private void OnDisable()
        {
            _stateMachineShowing.OnExit -= OnShowingExit;
            _stateMachineHiding.OnExit -= OnHidingExit;
        }

        public override void HideAnimation(Action hiddenCallback)
        {
            _viewAnimator.SetBool(_animatorBoolKey, false);
            _hideCallback = hiddenCallback;
        }

        public override void ShowAnimation(Action showedCallback)
        {
            _viewAnimator.SetBool(_animatorBoolKey, true);
            _showCallback = showedCallback;
        }

        public override void ResetAnimation(bool hidden = true)
        {
            _viewAnimator.SetTrigger(hidden ? _hidedTrigger : _showedTrigger);
        }

        private void OnHidingExit()
        {
            _hideCallback?.Invoke();
        }

        private void OnShowingExit()
        {
            _showCallback?.Invoke();
        }
    }
}