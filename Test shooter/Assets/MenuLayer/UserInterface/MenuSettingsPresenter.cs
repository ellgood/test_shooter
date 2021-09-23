using CommonLayer.DataContext.Settings;
using CommonLayer.UserInterface.Presenter.Settings;


namespace MenuLayer.UserInterface
{
    public sealed class MenuSettingsPresenter : SettingsPresenterBase
    {
        public MenuSettingsPresenter(ICharacterSettingsDataContext characterSettingsCtx) : base(characterSettingsCtx)
        {
        }

        public override string ViewKey => "menu_settings_view";

        protected override void OnMenuButton()
        {
            Route.RouteHide();
        }
    }
}