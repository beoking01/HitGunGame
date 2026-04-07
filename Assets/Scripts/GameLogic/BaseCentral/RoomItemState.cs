using System;
using UnityEngine;

[Serializable]
public class RoomItemState
{
    [Tooltip("Stable instance id for an item placed in Base Central room")]
    public string placementId;

    [Tooltip("Stable item id used to resolve prefab from ItemPrefabDatabase")]
    public string itemId;

    [Tooltip("Transform relative to roomRoot")]
    public Vector3 localPosition;

    [Tooltip("Transform relative to roomRoot")]
    public Quaternion localRotation;

    [Tooltip("Keeps room arrangement consistent across sessions")]
    public Vector3 localScale = Vector3.one;

    public RoomItemState Clone()
    {
        return new RoomItemState
        {
            placementId = placementId,
            itemId = itemId,
            localPosition = localPosition,
            localRotation = localRotation,
            localScale = localScale
        };
    }
}