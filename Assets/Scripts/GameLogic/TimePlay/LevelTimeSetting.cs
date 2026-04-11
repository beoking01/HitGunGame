using UnityEngine;

public class LevelTimeSetting : MonoBehaviour
{
    [Header("Thiết lập khung giờ cho màn này")]
    public float startHour = 23f; // Ví dụ: 23 (11 PM)
    public float endHour = 8f;    // Ví dụ: 8 (8 AM)
    public float timeSpeed = 0.5f;

    // Hàm này giúp Manager dễ dàng lấy cấu hình của màn hiện tại
    public static LevelTimeSetting Instance;

    private void Awake()
    {
        Instance = this;
    }
}