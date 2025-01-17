﻿namespace CommonLayer.UserInterface.Utility.Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class TransitioningTriggerBehaviour : TriggerBehaviour
        {
            // transitionGuard can be null if there is no guard function on the transition
            public TransitioningTriggerBehaviour(TTrigger trigger, TState destination, TransitionGuard transitionGuard)
                : base(trigger, transitionGuard)
            {
                Destination = destination;
            }

            internal TState Destination { get; }

            public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
            {
                destination = Destination;
                return true;
            }
        }
    }
}