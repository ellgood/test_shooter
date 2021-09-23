using System;
using CommonLayer.UserInterface.Utility.Stateless.Reflection;

namespace CommonLayer.UserInterface.Utility.Stateless
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class DynamicTriggerBehaviour : TriggerBehaviour
        {
            private readonly Func<object[], TState> _destination;

            public DynamicTriggerBehaviour(
                TTrigger trigger,
                Func<object[], TState> destination,
                TransitionGuard transitionGuard,
                DynamicTransitionInfo info)
                : base(trigger, transitionGuard)
            {
                _destination = destination ?? throw new ArgumentNullException(nameof(destination));
                TransitionInfo = info ?? throw new ArgumentNullException(nameof(info));
            }

            internal DynamicTransitionInfo TransitionInfo { get; }

            public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
            {
                destination = _destination(args);
                return true;
            }
        }
    }
}