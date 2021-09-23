using System.Text;

namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    public static class PresenterRouteUtils
    {
        private static readonly StringBuilder StringBuilder = new StringBuilder();

        public static StringBuilder DebugStringBuilder
        {
            get
            {
                StringBuilder.Clear();
                return StringBuilder;
            }
        }
    }
}