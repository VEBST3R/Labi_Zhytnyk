using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Button LoadButton;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private AudioSource Music;
    private void Start()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        if (File.Exists(saveFilePath))
        {
            LoadButton.interactable = true;
        }
        if (PlayerPrefs.HasKey("Sound"))
        {
            PlayerPrefs.SetFloat("Sound", volumeSlider.value);
            PlayerPrefs.Save();
        }
        if (PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", musicSlider.value);
            PlayerPrefs.Save();
            Music.volume = musicSlider.value;
        }
    }
    public void Lab_1Click()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void GameStartClick()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "save.json");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        SceneManager.LoadSceneAsync(2);
    }
    public void GameCountinueClick()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void SettingsClick()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
