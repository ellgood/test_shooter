using System;

namespace CommonLayer.UserInterface.Utility.ExecutionOrder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExecutionOrderAttribute : Attribute
    {
        public readonly int Order;

        public ExecutionOrderAttribute(int order)
        {
            Order = order;
        }
    }
}