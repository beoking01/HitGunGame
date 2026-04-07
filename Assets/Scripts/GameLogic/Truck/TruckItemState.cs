using System;
using UnityEngine;

[Serializable]
public class TruckItemState
{
    [Tooltip("Unique id for a spawned item instance in truck cargo")]
    public string cargoItemId;

    [Tooltip("Stable item id used to resolve prefab from ItemPrefabDatabase")]
    public string itemId;

    [Tooltip("Transform relative to cargoRoot")]
    public Vector3 localPosition;

    [Tooltip("Transform relative to cargoRoot")]
    public Quaternion localRotation;

    [Tooltip("Keeps stacking and arrangement consistent across scenes")]
    public Vector3 localScale = Vector3.one;

    public TruckItemState Clone()
    {
        return new TruckItemState
        {
            cargoItemId = cargoItemId,
            itemId = itemId,
            localPosition = localPosition,
            localRotation = localRotation,
            localScale = localScale
        };
    }
}
