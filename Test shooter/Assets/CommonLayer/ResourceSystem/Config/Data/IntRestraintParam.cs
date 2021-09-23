using System;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Config.Data
{
    [Serializable]
    public struct IntRestraintParam
    {
        [SerializeField] private int minValue;

        [SerializeField] private int maxValue;

        [SerializeField] private int value;

        public int MinValue => minValue;

        public int MaxValue => maxValue;

        public int Value => value;

        public override int GetHashCode()
        {
            return minValue.GetHashCode() + maxValue.GetHashCode() + value.GetHashCode();
        }
    }
}