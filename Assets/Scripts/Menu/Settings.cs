using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider musicSlider;
    public Toggle lowQualityToggle;
    public Toggle mediumQualityToggle;
    public Toggle highQualityToggle;
    public Button backButton;
    public AudioSource Music;
    public AudioSource[] Sound;
    public GameObject settingsPanel;
    public GameObject mainMenuPanel;

    private void Start()
    {
        // Додаємо обробники подій для слайдерів, перемикачів і кнопки
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        lowQualityToggle.onValueChanged.AddListener(isOn => OnQualityChanged(0, isOn));
        mediumQualityToggle.onValueChanged.AddListener(isOn => OnQualityChanged(1, isOn));
        highQualityToggle.onValueChanged.AddListener(isOn => OnQualityChanged(2, isOn));

        // int musvol = PlayerPrefs.GetInt("Music");
        // musicSlider.value = musvol;
        // int soundvol = PlayerPrefs.GetInt("Sound");
        // volumeSlider.value = soundvol;
    }

    private void OnVolumeChanged(float volume)
    {
        foreach (var sound in Sound)
        {
            sound.volume = volume;
        }
        PlayerPrefs.SetFloat("Sound", volume);
        PlayerPrefs.Save();
    }

    private void OnMusicChanged(float volume)
    {
        // Перетворюємо значення слайдера (0-1) на децибели (-80-0)
        Music.volume = volume;
        PlayerPrefs.SetFloat("Music", volume);
        PlayerPrefs.Save();
    }

    private void OnQualityChanged(int qualityLevel, bool isOn)
    {
        // Змінюємо якість графіки
        if (isOn)
        {
            QualitySettings.SetQualityLevel(qualityLevel);
            PlayerPrefs.SetInt("Quality", qualityLevel);
            PlayerPrefs.Save();
        }
    }

}

