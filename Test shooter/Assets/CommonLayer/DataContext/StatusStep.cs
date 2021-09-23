namespace CommonLayer.DataContext
{
    public enum StatusStep : byte
    {
        Start = 0,
        Process = 1,
        Complete = 2,
        Connecting = 3,
        Connected = 4
    }
}