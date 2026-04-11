using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    private float currentTime;
    private LevelTimeSetting currentSettings;
    private bool isTimeStopped = false;

    void Start()
    {
        currentSettings = LevelTimeSetting.Instance;
        if (currentSettings != null)
        {
            currentTime = currentSettings.startHour;
            isTimeStopped = false;
        }
    }

    void Update()
    {
        if (currentSettings == null) return;

        if (!isTimeStopped)
        {
            // Sử dụng Mathf.MoveTowards để tiến dần về đích mà không vượt quá
            float nextTime = currentTime + Time.deltaTime * currentSettings.timeSpeed;
            
            // Xử lý bước nhảy qua 24h (Ví dụ từ 23h sáng 0h)
            if (nextTime >= 24f) nextTime -= 24f;

            currentTime = nextTime;

            // Kiểm tra xem đã chạm mốc endHour chưa (dùng sai số nhỏ 0.01 để so sánh)
            if (Mathf.Abs(currentTime - currentSettings.endHour) < 0.1f)
            {
                currentTime = currentSettings.endHour;
                isTimeStopped = true;
            }
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        int hours = Mathf.FloorToInt(currentTime);
        int minutes = Mathf.FloorToInt((currentTime - hours) * 60);
        
        string suffix = hours >= 12 ? "PM" : "AM";
        int displayHour = hours % 12;
        if (displayHour == 0) displayHour = 12;

        timeText.text = string.Format("{0:00}:{1:00} {2}", displayHour, minutes, suffix);

        // Xử lý hiệu ứng nhấp nháy đỏ khi dừng
        if (isTimeStopped)
        {
            // Sử dụng hàm sin hoặc PingPong để tạo hiệu ứng nhấp nháy mượt mà
            float alpha = Mathf.PingPong(Time.time * 2f, 1f); 
            timeText.color = Color.Lerp(Color.white, Color.red, alpha);
        }
        else
        {
            timeText.color = Color.white;
        }
    }
}