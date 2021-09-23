using System;
using UnityEngine;

namespace CommonLayer.UserInterface.Views.StateMachines
{
    public abstract class UiStateMachineBase : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnEnter?.Invoke();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnExit?.Invoke();
        }

        public event Action OnEnter;
        public event Action OnExit;
    }
}