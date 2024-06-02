using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Rendering;

public class LevelManager : MonoBehaviour
{
    public float timeUntilNextLevel = 60.0f; // час до наступного рівня
    public Text timerText; // текстове поле для виведення часу
    public Text ScoreText; // текстове поле для виведення очків
    [SerializeField] private int maXscore = 0; // загальна кількість очків
    public GameObject yourObject; // об'єкт, який ви хочете відобразити
    public GameObject secondObject; // об'єкт, який ви хочете відобразити

    public GameObject settingsPanel;
    public GameObject mainMenuPanel;
    public Slider volumeSFX;
    public AudioSource Music;
    public AudioSource[] Sound;
    public Slider volumeMusic;
    public Text Info_text;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        volumeSFX.value = PlayerPrefs.GetFloat("Sound");
        foreach (var sound in Sound)
        {
            sound.volume = volumeSFX.value;
        }

        volumeMusic.value = PlayerPrefs.GetFloat("Music");
        Music.volume = volumeMusic.value;
        StartCoroutine(Hideinfotext());

    }
    IEnumerator Hideinfotext()
    {
        yield return new WaitForSeconds(5);
        Info_text.text = "";

    }

    public bool isPaused = false;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && settingsPanel.activeSelf == false && mainMenuPanel.activeSelf == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            yourObject.SetActive(true);
            secondObject.GetComponent<CanvasGroup>().alpha = 0;
            // Time.timeScale = 0;
            isPaused = true;
        }

    }
    public void onContineButtonClick()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        yourObject.SetActive(false);
        secondObject.GetComponent<CanvasGroup>().alpha = 1;
        // Time.timeScale = 1;
        isPaused = false;
    }
    public void onExitButtonClick()
    {
        SceneManager.LoadSceneAsync(0);
        // Time.timeScale = 1;
    }
    public void onSettingsButtonClick()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void onExitSettingsButtonClick()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}