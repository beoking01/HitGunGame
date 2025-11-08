using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject gameOverUI;
    public GameObject gameWinUI;
    // public GameObject winUI;
    private bool gamePlay = false;
    private float conditionWin = 10;
    private PlayerInventory playerInventory;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        DontDestroyOnLoad(gameObject); // giữ lại qua scene
    }
    public void GameOver()
    {
        gamePlay = true;
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
    }
    public bool isGamePlay()
    {
        return gamePlay;
    }
    public void WinGame()
    {
        gamePlay = true;
        Time.timeScale = 0f;
        gameWinUI.SetActive(true);
    }
    public void CheckConditionWin()
    {
        if (playerInventory.showMoney() >= conditionWin)
        {
            WinGame();
        }
    }
}
