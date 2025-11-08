using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1f;
    }
    public void GoToLevel1()
    {
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1f;
    }
    public void GoToLevel2()
    {
        SceneManager.LoadScene("Level2");
        Time.timeScale = 1f;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
