using System;
using UnityEngine;

[DisallowMultipleComponent]
public class LootItem : MonoBehaviour
{
    [SerializeField] private string itemId;
    [SerializeField] private string cargoItemId;

    public string ItemId => itemId;
    public string CargoItemId => cargoItemId;
    public bool HasValidItemId => !string.IsNullOrEmpty(itemId);

    private void Awake()
    {
        EnsureCargoItemId();
    }

    public void EnsureCargoItemId()
    {
        if (string.IsNullOrEmpty(cargoItemId))
        {
            cargoItemId = Guid.NewGuid().ToString("N");
        }
    }

    public void SetItemId(string newItemId)
    {
        itemId = newItemId;
    }

    public void SetCargoItemId(string newCargoItemId)
    {
        cargoItemId = newCargoItemId;
    }

}
