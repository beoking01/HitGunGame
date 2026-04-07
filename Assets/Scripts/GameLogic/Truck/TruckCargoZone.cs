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
        TryRegisterOrUpdateCargoItem(other, true);
    }

    private void OnTriggerStay(Collider other)
    {
        TryRegisterOrUpdateCargoItem(other, false);
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

    private void TryRegisterOrUpdateCargoItem(Collider other, bool setParentToCargo)
    {
        if (other == null || !other.CompareTag(itemTag))
            return;

        LootItem lootItem = ItemRuntimeUtility.GetOrAddLootItem(other.gameObject);
        ItemRuntimeUtility.EnsureItemId(lootItem, other.GetComponent<WorldItem>(), prefabDatabase);

        if (!lootItem.HasValidItemId)
        {
            if (setParentToCargo)
            {
                Debug.LogWarning("TruckCargoZone: Missing itemId, cannot store item state.");
            }
            return;
        }

        if (setParentToCargo)
        {
            other.transform.SetParent(cargoRoot, true);
        }

        if (TruckStateManager.Instance != null)
        {
            TruckStateManager.Instance.UpsertFromCargo(lootItem, cargoRoot);
        }
    }
}
