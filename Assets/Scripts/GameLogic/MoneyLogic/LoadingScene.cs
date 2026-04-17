using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private PannelFieldList pannelFieldList;

    // Biến tĩnh để truyền tên Scene cần nạp từ Scene A sang
    public static string SceneToLoad; 

    private void Start()
    {
        pannelFieldList.randomPanel();
        if (!string.IsNullOrEmpty(SceneToLoad))
        {
            StartCoroutine(LoadAsyncOperation());
        }
    }

    IEnumerator LoadAsyncOperation()
    {
        // Bắt đầu nạp Scene mục tiêu ở luồng phụ
        AsyncOperation gameLevel = SceneManager.LoadSceneAsync(SceneToLoad);

        // Ngăn không cho Scene mới kích hoạt ngay lập tức để người chơi kịp nhìn màn hình loading
        gameLevel.allowSceneActivation = false;

        while (!gameLevel.isDone)
        {
            // Unity nạp đến 0.9 là xong dữ liệu
            float progress = Mathf.Clamp01(gameLevel.progress / 0.9f);
            progressBar.value = progress;
            loadingText.text = $"Loading... {(progress * 100):n0}%";

            // Khi nạp xong dữ liệu (0.9)
            if (gameLevel.progress >= 0.9f)
            {
                loadingText.text = "Press any key to continue...";
                if (Input.anyKey) 
                {
                    gameLevel.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}