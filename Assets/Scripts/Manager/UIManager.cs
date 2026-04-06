using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI pointsText;

    
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
        SaveGameFacade.SaveAllPersistentStates();
        SceneManager.LoadScene("Menu");
    }
}
