using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    [SerializeField] private Transform agreeGrationPoint;
    private float currentTime;
    private LevelTimeSetting currentSettings;
    private bool isTimeStopped = false;
    private bool hasSentTimeExpiredNotice = false;

    void Start()
    {
        currentSettings = LevelTimeSetting.Instance;
        if (currentSettings != null)
        {
            currentTime = currentSettings.startHour;
            isTimeStopped = false;
            hasSentTimeExpiredNotice = false;
        }
    }

    void Update()
    {
        if (currentSettings == null) return;

        if (!isTimeStopped)
        {
            float previousTime = currentTime;
            float nextTime = currentTime + Time.deltaTime * currentSettings.timeSpeed;
            
            if (nextTime >= 24f) nextTime -= 24f;

            currentTime = nextTime;

            if (HasReachedEndHour(previousTime, currentTime, currentSettings.endHour))
            {
                currentTime = currentSettings.endHour;
                isTimeStopped = true;
            }
        }

        if (isTimeStopped && !hasSentTimeExpiredNotice)
        {
            NotifyEnemiesTimeExpired();
        }

        UpdateUI();
    }

    private bool HasReachedEndHour(float previous, float current, float endHour)
    {
        if (Mathf.Approximately(previous, current))
            return false;

        if (previous < current)
            return endHour >= previous && endHour <= current;

        return endHour >= previous || endHour <= current;
    }


    private void NotifyEnemiesTimeExpired()
    {
        if (EnemyManager.Instance == null)
            return;

        if (agreeGrationPoint == null)
            return;

        EnemyManager.Instance.NotifyTimeExpired(agreeGrationPoint.position);
        hasSentTimeExpiredNotice = true;
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
            float alpha = Mathf.PingPong(Time.time * 2f, 1f); 
            timeText.color = Color.Lerp(Color.white, Color.red, alpha);
        }
        else
        {
            timeText.color = Color.white;
        }
    }
}