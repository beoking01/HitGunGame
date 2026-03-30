using UnityEngine;

public class Fasttest : MonoBehaviour 
{
    public void Loadsencene(string scenename)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scenename);
    }
}