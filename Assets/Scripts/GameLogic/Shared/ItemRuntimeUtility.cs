using System.Collections;
using UnityEngine;

public static class ItemRuntimeUtility
{
    public static LootItem GetOrAddLootItem(GameObject itemObject)
    {
        if (itemObject == null)
            return null;

        LootItem lootItem = itemObject.GetComponent<LootItem>();
        if (lootItem == null)
            lootItem = itemObject.AddComponent<LootItem>();

        lootItem.EnsureCargoItemId();
        return lootItem;
    }

    public static void EnsureItemId(LootItem lootItem, WorldItem worldItem, ItemPrefabDatabase prefabDatabase)
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

    public static void SetupLootAndWorldItem(GameObject spawned, string itemId, string instanceId, ItemPrefabDatabase prefabDatabase)
    {
        if (spawned == null)
            return;

        LootItem lootItem = GetOrAddLootItem(spawned);
        if (lootItem != null)
        {
            lootItem.SetItemId(itemId);
            lootItem.SetCargoItemId(instanceId);
        }

        WorldItem worldItem = spawned.GetComponent<WorldItem>();
        if (worldItem == null)
            worldItem = spawned.AddComponent<WorldItem>();

        if (prefabDatabase != null && prefabDatabase.TryGetItemData(itemId, out ItemData itemData))
        {
            worldItem.itemData = itemData;
        }
    }

    public static void ApplyLocalTransform(Transform target, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
    {
        if (target == null)
            return;

        target.localPosition = localPosition;
        target.localRotation = localRotation;
        target.localScale = localScale;
    }

    public static IEnumerator ReapplyTransformAfterSpawn(Transform target, Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
    {
        if (target == null)
            yield break;

        yield return null;
        if (target == null)
            yield break;

        ApplyLocalTransform(target, localPosition, localRotation, localScale);

        yield return new WaitForFixedUpdate();
        if (target == null)
            yield break;

        ApplyLocalTransform(target, localPosition, localRotation, localScale);
    }
}
