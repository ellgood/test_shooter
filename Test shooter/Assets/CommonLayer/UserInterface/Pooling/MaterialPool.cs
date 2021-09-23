using UnityEngine;

namespace CommonLayer.UserInterface.Pooling
{
    public sealed class MaterialPool : UnityObjectPoolBase<Material>
    {
        public MaterialPool(Material shared) : base(shared)
        { 
            
        }
        protected override void OnBeforeRent(Material instance)
        {
            
        }

        protected override void OnAfterReturn(Material instance)
        {
            
        }
    }
}