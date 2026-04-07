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
        TryRegisterOrUpdateRoomItem(other, true);
    }

    private void OnTriggerStay(Collider other)
    {
        TryRegisterOrUpdateRoomItem(other, false);
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

    private void TryRegisterOrUpdateRoomItem(Collider other, bool setParentToRoom)
    {
        if (other == null || !other.CompareTag(itemTag))
            return;

        LootItem lootItem = ItemRuntimeUtility.GetOrAddLootItem(other.gameObject);
        ItemRuntimeUtility.EnsureItemId(lootItem, other.GetComponent<WorldItem>(), prefabDatabase);

        if (!lootItem.HasValidItemId)
        {
            if (setParentToRoom)
            {
                Debug.LogWarning("RoomPlacementZone: Missing itemId, cannot store room state.");
            }
            return;
        }

        if (setParentToRoom)
        {
            other.transform.SetParent(roomRoot, true);
        }

        if (RoomStateManager.Instance != null)
        {
            RoomStateManager.Instance.UpsertFromRoom(lootItem, roomRoot);
        }
    }
}