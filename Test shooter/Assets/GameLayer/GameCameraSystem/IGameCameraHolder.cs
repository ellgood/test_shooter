using UnityEngine;

namespace GameLayer.GameCameraSystem
{
    public interface IGameCameraHolder
    {
        void SetParent(Transform parent);
        void ResetParent();
    }
}