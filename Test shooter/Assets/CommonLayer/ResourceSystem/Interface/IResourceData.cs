namespace CommonLayer.ResourceSystem.Interface
{
    public interface IResourceData<out TContent> : IResourceData
        where TContent : class,new()
    {
        TContent Content { get; }
    }
    
    public interface IResourceData
    {
        
    }
}