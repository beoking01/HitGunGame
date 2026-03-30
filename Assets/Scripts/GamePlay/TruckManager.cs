using UnityEngine;
using UnityEngine.SceneManagement;

public class TruckManager : MonoBehaviour
{
    public static TruckManager Instance;

    private void Awake()
    {
        // 1. Logic Singleton cho Xe tải
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null); // Đảm bảo là Root để DontDestroyOnLoad hoạt động
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // Nếu đã có một chiếc xe "xịn" đi từ Scene trước sang, 
            // thì chiếc xe "có sẵn" trong Scene hiện tại phải biến mất.
            Destroy(gameObject); 
            return; // Thoát hàm để không chạy logic bên dưới
        }
    }
    private void OnEnable()
    {
        // Đăng ký sự kiện: Mỗi khi một Scene được load xong, hàm OnSceneLoaded sẽ chạy
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Hủy đăng ký để tránh lỗi bộ nhớ
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 1. Tìm điểm đỗ xe trong Scene mới
        GameObject parkingSlot = GameObject.FindWithTag("ParkingSlot");

        if (parkingSlot != null)
        {
            // 2. "Hút" xe về vị trí và góc quay của điểm đỗ
            transform.position = parkingSlot.transform.position;
            transform.rotation = parkingSlot.transform.rotation;
            
            Debug.Log("Xe tải đã vào bãi đỗ tại: " + scene.name);
        }
        else
        {
            Debug.LogWarning("Không tìm thấy điểm đỗ xe (ParkingSlot) trong Scene này!");
        }
    }
}