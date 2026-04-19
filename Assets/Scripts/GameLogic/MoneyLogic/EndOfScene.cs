using UnityEngine;
using UnityEngine.SceneManagement;
public class EndOfLevel : MonoBehaviour
{
    public void LoadNextScene(string sceneName)
    {
        SaveGameFacade.SaveAllPersistentStates();
        LoadingScene.SceneToLoad = sceneName;
        SceneManager.LoadScene("Loading");
        GameManager.Instance.ContinueGame();
    }
}