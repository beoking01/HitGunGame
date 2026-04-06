using UnityEngine;

[DisallowMultipleComponent]
public class RoomPlacementZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform roomRoot;
    [SerializeField] private ItemPrefabDatabase prefabDatabase;

    [Header("Rules")]
    [SerializeField] private string itemTag = "Item";

    private void Reset()
    {
        roomRoot = transform;
    }

    private void Awake()
    {
        if (roomRoot == null)
            roomRoot = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryRegisterItem(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryRegisterItem(other);
    }

    private void OnDisable()
    {
        CaptureSnapshotNow();
    }

    public bool RemoveItemFromRoom(LootItem lootItem)
    {
        if (lootItem == null || roomRoot == null)
            return false;

        if (!lootItem.transform.IsChildOf(roomRoot))
            return false;

        if (RoomStateManager.Instance != null)
        {
            RoomStateManager.Instance.RemoveByPlacementId(lootItem.CargoItemId);
        }

        lootItem.transform.SetParent(null, true);

        return true;
    }

    public void CaptureSnapshotNow()
    {
        if (RoomStateManager.Instance != null)
        {
            RoomStateManager.Instance.CaptureSnapshot(roomRoot);
        }
    }

    private void TryRegisterItem(Collider other)
    {
        if (other == null || !other.CompareTag(itemTag))
            return;

        LootItem lootItem = GetOrAddLootItem(other.gameObject);
        EnsureItemId(lootItem, other.GetComponent<WorldItem>());

        if (!lootItem.HasValidItemId)
        {
            Debug.LogWarning("RoomPlacementZone: Missing itemId, cannot store room state.");
            return;
        }

        other.transform.SetParent(roomRoot, true);

        if (RoomStateManager.Instance != null)
        {
            RoomStateManager.Instance.UpsertFromRoom(lootItem, roomRoot);
        }
    }

    private LootItem GetOrAddLootItem(GameObject itemObject)
    {
        LootItem lootItem = itemObject.GetComponent<LootItem>();
        if (lootItem == null)
            lootItem = itemObject.AddComponent<LootItem>();

        lootItem.EnsureCargoItemId();
        return lootItem;
    }

    private void EnsureItemId(LootItem lootItem, WorldItem worldItem)
    {
        if (lootItem == null || lootItem.HasValidItemId)
            return;

        if (worldItem == null || worldItem.itemData == null)
            return;

        if (prefabDatabase != null && prefabDatabase.TryFindIdByItemData(worldItem.itemData, out string mappedItemId))
        {
            lootItem.SetItemId(mappedItemId);
            return;
        }

        string fallbackItemId = worldItem.itemData.name;
        if (!string.IsNullOrEmpty(fallbackItemId))
        {
            lootItem.SetItemId(fallbackItemId);
        }
    }
}