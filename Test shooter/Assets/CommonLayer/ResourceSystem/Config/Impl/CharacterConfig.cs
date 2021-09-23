using System;
using CommonLayer.ResourceSystem.Config.Data;
using CommonLayer.ResourceSystem.Config.Interfaces;
using UnityEngine;

namespace CommonLayer.ResourceSystem.Config.Impl
{
    [Serializable]
    public sealed class CharacterConfig : ConfigBase, ICharacterConfig
    {
        [SerializeField] private bool cursorVisible;

        [SerializeField] private FloatRestraintParam sensitivity;

        [SerializeField] private bool verticalInverted;

        [SerializeField] private float verticalRestraint;

        [SerializeField] private float lookSmoothFactor;

        [SerializeField] private FloatRestraintParam speed;

        [SerializeField] private float sitSpeedFactor;

        [SerializeField] private float gravity;

        [SerializeField] private float movementSmoothFactor;

        [SerializeField] private float jumpHeight;

        [SerializeField] private float groundedVelocityY;

        [SerializeField] private float sitPointOffsetY;

        [SerializeField] private float stepOffSet;

        public bool CursorVisible => cursorVisible;

        public FloatRestraintParam Sensitivity => sensitivity;

        public bool VerticalInverted => verticalInverted;

        public float VerticalRestraint => verticalRestraint;

        public float LookSmoothFactor => lookSmoothFactor;

        public FloatRestraintParam Speed => speed;

        public float SitSpeedFactor => sitSpeedFactor;

        public float Gravity => gravity;

        public float MovementSmoothFactor => movementSmoothFactor;

        public float JumpHeight => jumpHeight;

        public float GroundedVelocityY => groundedVelocityY;

        public float SitPointOffsetY => sitPointOffsetY;

        public float StepOffSet => stepOffSet;

        public override int GetHash()
        {
            return cursorVisible.GetHashCode() +
                   sensitivity.GetHash() +
                   verticalInverted.GetHashCode() +
                   verticalRestraint.GetHashCode() +
                   lookSmoothFactor.GetHashCode() +
                   speed.GetHash() +
                   sitSpeedFactor.GetHashCode() +
                   gravity.GetHashCode() +
                   movementSmoothFactor.GetHashCode() +
                   jumpHeight.GetHashCode() +
                   groundedVelocityY.GetHashCode() +
                   sitPointOffsetY.GetHashCode() +
                   stepOffSet.GetHashCode();
        }
    }
}