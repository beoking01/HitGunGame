[System.Serializable]
public class InventorySlot
{
    public ItemData itemData;

    public bool IsEmpty()
    {
        return itemData == null;
    }

    public void SetItem(ItemData newItem)
    {
        itemData = newItem;
    }

    public void Clear()
    {
        itemData = null;
    }
}