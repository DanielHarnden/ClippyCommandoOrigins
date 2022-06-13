using UnityEngine;
using UnityEngine.SceneManagement;

// The functions for main menu buttons.
public class MainMenu : MonoBehaviour
{
    public GameObject about;
    public GameObject settings;

    public GameObject unmuteButton;
    public GameObject muteButton;

    private void Start()
    {
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }
    }

    public void StoryMode()
    {
        SceneManager.LoadScene("StoryMode");
    }

    public void InfiniteMode()
    {
        SceneManager.LoadScene("InfiniteMode");
    }

    public void Settings()
    {
        settings.SetActive(true);
    }

    public void About()
    {
        about.SetActive(true);
    }

    public void Close()
    {   
        settings.SetActive(false);
        about.SetActive(false);
    }

    public void Mute()
    {
        AudioListener.volume = 0f;
        unmuteButton.SetActive(true);
        muteButton.SetActive(false);
    }

    public void Unmute()
    {
        AudioListener.volume = 1f;
        muteButton.SetActive(true);
        unmuteButton.SetActive(false);
    }
}
