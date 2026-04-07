using UnityEngine;

public class PlayerPickUpDrop : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public InventorySystem inventorySystem;
    public Transform dropPoint;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private Transform weaponGrabPointTransform;
    [Header("Settings")]
    public float pickupDistance = 3f;
    public KeyCode pickupKey = KeyCode.F;
    public KeyCode dropKey = KeyCode.G;

    private void Start()
    {
        if (inventorySystem != null)
        { 
            if(weaponGrabPointTransform != null)
                inventorySystem.weaponGrabPointTransform = weaponGrabPointTransform;
            if(objectGrabPointTransform != null)
                inventorySystem.objectGrabPointTransform = objectGrabPointTransform;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(pickupKey))
            TryPickup();

        if (Input.GetKeyDown(dropKey))
            TryDrop();
    }

    private void TryPickup()
    {
        if (inventorySystem == null || objectGrabPointTransform == null || playerCamera == null)
        {
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, pickupDistance))
            return;

        WorldItem pickupItem = hit.collider.GetComponent<WorldItem>();
        if (pickupItem == null || pickupItem.itemData == null)
            return;

        if (!hit.collider.TryGetComponent(out ObjectGrabbable incomingGrabbable))
            return;

        DetachFromContainerIfNeeded(incomingGrabbable);

        // Always detach from any parent before grabbing so scale is not inherited from room/truck roots.
        // incomingGrabbable.transform.SetParent(null, true);

        int index = inventorySystem.selectedIndex;

        // Nếu slot hiện tại đã có item/object thì thả object cũ ra ngoài trước
        ObjectGrabbable oldGrabbable = inventorySystem.GetSlotGrabbable(index);

        if (oldGrabbable != null && oldGrabbable != incomingGrabbable)
        {
            DropObjectToWorld(oldGrabbable);
        }

        // Gán item mới vào slot đang chọn
        inventorySystem.SetSelectedItem(pickupItem.itemData);

        // Gán object mới vào slot
        inventorySystem.SetSlotGrabbable(index, incomingGrabbable);
        if (pickupItem.itemData != null && pickupItem.itemData.itemType == ItemData.ItemType.Weapon)
        {
            incomingGrabbable.Grab(inventorySystem.weaponGrabPointTransform);
        }
        else
        {
            incomingGrabbable.Grab(inventorySystem.objectGrabPointTransform);
        }
        
        incomingGrabbable.gameObject.SetActive(true);
    }

    private void TryDrop()
    {
        if (inventorySystem == null)
            return;

        int index = inventorySystem.selectedIndex;
        ItemData selectedItem = inventorySystem.GetSelectedItem();

        if (selectedItem == null)
            return;

        ObjectGrabbable current = inventorySystem.GetSlotGrabbable(index);

        if (current != null)
        {
            current.transform.SetParent(null); // Bỏ cha trước khi thả
            DropObjectToWorld(current);
        }

        inventorySystem.ClearSelectedSlot();
    }

    private void DropObjectToWorld(ObjectGrabbable obj)
    {
        if (obj == null)
            return;

        Transform targetPoint = dropPoint != null ? dropPoint : transform;

        obj.gameObject.SetActive(true);
        obj.transform.position = targetPoint.position;
        obj.transform.rotation = targetPoint.rotation;
        obj.Drop();
    }

    private void DetachFromContainerIfNeeded(ObjectGrabbable grabbable)
    {
        if (grabbable == null)
            return;

        LootItem lootItem = grabbable.GetComponent<LootItem>();
        if (lootItem == null)
            return;

        TruckCargoZone cargoZone = grabbable.GetComponentInParent<TruckCargoZone>();
        if (cargoZone != null)
        {
            cargoZone.RemoveItemFromTruck(lootItem);
            return;
        }

        RoomPlacementZone roomZone = grabbable.GetComponentInParent<RoomPlacementZone>();
        if (roomZone != null)
        {
            roomZone.RemoveItemFromRoom(lootItem);
        }
    }
}