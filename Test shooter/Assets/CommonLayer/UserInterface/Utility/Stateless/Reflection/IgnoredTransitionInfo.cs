﻿using System.Collections.Generic;
using System.Linq;

namespace CommonLayer.UserInterface.Utility.Stateless.Reflection
{
    /// <summary>
    ///     Describes a trigger that is "ignored" (stays in the same state)
    /// </summary>
    public class IgnoredTransitionInfo : TransitionInfo
    {
        private IgnoredTransitionInfo() { }

        internal static IgnoredTransitionInfo Create<TState, TTrigger>(StateMachine<TState, TTrigger>.IgnoredTriggerBehaviour behaviour)
        {
            var transition = new IgnoredTransitionInfo
            {
                Trigger = new TriggerInfo(behaviour.Trigger),
                GuardConditionsMethodDescriptions = behaviour.Guard == null
                    ? new List<InvocationInfo>()
                    : behaviour.Guard.Conditions.Select(c => c.MethodDescription)
            };

            return transition;
        }
    }
}