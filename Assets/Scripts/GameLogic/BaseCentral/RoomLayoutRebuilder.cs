using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class RoomLayoutRebuilder : MonoBehaviour
{
    [SerializeField] private Transform roomRoot;
    [SerializeField] private ItemPrefabDatabase prefabDatabase;
    [SerializeField] private bool clearExistingChildren = true;
    [SerializeField] private bool rebuildOnStart = true;
    [SerializeField] private bool reapplyTransformAfterSpawn = true;

    private Coroutine rebuildCoroutine;

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
        if (rebuildOnStart)
            Rebuild();
    }

    public void Rebuild()
    {
        if (!isActiveAndEnabled)
            return;

        if (rebuildCoroutine != null)
            StopCoroutine(rebuildCoroutine);

        rebuildCoroutine = StartCoroutine(RebuildRoutine());
    }

    private IEnumerator RebuildRoutine()
    {
        RoomStateManager manager = RoomStateManager.Instance;

        if (roomRoot == null || prefabDatabase == null || manager == null || manager.State == null || manager.State.Items == null)
            yield break;

        if (clearExistingChildren)
        {
            for (int i = roomRoot.childCount - 1; i >= 0; i--)
            {
                Transform child = roomRoot.GetChild(i);
                if (child != null)
                    Destroy(child.gameObject);
            }

            // Đợi 1 frame để tránh object cũ và mới chồng nhau trong cùng frame.
            yield return null;
        }

        var items = manager.State.Items;
        int count = items.Count;

        for (int i = 0; i < count; i++)
        {
            RoomItemState state = items[i];
            if (state == null)
                continue;

            if (!prefabDatabase.TryGetPrefab(state.itemId, out GameObject prefab) || prefab == null)
            {
                Debug.LogWarning($"RoomLayoutRebuilder: Missing prefab for itemId {state.itemId}");
                continue;
            }

            GameObject spawned = Instantiate(prefab, roomRoot, false);
            Transform spawnedTransform = spawned.transform;

            ApplyLocalTransform(spawnedTransform, state);

            if (reapplyTransformAfterSpawn)
                StartCoroutine(ReapplyTransformAfterSpawn(spawnedTransform, state));

            SetupLootItem(spawned, state);
            SetupWorldItem(spawned, state);
        }

        rebuildCoroutine = null;
    }

    private static void ApplyLocalTransform(Transform target, RoomItemState state)
    {
        if (target == null || state == null)
            return;

        target.localPosition = state.localPosition;
        target.localRotation = state.localRotation;
        target.localScale = state.localScale;
    }

    private IEnumerator ReapplyTransformAfterSpawn(Transform target, RoomItemState state)
    {
        if (target == null || state == null)
            yield break;

        yield return null;

        if (target == null)
            yield break;

        ApplyLocalTransform(target, state);

        yield return new WaitForFixedUpdate();

        if (target == null)
            yield break;

        ApplyLocalTransform(target, state);
    }

    private void SetupLootItem(GameObject spawned, RoomItemState state)
    {
        LootItem lootItem = spawned.GetComponent<LootItem>();
        if (lootItem == null)
            lootItem = spawned.AddComponent<LootItem>();

        lootItem.SetItemId(state.itemId);
        lootItem.SetCargoItemId(state.placementId);
    }

    private void SetupWorldItem(GameObject spawned, RoomItemState state)
    {
        WorldItem worldItem = spawned.GetComponent<WorldItem>();
        if (worldItem == null)
            worldItem = spawned.AddComponent<WorldItem>();

        if (prefabDatabase.TryGetItemData(state.itemId, out ItemData itemData))
            worldItem.itemData = itemData;
    }
}