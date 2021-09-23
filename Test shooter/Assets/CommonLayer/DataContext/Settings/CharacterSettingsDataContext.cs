using System;
using System.Collections;
using CommonLayer.ResourceSystem.Config.Interfaces;
using CommonLayer.ResourceSystem.Impl;
using CommonLayer.ResourceSystem.Interface;
using CommonLayer.SaveLoadSystem;


namespace CommonLayer.DataContext.Settings
{
    public class CharacterSettingsDataContext : DataContextBase, ICharacterSettingsDataContext
    {
        private const string CharacterSaveKey = "CharacterSettings";
        private readonly IResourcesController _resourcesController;

        private readonly ISaveLoadManager _saveLoadManager;

        public ICharacterConfig Config { get; private set; }

        private CharacterSettings _settingsData;

        private bool _isDirty;

        public CharacterSettingsDataContext(ISaveLoadManager saveLoadManager, IResourcesController resourcesController)
        {
            _saveLoadManager = saveLoadManager;
            _resourcesController = resourcesController;
        }

        public event Action<CharacterSettingsType> SettingsChanged;
        public float VerticalRestraint => Config.VerticalRestraint;
        public bool CursorVisible => Config.CursorVisible;
        public float Sensitivity => _settingsData.Sensitivity;
        public float Speed => _settingsData.Speed;
        public float MovementSmoothFactor => Config.MovementSmoothFactor;
        public float Gravity => Config.Gravity;
        public float StepOffSet => Config.StepOffSet;
        public float GroundedVelocityY => Config.GroundedVelocityY;
        public float JumpHeight => Config.JumpHeight;
        public float SitPointOffsetY => Config.SitPointOffsetY;
        public float SitSpeedFactor => Config.SitSpeedFactor;
        public bool VerticalInverted => _settingsData.VerticalInverted;
        public float LookSmoothFactor => Config.LookSmoothFactor;

        public void SetSensitivity(float value)
        {
            _settingsData.SetSensitivity(value);
            _isDirty = true;
            SettingsChanged?.Invoke(CharacterSettingsType.Sensitivity);
        }

        public void SetSpeed(float value)
        {
            _settingsData.SetSpeed(value);
            _isDirty = true;
            SettingsChanged?.Invoke(CharacterSettingsType.Speed);
        }

        public void SetVerticalInverted(bool value)
        {
            _settingsData.SetVerticalInverted(value);
            _isDirty = true;
            SettingsChanged?.Invoke(CharacterSettingsType.InvertedVertical);
        }

        public void SaveSettings()
        {
            if (!_isDirty)
            {
                return;
            }
            _saveLoadManager.Save(CharacterSaveKey, _settingsData);
            _isDirty = false;
        }

        public void LoadSettings()
        {
            _settingsData = _saveLoadManager.TryLoad(CharacterSaveKey, out CharacterSettings data)
                ? data
                : null;
        }

        protected override IEnumerator OnInitializeProcess()
        {
            Config = _resourcesController.GetData<CharacterResourcesData>().Content.Config;
            LoadSettings();
            CheckValidSettings();
            yield break;
        }

        private void CheckValidSettings()
        {
            if (_settingsData != null && _settingsData.Hash == Config.GetHash()) return;
            _settingsData = new CharacterSettings(Config);
        }

        protected override void OnReset()
        {
            SaveSettings();
        }
    }
}