using System;
using UnityEngine;

public class FastTest : MonoBehaviour
{
    public void LoadScene(String sceneName)
    {
        SaveGameFacade.SaveAllPersistentStates();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}