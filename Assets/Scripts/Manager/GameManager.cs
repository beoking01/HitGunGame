using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject gameOverUI;
    public GameObject pauseMenuUI;
    // public GameObject gameWinUI;
    // public GameObject winUI;
    private bool gamePlay = true;
    // private float conditionWin = 10;
    // private PlayerInventory playerInventory;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
        DontDestroyOnLoad(gameObject); // giữ lại qua scene
    }
    public void Start()
    {
        SetCursorGameplay(true);
    }
    public void GameOver()
    {
        SetGameplay(false);
        if (TruckStateManager.Instance != null)
        {
            TruckStateManager.Instance.State.Clear();
        }
        gameOverUI.SetActive(true);
    }
    public bool isGamePlay()
    {
        return gamePlay;
    }
    // public void WinGame()
    // {
    //     gamePlay = false;
    //     Time.timeScale = 0f;
    // }
    public void ContinueGame()
    {
        SetGameplay(true);
        SaveGameFacade.SaveAllPersistentStates();
    }

    private void SetCursorGameplay(bool gameplay)
    {
        Cursor.lockState = gameplay ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !gameplay;
    }
    public void SetGameplay(bool isPlaying)
    {
        gamePlay = isPlaying;
        Time.timeScale = isPlaying ? 1f : 0f;
        SetCursorGameplay(isPlaying);
    }
    public void TogglePauseMenu()
    {
        if (pauseMenuUI != null)
        {
            bool isActive = pauseMenuUI.activeSelf;
            pauseMenuUI.SetActive(!isActive);
            SetGameplay(isActive); // Nếu đang active thì chuyển sang gameplay, ngược lại thì pause
        }
    }
    public void ContinueFromPause()
    {
        if (pauseMenuUI != null && pauseMenuUI.activeSelf)
        {
            pauseMenuUI.SetActive(false);
            SetGameplay(true);
        }
    }
}
