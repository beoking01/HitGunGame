using UnityEngine;
using UnityEngine.SceneManagement;
public class EndOfLevel : MonoBehaviour
{
    public void LoadNextScene(string sceneName)
    {
        SaveGameFacade.SaveAllPersistentStates();
        SceneManager.LoadScene(sceneName);
    }
}