using UnityEngine;

[DisallowMultipleComponent]
public class TruckCargoRebuilder : MonoBehaviour
{
    [SerializeField] private Transform cargoRoot;
    [SerializeField] private ItemPrefabDatabase prefabDatabase;
    [SerializeField] private bool clearExistingChildren = true;
    [SerializeField] private bool applyInCargoPhysics = true;

    private bool rebuilt;

    private void Reset()
    {
        cargoRoot = transform;
    }

    private void Awake()
    {
        if (cargoRoot == null)
            cargoRoot = transform;
    }

    private void Start()
    {
        Rebuild();
    }

    public void Rebuild()
    {
        if (rebuilt)
            return;

        rebuilt = true;

        if (TruckStateManager.Instance == null || prefabDatabase == null || cargoRoot == null)
            return;

        if (clearExistingChildren)
        {
            for (int i = cargoRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(cargoRoot.GetChild(i).gameObject);
            }
        }

        var items = TruckStateManager.Instance.State.Items;

        for (int i = 0; i < items.Count; i++)
        {
            TruckItemState state = items[i];
            if (state == null)
                continue;

            if (!prefabDatabase.TryGetPrefab(state.itemId, out GameObject prefab))
            {
                Debug.LogWarning("TruckCargoRebuilder: Missing prefab for itemId " + state.itemId);
                continue;
            }

            GameObject spawned = Instantiate(prefab, cargoRoot);
            spawned.transform.localPosition = state.localPosition;
            spawned.transform.localRotation = state.localRotation;
            spawned.transform.localScale = state.localScale;

            LootItem lootItem = spawned.GetComponent<LootItem>();
            if (lootItem == null)
                lootItem = spawned.AddComponent<LootItem>();

            lootItem.SetItemId(state.itemId);
            lootItem.SetCargoItemId(state.cargoItemId);

            if (applyInCargoPhysics)
            {
            }

            WorldItem worldItem = spawned.GetComponent<WorldItem>();
            if (worldItem == null)
                worldItem = spawned.AddComponent<WorldItem>();

            if (prefabDatabase.TryGetItemData(state.itemId, out ItemData itemData))
            {
                worldItem.itemData = itemData;
            }
        }
    }
}
