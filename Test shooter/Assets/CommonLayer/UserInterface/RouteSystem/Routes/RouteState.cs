namespace CommonLayer.UserInterface.RouteSystem.Routes
{
    public enum RouteState
    {
        Inactive,
        Opened,
        
        Visible,
        
        VisibleShow,
        Showing,
        Shown,

        VisibleHide,
        Hiding,
        Hidden,

        ParentHiding,
        ParentHidden,

        Closing,
        Disposed
    }
}