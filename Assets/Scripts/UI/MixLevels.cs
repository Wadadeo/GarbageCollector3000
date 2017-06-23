using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MixLevels : MonoBehaviour
{
    private static string masterVolumeParameter = "MasterVolume";
    private static string fxVolumeParameter = "FXVolume";
    private static string musicVolumeParameter = "MusicVolume";

    [Header("Mixer")]
    public AudioMixer audioMixer;
    [Header("UI Elements")]
    public Slider masterSlider;
    public Slider fxSlider;
    public Slider musicSlider;

    public void InitVolumes()
    {
        float masterVolume = PlayerPrefs.GetFloat(masterVolumeParameter, 0f);
        float fxVolume = PlayerPrefs.GetFloat(fxVolumeParameter, 0f);
        float musicVolume = PlayerPrefs.GetFloat(musicVolumeParameter, 0f);

        SetMasterVolume(masterVolume);
        SetFXVolume(fxVolume);
        SetMusicVolume(musicVolume);
        masterSlider.value = masterVolume;
        fxSlider.value = fxVolume;
        musicSlider.value = musicVolume;
    }

    public void SetMasterVolume(float lvl)
    {
        PlayerPrefs.SetFloat(masterVolumeParameter, lvl);
        audioMixer.SetFloat(masterVolumeParameter, lvl);
    }

    public void SetFXVolume(float lvl)
    {
        PlayerPrefs.SetFloat(fxVolumeParameter, lvl);
        audioMixer.SetFloat(fxVolumeParameter, lvl);
    }

    public void SetMusicVolume(float lvl)
    {
        PlayerPrefs.SetFloat(musicVolumeParameter, lvl);
        audioMixer.SetFloat(musicVolumeParameter, lvl);
    }

}
