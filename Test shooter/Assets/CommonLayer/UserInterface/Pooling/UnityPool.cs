using UnityEngine;

namespace CommonLayer.UserInterface.Pooling
{
    public class UnityPool : UnityComponentPool<Component>
    {
        public UnityPool(GameObject original, Transform rentRoot = null, Transform poolRoot = null) : base(original.transform, rentRoot, poolRoot)
        {
        }

        public GameObject Rent(string goName)
        {
            GameObject go = Rent();
            go.name = goName;
            return go;
        }

        public new GameObject Rent()
        {
            GameObject rent = base.Rent().gameObject;
            return rent;
        }

        public void Return(GameObject instance)
        {
            base.Return(instance.transform);
        }
    }
}