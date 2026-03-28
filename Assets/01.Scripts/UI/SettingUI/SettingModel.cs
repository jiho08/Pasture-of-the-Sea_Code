using System;
using Code.Core;
using Code.Core.Defines;
using Hellmade.Sound;
using UnityEngine;

namespace Code.UI.Setting
{
    public class SettingModel
    {
        public void ApplyScreenMode(int screenIndex)
        {
            Screen.fullScreen = screenIndex switch
            {
                0 => true,
                1 => false,
                _ => throw new ArgumentOutOfRangeException(nameof(screenIndex),
                    "<color=red>화면 설정 값은 0 ~ 1 사이여야 합니다.</color>")
            };

            PlayerPrefs.SetInt(ConstDefine.ScreenModePrefsKey, screenIndex);
        }

        public void SetMasterVolume(float volume)
        {
            if (volume is < ConstDefine.MinVolume or > ConstDefine.MaxVolume) // volume < 0f || volume > 100f
                throw new ArgumentOutOfRangeException(nameof(volume),
                    $"<color=red>볼륨 값은 0 ~ 100 사이여야 합니다. 값 : {volume}</color>");
            
            UnityLogger.Log("전체음 설정");
            //EazySoundManager.GlobalVolume = volume;
            PlayerPrefs.SetFloat(ConstDefine.MasterVolumePrefsKey, volume);
        }

        public void SetMusicVolume(float volume)
        {
            if (volume is < ConstDefine.MinVolume or > ConstDefine.MaxVolume) // volume < 0f || volume > 100f
                throw new ArgumentOutOfRangeException(nameof(volume),
                    $"<color=red>볼륨 값은 0 ~ 100 사이여야 합니다. 값 : {volume}</color>");
            
            UnityLogger.Log("배경음 설정");
            EazySoundManager.GlobalMusicVolume = volume;
            PlayerPrefs.SetFloat(ConstDefine.MusicVolumePrefsKey, volume);
        }

        public void SetEffectVolume(float volume)
        {
            if (volume is < ConstDefine.MinVolume or > ConstDefine.MaxVolume) // volume < 0f || volume > 100f
                throw new ArgumentOutOfRangeException(nameof(volume),
                    $"<color=red>볼륨 값은 0 ~ 100 사이여야 합니다. 값 : {volume}</color>");
            
            UnityLogger.Log("효과음 설정");
            EazySoundManager.GlobalSoundsVolume = volume;
            EazySoundManager.GlobalUISoundsVolume = volume;
            PlayerPrefs.SetFloat(ConstDefine.EffectVolumePrefsKey, volume);
        }
    }
}