using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    public InventorySystem inventorySystem;

    [Header("UI Slots")]
    public Image[] itemIcons;        // 5 image icon
    public Image[] slotHighlights;   // 5 image viền trắng

    private void Start()
    {
        inventorySystem.OnInventoryChanged += RefreshUI;
        inventorySystem.OnSelectedSlotChanged += UpdateSelectionUI;

        RefreshUI();
        UpdateSelectionUI(inventorySystem.selectedIndex);
    }

    private void OnDestroy()
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnInventoryChanged -= RefreshUI;
            inventorySystem.OnSelectedSlotChanged -= UpdateSelectionUI;
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < inventorySystem.slots.Length; i++)
        {
            ItemData item = inventorySystem.slots[i].itemData;

            if (item != null && item.itemIcon != null)
            {
                itemIcons[i].sprite = item.itemIcon;
                itemIcons[i].enabled = true;
            }
            else
            {
                itemIcons[i].sprite = null;
                itemIcons[i].enabled = false;
            }
        }
    }

    public void UpdateSelectionUI(int selectedIndex)
    {
        for (int i = 0; i < slotHighlights.Length; i++)
        {
            slotHighlights[i].enabled = (i == selectedIndex);
        }
    }
}