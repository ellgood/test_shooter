using System;

namespace CommonLayer.UserInterface.Utility.ExecutionOrder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExecuteBeforeAttribute : Attribute
    {
        public readonly Type TargetType;
        public readonly int OrderDecrease;

        public ExecuteBeforeAttribute(Type targetType)
        {
            TargetType = targetType;
            OrderDecrease = 10;
        }
    }
}