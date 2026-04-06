using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoomState
{
    [SerializeField] private List<RoomItemState> items = new List<RoomItemState>();

    public IReadOnlyList<RoomItemState> Items => items;

    public void Clear()
    {
        items.Clear();
    }

    public void Upsert(RoomItemState state)
    {
        if (state == null)
            return;

        if (string.IsNullOrEmpty(state.placementId) || string.IsNullOrEmpty(state.itemId))
            return;

        int index = items.FindIndex(x => x.placementId == state.placementId);
        if (index < 0)
        {
            items.Add(state.Clone());
            return;
        }

        items[index] = state.Clone();
    }

    public bool RemoveByPlacementId(string placementId)
    {
        if (string.IsNullOrEmpty(placementId))
            return false;

        int removed = items.RemoveAll(x => x.placementId == placementId);
        return removed > 0;
    }

    public List<RoomItemState> CopyAll()
    {
        List<RoomItemState> copy = new List<RoomItemState>(items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            copy.Add(items[i].Clone());
        }

        return copy;
    }
}