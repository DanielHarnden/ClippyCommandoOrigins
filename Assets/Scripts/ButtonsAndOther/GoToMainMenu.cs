using UnityEngine;
using UnityEngine.SceneManagement;

// This is so self explanatory that this comment is just here as a joke. If you don't know what this does, please go read some Unity documentation.
public class GoToMainMenu : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}