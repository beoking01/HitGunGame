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

            ItemRuntimeUtility.ApplyLocalTransform(spawnedTransform, state.localPosition, state.localRotation, state.localScale);

            if (reapplyTransformAfterSpawn)
                StartCoroutine(ItemRuntimeUtility.ReapplyTransformAfterSpawn(spawnedTransform, state.localPosition, state.localRotation, state.localScale));

            ItemRuntimeUtility.SetupLootAndWorldItem(spawned, state.itemId, state.placementId, prefabDatabase);
        }

        rebuildCoroutine = null;
    }
}