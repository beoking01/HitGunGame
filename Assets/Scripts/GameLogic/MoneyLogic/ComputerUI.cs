using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ComputerUI : MonoBehaviour, IPointObserver
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [Header("Danh sách tất cả các Panels")]
    public List<GameObject> allPanels;

    private void Start()
    {
        PointManager.Instance.AddObserver(this);
        UpdatePointsText(PointManager.Instance.Points);
    }

    private void OnDestroy()
    {
        if (PointManager.Instance != null)
            PointManager.Instance.RemoveObserver(this);
    }

    public void OnPointsChanged(float newPoints)
    {
        UpdatePointsText(newPoints);
    }

    private void UpdatePointsText(float points)
    {
        if (pointsText != null)
            pointsText.text = $"{points:F2}";
    }
    public void BuyProcess(float price)
    {
        if (PointManager.Instance.Points >= price)
        {
            PointManager.Instance.AddPoints(-price);
            Debug.Log("Purchase successful!");
        }
        else
        {
            Debug.Log("Not enough points to purchase.");
        }
    }
    // Hàm này sẽ được gọi khi bấm nút
    public void OpenPanel(int panelIndex)
    {
        for (int i = 0; i < allPanels.Count; i++)
        {
            // Nếu i trùng với index truyền vào thì hiện, ngược lại thì ẩn
            allPanels[i].SetActive(i == panelIndex);
        }
    }
}