using UnityEngine;

public class PlayerItemActionController : MonoBehaviour
{
    [SerializeField] private InventorySystem inventorySystem;

    private void Awake()
    {
        if (inventorySystem == null)
            inventorySystem = GetComponent<InventorySystem>();
    }

    public void UsePrimary()
    {
        if (inventorySystem == null)
            return;

        ItemData selectedItem = inventorySystem.GetSelectedItem();
        if (selectedItem == null)
            return;

        ObjectGrabbable currentObject = inventorySystem.GetSlotGrabbable(inventorySystem.selectedIndex);
        if (currentObject == null)
            return;

        switch (selectedItem.itemType)
        {
            case ItemData.ItemType.Normal:
                return;

            case ItemData.ItemType.Weapon:
                IWeaponAction weaponAction = ResolveAction<IWeaponAction>(currentObject);
                if (weaponAction != null)
                {
                    weaponAction.PrimaryAttack(gameObject);
                }
                else
                    Debug.LogWarning($"No IWeaponAction found on selected item '{currentObject.name}'.");
                return;

            case ItemData.ItemType.Consumable:
                IConsumableAction consumableAction = ResolveAction<IConsumableAction>(currentObject);
                if (consumableAction != null && consumableAction.Consume(gameObject))
                    ConsumeCurrentItem(currentObject);
                else if (consumableAction == null)
                    Debug.LogWarning($"No IConsumableAction found on selected item '{currentObject.name}'.");
                return;
        }
    }

    public void ReloadCurrent()
    {
        if (inventorySystem == null)
            return;

        ItemData selectedItem = inventorySystem.GetSelectedItem();
        if (selectedItem == null || selectedItem.itemType != ItemData.ItemType.Weapon)
            return;

        ObjectGrabbable currentObject = inventorySystem.GetSlotGrabbable(inventorySystem.selectedIndex);
        if (currentObject == null)
            return;

        IReloadableAction reloadableAction = ResolveAction<IReloadableAction>(currentObject);
        if (reloadableAction != null)
        {
            reloadableAction.Reload(gameObject);
        }
        else
            Debug.LogWarning($"No IReloadableAction found on selected weapon '{currentObject.name}'.");
    }

    private void ConsumeCurrentItem(ObjectGrabbable currentObject)
    {
        inventorySystem.ClearSelectedSlot();
        Destroy(currentObject.gameObject);
    }


   

    private T ResolveAction<T>(ObjectGrabbable currentObject) where T : class
    {
        T action = currentObject.GetComponent(typeof(T)) as T;
        if (action != null)
            return action;

        action = currentObject.GetComponentInChildren(typeof(T), true) as T;
        if (action != null)
            return action;

        action = currentObject.GetComponentInParent(typeof(T)) as T;
        return action;
    }
}
