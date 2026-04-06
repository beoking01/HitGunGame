using UnityEngine;

[DisallowMultipleComponent]
public class RoomLayoutRebuilder : MonoBehaviour
{
    [SerializeField] private Transform roomRoot;
    [SerializeField] private ItemPrefabDatabase prefabDatabase;
    [SerializeField] private bool clearExistingChildren = true;

    private bool rebuilt;

    private void Reset()
    {
        roomRoot = transform;
    }

    private void Awake()
    {
        if (roomRoot == null)
            roomRoot = transform;
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

        if (RoomStateManager.Instance == null || prefabDatabase == null || roomRoot == null)
            return;

        if (clearExistingChildren)
        {
            for (int i = roomRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(roomRoot.GetChild(i).gameObject);
            }
        }

        var items = RoomStateManager.Instance.State.Items;

        for (int i = 0; i < items.Count; i++)
        {
            RoomItemState state = items[i];
            if (state == null)
                continue;

            if (!prefabDatabase.TryGetPrefab(state.itemId, out GameObject prefab))
            {
                Debug.LogWarning("RoomLayoutRebuilder: Missing prefab for itemId " + state.itemId);
                continue;
            }

            GameObject spawned = Instantiate(prefab, roomRoot);
            spawned.transform.localPosition = state.localPosition;
            spawned.transform.localRotation = state.localRotation;
            spawned.transform.localScale = state.localScale;

            LootItem lootItem = spawned.GetComponent<LootItem>();
            if (lootItem == null)
                lootItem = spawned.AddComponent<LootItem>();

            lootItem.SetItemId(state.itemId);
            lootItem.SetCargoItemId(state.placementId);

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