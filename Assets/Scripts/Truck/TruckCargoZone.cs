using UnityEngine;

[DisallowMultipleComponent]
public class TruckCargoZone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cargoRoot;
    [SerializeField] private ItemPrefabDatabase prefabDatabase;

    [Header("Rules")]
    [SerializeField] private string itemTag = "Item";

    private void Reset()
    {
        cargoRoot = transform;
    }

    private void Awake()
    {
        if (cargoRoot == null)
            cargoRoot = transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(itemTag))
            return;

        LootItem lootItem = GetOrAddLootItem(other.gameObject);
        EnsureItemId(lootItem, other.GetComponent<WorldItem>());

        if (!lootItem.HasValidItemId)
        {
            Debug.LogWarning("TruckCargoZone: Missing itemId, cannot store item state.");
            return;
        }

        other.transform.SetParent(cargoRoot, true);


        if (TruckStateManager.Instance != null)
        {
            TruckStateManager.Instance.UpsertFromCargo(lootItem, cargoRoot);
        }
    }

    private void OnDisable()
    {
        CaptureSnapshotNow();
    }

    public bool RemoveItemFromTruck(LootItem lootItem)
    {
        if (lootItem == null || cargoRoot == null)
            return false;

        if (!lootItem.transform.IsChildOf(cargoRoot))
            return false;

        if (TruckStateManager.Instance != null)
        {
            TruckStateManager.Instance.RemoveByCargoId(lootItem.CargoItemId);
        }

        lootItem.transform.SetParent(null, true);

        return true;
    }

    public void CaptureSnapshotNow()
    {
        if (TruckStateManager.Instance != null)
        {
            TruckStateManager.Instance.CaptureSnapshot(cargoRoot);
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

        // Fallback id keeps cargo state usable even before database mapping is fully configured.
        string fallbackItemId = worldItem.itemData.name;
        if (!string.IsNullOrEmpty(fallbackItemId))
        {
            lootItem.SetItemId(fallbackItemId);
        }
    }
}
