﻿using CommonLayer.UserInterface.Utility.Stateless.Reflection;

namespace CommonLayer.UserInterface.Utility.Stateless.Graph
{
    /// <summary>
    ///     Used to keep track of the decision point of a dynamic transition
    /// </summary>
    public class Decision : State
    {
        /// <summary>
        ///     Creates a new instance of <see cref="Decision" />.
        /// </summary>
        /// <param name="method">The name of the invoked method.</param>
        /// <param name="num">The decision count.</param>
        public Decision(InvocationInfo method, int num)
            : base("Decision" + num)
        {
            Method = method;
        }

        /// <summary>
        ///     Gets the underlying method description of the invoked method.
        /// </summary>
        public InvocationInfo Method { get; }
    }
}