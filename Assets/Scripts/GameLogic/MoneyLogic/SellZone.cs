using System.Collections.Generic;
using UnityEngine;

public class SellZone : MonoBehaviour
{
    private List<WorldItem> itemsInZone = new List<WorldItem>();

    private void OnTriggerEnter(Collider other)
    {
        WorldItem worldItem = other.GetComponent<WorldItem>();
        if (worldItem != null && !itemsInZone.Contains(worldItem))
        {
            itemsInZone.Add(worldItem);
            Debug.Log("Item entered sell zone: " + worldItem.itemData.itemName);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WorldItem worldItem = other.GetComponent<WorldItem>();
        if (worldItem != null && itemsInZone.Contains(worldItem))
        {
            itemsInZone.Remove(worldItem);
        }
    }

    // Hàm kiểm tra và bán tất cả vật phẩm trong khu vực
    public void CheckItemsInZone()
    {
        foreach (WorldItem item in itemsInZone)
        {
            if (item != null && item.itemData != null)
            {
                float price = item.itemData.price;
                // Thêm điểm vào PointManager
                PointManager.Instance.AddPoints(price);
                // Hủy vật phẩm sau khi bán
                Destroy(item.gameObject);
            }
        }
        // Xóa danh sách sau khi bán
        itemsInZone.Clear();
    }
}