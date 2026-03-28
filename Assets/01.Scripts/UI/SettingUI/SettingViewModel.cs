using System;
using Code.Core;
using Code.Core.Defines;

namespace Code.UI.Setting
{
    public class SettingViewModel
    {
        private static SettingViewModel _instance;
        public static SettingViewModel Instance => _instance ??= new SettingViewModel();

        private readonly SettingModel _model = new();

        public NotifyValue<bool> IsOpen { get; } = new();
        public NotifyValue<int> ScreenModeIndex { get; } = new();
        public NotifyValue<float> MasterVolume { get; } = new(ConstDefine.DefaultVolume);
        public NotifyValue<float> BGMVolume { get; } = new(ConstDefine.DefaultVolume);
        public NotifyValue<float> SFXVolume { get; } = new(ConstDefine.DefaultVolume);

        private SettingViewModel()
        {
        } // private 생성자 (다른 곳에서 생성 못하게)

        public void Toggle() => IsOpen.Value = !IsOpen.Value;

        public void Open() => IsOpen.Value = true;

        public void Close() => IsOpen.Value = false;

        public void ApplyScreenMode(int index)
        {
            try
            {
                _model.ApplyScreenMode(index);
                ScreenModeIndex.Value = index;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                UnityLogger.LogError(exception.Message);
            }
        }

        public void SetMasterVolume(float volume)
        {
            try
            {
                _model.SetMasterVolume(volume);
                MasterVolume.Value = volume;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                UnityLogger.LogError(exception.Message);
            }
        }

        public void SetMusicVolume(float volume)
        {
            try
            {
                _model.SetMusicVolume(volume);
                BGMVolume.Value = volume;
            }
            catch (ArgumentException exception)
            {
                UnityLogger.LogError(exception.Message);
            }
        }

        public void SetEffectVolume(float volume)
        {
            try
            {
                _model.SetEffectVolume(volume);
                SFXVolume.Value = volume;
            }
            catch (ArgumentOutOfRangeException exception)
            {
                UnityLogger.LogError(exception.Message);
            }
        }
    }
}