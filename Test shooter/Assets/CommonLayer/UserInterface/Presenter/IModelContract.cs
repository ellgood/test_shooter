namespace CommonLayer.UserInterface.Presenter
{
    public interface IModelContract<in TContract>
    {
        void UseModel(TContract model);
    }
}