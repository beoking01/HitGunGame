using System;
using UnityEngine;

public class FastTest : MonoBehaviour
{
    public void LoadScene(String sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}