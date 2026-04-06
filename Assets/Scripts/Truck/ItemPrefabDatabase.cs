using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemPrefabDatabase", menuName = "Game/Truck/Item Prefab Database")]
public class ItemPrefabDatabase : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public string itemId;
        public GameObject worldPrefab;
        public ItemData itemData;
    }

    [SerializeField] private List<Entry> entries = new List<Entry>();

    private Dictionary<string, Entry> map;

    private void OnEnable()
    {
        RebuildCache();
    }

    private void RebuildCache()
    {
        map = new Dictionary<string, Entry>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < entries.Count; i++)
        {
            Entry entry = entries[i];
            if (entry == null || string.IsNullOrEmpty(entry.itemId))
                continue;

            if (!map.ContainsKey(entry.itemId))
            {
                map.Add(entry.itemId, entry);
            }
        }
    }

    public bool TryGetPrefab(string itemId, out GameObject prefab)
    {
        prefab = null;
        if (string.IsNullOrEmpty(itemId))
            return false;

        if (map == null)
            RebuildCache();

        if (!TryResolveEntry(itemId, out Entry entry))
            return false;

        prefab = entry.worldPrefab;
        return prefab != null;
    }

    public bool TryGetItemData(string itemId, out ItemData itemData)
    {
        itemData = null;
        if (string.IsNullOrEmpty(itemId))
            return false;

        if (map == null)
            RebuildCache();

        if (!TryResolveEntry(itemId, out Entry entry))
            return false;

        itemData = entry.itemData;
        return itemData != null;
    }

    public bool TryFindIdByItemData(ItemData itemData, out string itemId)
    {
        itemId = null;
        if (itemData == null)
            return false;

        for (int i = 0; i < entries.Count; i++)
        {
            Entry entry = entries[i];
            if (entry == null)
                continue;

            if (entry.itemData == itemData && !string.IsNullOrEmpty(entry.itemId))
            {
                itemId = entry.itemId;
                return true;
            }
        }

        return false;
    }

    private bool TryResolveEntry(string itemId, out Entry entry)
    {
        entry = null;

        if (string.IsNullOrEmpty(itemId))
            return false;

        if (map != null && map.TryGetValue(itemId, out entry))
            return entry != null;

        for (int i = 0; i < entries.Count; i++)
        {
            Entry candidate = entries[i];
            if (candidate == null)
                continue;

            if (candidate.itemData != null && string.Equals(candidate.itemData.name, itemId, StringComparison.OrdinalIgnoreCase))
            {
                entry = candidate;
                return true;
            }

            if (candidate.worldPrefab != null && string.Equals(candidate.worldPrefab.name, itemId, StringComparison.OrdinalIgnoreCase))
            {
                entry = candidate;
                return true;
            }
        }

        return false;
    }
}
