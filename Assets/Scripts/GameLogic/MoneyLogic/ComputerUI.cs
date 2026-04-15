using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ComputerUI : MonoBehaviour, IPointObserver
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private ItemPrefabDatabase prefabDatabase;
    [SerializeField] private Transform itemSpawnRoot;
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
    public void Buy(string itemId)
    {
        ItemData itemData = null;
        prefabDatabase.TryGetItemData(itemId, out itemData);
        float price = itemData.price ;
        if (PointManager.Instance.Points >= price)
        {
            PointManager.Instance.AddPoints(-price);
            BuyProcess(itemId);
            Debug.Log("Purchase successful!");
        }
        else
        {
            Debug.Log("Not enough points to purchase.");
        }
    }
    private void BuyProcess(string itemId)
    {
        if (prefabDatabase == null || itemSpawnRoot == null)
            return;

        if (!prefabDatabase.TryGetPrefab(itemId, out GameObject prefab))
        {
            Debug.LogWarning("ComputerUI: Missing prefab for itemId " + itemId);
            return;
        }

        Instantiate(prefab, itemSpawnRoot);
    }
}