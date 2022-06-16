using UnityEngine;
using UnityEngine.SceneManagement;

// The functions for main menu buttons.
public class MainMenu : MonoBehaviour
{
    public GameObject about;

    public GameObject unmuteButton;
    public GameObject muteButton;

    private void Start()
    {
        if (Time.timeScale != 1f)
        {
            Time.timeScale = 1f;
        }

        if (AudioListener.volume == 0f)
        {
            AudioListener.volume = 0f;
            unmuteButton.SetActive(true);
            muteButton.SetActive(false);
        } else {
            AudioListener.volume = 1f;
            muteButton.SetActive(true);
            unmuteButton.SetActive(false);
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

    public void About()
    {
        about.SetActive(true);
    }

    public void Close()
    {   
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
