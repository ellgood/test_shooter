using System;

namespace CommonLayer.SceneControllers.Routines
{
    public readonly struct TaskStatus : IEquatable<TaskStatus>
    {
        public static TaskStatus Continue = new TaskStatus(1);
        public static TaskStatus TaskFailed = new TaskStatus(byte.MaxValue);

        public TaskStatus(byte value)
        {
            Value = value;
        }

        public byte Value { get; }

        #region IEquatable<TaskStatus> Implementation

        public bool Equals(TaskStatus other)
        {
            return Value == other.Value;
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is TaskStatus other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return $"{nameof(Value)}: {Value}";
        }
    }
}