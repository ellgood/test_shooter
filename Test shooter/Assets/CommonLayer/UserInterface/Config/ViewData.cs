using UnityEngine;

namespace CommonLayer.UserInterface.Config
{
    [CreateAssetMenu(fileName = "UiViewsDataBase", menuName = "DataBases/Ui")]
    public sealed class ViewData : ScriptableObject
    {
        [SerializeField] 
        private ViewInfo[] _views;

        public ViewInfo[] Views => _views;

        public bool TryGetValue(string viewKey, out ViewInfo viewInfo)
        {
            if (_views == null || _views.Length <= 0)
            {
                viewInfo = default;
                return false;
            }

            foreach (var view in _views)
            {
                if (view.ViewKey != viewKey)
                {
                    continue;
                }

                viewInfo = view;
                return true;
            }
            viewInfo = default;
            return false;
        }
    }
}