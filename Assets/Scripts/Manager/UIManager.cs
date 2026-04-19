using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public void Quit()
    {
        SaveGameFacade.SaveAllPersistentStates();
        Application.Quit();
    }
    public void PlayAgain()
    {
        SaveGameFacade.SaveAllPersistentStates();
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1f;
    }
    public void GoToLevel1()
    {
        SaveGameFacade.SaveAllPersistentStates();
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1f;
    }
    public void GoToLevel2()
    {
        SaveGameFacade.SaveAllPersistentStates();
        SceneManager.LoadScene("Level2");
        Time.timeScale = 1f;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void startBase()
    {
        string sceneName = "BaseCentral";
        LoadingScene.SceneToLoad = sceneName;
        SceneManager.LoadScene("Loading");
    }
}
