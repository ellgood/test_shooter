using System;

namespace CommonLayer.UserInterface.Utility.ExecutionOrder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ExecuteAfterAttribute : Attribute
    {
        public readonly Type TargetType;
        public readonly int OrderIncrease;

        public ExecuteAfterAttribute(Type targetType)
        {
            TargetType = targetType;
            OrderIncrease = 10;
        }
    }
}