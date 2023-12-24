using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMPro.TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider sliderMusicVolume;
    [SerializeField] private Slider sliderSoundsVolume;

    private RefreshRate currentRefreshRate;
    private int currentResolutionIndex;
    private List<Resolution> filteredResolutions;

    private bool isInitialized = false;

    // Strings for PlayerPrefs
    private readonly string pp_musicVolume = "MusicVolume";
    private readonly string pp_soundVolume = "SoundVolume";

    private void Start()
    {
        // Default values of settings
        float musicVolume = 1f;
        float soundVolume = 1f;

        // Get player values, if he change them
        if (PlayerPrefs.HasKey(pp_musicVolume))
        {
            musicVolume = PlayerPrefs.GetFloat(pp_musicVolume);
        }
        if (PlayerPrefs.HasKey(pp_soundVolume))
        {
            soundVolume = PlayerPrefs.GetFloat(pp_soundVolume);
        }

        Debug.Log($"Settings: Start: musicVolume={musicVolume} soundVolume={soundVolume}");
        SetMusicVolume(musicVolume);
        sliderMusicVolume.value = musicVolume;
        SetSoundsVolume(soundVolume);
        sliderSoundsVolume.value = soundVolume;

        Resolution[] resolutions = Screen.resolutions;
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        filteredResolutions = new List<Resolution>();

        // Цикл по всем возможным разрешениям экрана. Отбираем те, что подходят по герцовке
        for (int i = 0; i < resolutions.Length; i++)
        {
            Debug.Log($"{resolutions[i].width}x{resolutions[i].height} {resolutions[i].refreshRateRatio.value} Hz");
            //if (resolutions[i].refreshRateRatio.numerator == currentRefreshRate.numerator && resolutions[i].refreshRateRatio.denominator == currentRefreshRate.denominator)
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate.value)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        // Отображаем filteredResolutions в Dropdown
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string optionText = string.Concat(filteredResolutions[i].width, "x", filteredResolutions[i].height);
            options.Add(optionText);

            if (filteredResolutions[i].width == Screen.currentResolution.width && filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        isInitialized = true;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution res = filteredResolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

        if (isInitialized)
        {
            PlayerPrefs.SetFloat(pp_musicVolume, volume);
        }
    }

    public void SetSoundsVolume(float volume)
    {
        audioMixer.SetFloat("SoundsVolume", Mathf.Log10(volume) * 20);

        if (isInitialized)
        {
            PlayerPrefs.SetFloat(pp_soundVolume, volume);
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
