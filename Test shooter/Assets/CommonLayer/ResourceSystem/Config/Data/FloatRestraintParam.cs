using System;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Config.Data
{
    [Serializable]
    public struct FloatRestraintParam
    {
        [SerializeField] private float minValue;

        [SerializeField] private float maxValue;

        [SerializeField] private float value;

        public float MinValue => minValue;

        public float MaxValue => maxValue;

        public float Value => value;

        public int GetHash()
        {
            return minValue.GetHashCode() + maxValue.GetHashCode() + value.GetHashCode();
        }
    }
}