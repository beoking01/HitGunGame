using UnityEngine;
using TMPro;
// process UI displayed in player screen
public class PlayerUI : MonoBehaviour,IPointObserver
{

    [SerializeField]
    private TextMeshProUGUI promtText;
    public TextMeshProUGUI pointText;
    private PlayerInventory playerInventory;
    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        // Đăng ký observer
        PointManager.Instance.AddObserver(this);
        // Cập nhật UI ban đầu
        OnPointsChanged(PointManager.Instance.GetPoints());
    }

    void Update()
    {
    }
    public void UpdateText(string promtMessage)
    {
        promtText.text = promtMessage;
    }


    private void OnDestroy()
    {
        // Hủy đăng ký
        if (PointManager.Instance != null)
        {
            PointManager.Instance.RemoveObserver(this);
        }
    }

    public void OnPointsChanged(float newPoints)
    {
        if (pointText != null)
        {
            pointText.text =  newPoints.ToString("F0");
        }
    }
}
