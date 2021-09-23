using System;
using CommonLayer.UserInterface.Views;
using UnityEngine;

namespace CommonLayer.UserInterface.Binder.Collection
{
    public sealed class CollectionBinder : CollectionBinderBase { }

    public struct IdxRange : IEquatable<IdxRange>
    {
        public IdxRange(int idx)
        {
            From = idx;
            To = idx;
        }

        public IdxRange(int from, int to)
        {
            From = from;
            To = to;
        }

        public int From { get; }

        public int To { get; }

        public int Len => To - From;

        #region IEquatable<IdxRange> Implementation

        public bool Equals(IdxRange other)
        {
            return From == other.From && To == other.To;
        }

        #endregion

        public override string ToString()
        {
            return $"[{From}:{To}]";
        }

        public bool Contains(int value)
        {
            return value >= From && value <= To;
        }

        public static IdxRange Clamp(IdxRange range, int min, int max)
        {
            return new IdxRange(Mathf.Max(min, range.From), Mathf.Min(max, range.To));
        }

        public override bool Equals(object obj)
        {
            return obj is IdxRange other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (From * 397) ^ To;
            }
        }
    }

    public struct ViewWithIdx
    {
        public ViewWithIdx(int idx, ViewElement view)
        {
            Idx = idx;
            View = view;
        }

        public int Idx { get; }

        public ViewElement View { get; }
    }
}