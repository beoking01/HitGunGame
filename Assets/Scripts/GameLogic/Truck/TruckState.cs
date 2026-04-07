using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TruckState
{
    [SerializeField] private List<TruckItemState> items = new List<TruckItemState>();

    public IReadOnlyList<TruckItemState> Items => items;

    public void Clear()
    {
        items.Clear();
    }

    public void Upsert(TruckItemState state)
    {
        if (state == null)
            return;

        if (string.IsNullOrEmpty(state.cargoItemId) || string.IsNullOrEmpty(state.itemId))
            return;

        int index = items.FindIndex(x => x.cargoItemId == state.cargoItemId);
        if (index < 0)
        {
            items.Add(state.Clone());
            return;
        }

        items[index] = state.Clone();
    }

    public bool RemoveByCargoId(string cargoItemId)
    {
        if (string.IsNullOrEmpty(cargoItemId))
            return false;

        int removed = items.RemoveAll(x => x.cargoItemId == cargoItemId);
        return removed > 0;
    }

    public List<TruckItemState> CopyAll()
    {
        List<TruckItemState> copy = new List<TruckItemState>(items.Count);
        for (int i = 0; i < items.Count; i++)
        {
            copy.Add(items[i].Clone());
        }

        return copy;
    }
}
