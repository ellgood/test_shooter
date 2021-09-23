namespace GameLayer.WeaponSystem
{
    public interface IWeaponSwitch
    {
        void SelectNext();
        void SelectPrevious();
        void SelectByIdx(int idx);
    }
}