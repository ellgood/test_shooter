using UnityEngine;
using UnityEngine.Assertions;

namespace GameLayer.GameCameraSystem
{
    public sealed class GameCameraHolder : MonoBehaviour, IGameCameraHolder
    {
        [SerializeField] 
        private Camera gameCamera;
        
        private Transform _cameraTransform;
        
        private void Awake()
        {
            Assert.IsNotNull(gameCamera);
            _cameraTransform = gameCamera.transform;
        }


        public void ResetParent()
        {
            SetParent(transform);
        }

        public void SetParent(Transform parent)
        {
            if (!_cameraTransform)
            {
                return;
            }
            _cameraTransform.SetParent(parent);
            _cameraTransform.localPosition = Vector3.zero;
            _cameraTransform.localRotation = Quaternion.identity;
            _cameraTransform.localScale = Vector3.one;
        }
    }
}