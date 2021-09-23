﻿namespace CommonLayer.UserInterface.Utility.Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class IgnoredTriggerBehaviour : TriggerBehaviour
        {
            public IgnoredTriggerBehaviour(TTrigger trigger, TransitionGuard transitionGuard)
                : base(trigger, transitionGuard) { }

            public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
            {
                destination = default;
                return false;
            }
        }
    }
}