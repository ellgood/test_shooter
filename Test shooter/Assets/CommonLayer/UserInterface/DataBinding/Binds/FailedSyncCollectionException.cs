namespace CommonLayer.UserInterface.DataBinding.Binds
{
    public class FailedSyncCollectionException : BindContextException
    {
        public FailedSyncCollectionException() { }

        public FailedSyncCollectionException(string msg) : base(msg) { }
    }
}