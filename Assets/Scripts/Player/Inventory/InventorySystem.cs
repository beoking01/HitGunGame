using System;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public InventorySlot[] slots = new InventorySlot[5];
    public int selectedIndex = 0;

    public Action OnInventoryChanged;
    public Action<int> OnSelectedSlotChanged;

    [HideInInspector] public Transform objectGrabPointTransform;
    [HideInInspector] public Transform weaponGrabPointTransform;
    private ObjectGrabbable[] slotGrabbables;

    private void Awake()
    {
        if (slots == null || slots.Length == 0)
            slots = new InventorySlot[5];

        slotGrabbables = new ObjectGrabbable[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                slots[i] = new InventorySlot();

            slotGrabbables[i] = null;
        }
    }

    private void Start()
    {
        UpdateSlotObjects();
        OnSelectedSlotChanged?.Invoke(selectedIndex);
        OnInventoryChanged?.Invoke();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= slots.Length)
            return;

        selectedIndex = index;
        UpdateSlotObjects();
        OnSelectedSlotChanged?.Invoke(selectedIndex);
    }

    public void SelectNext()
    {
        selectedIndex++;
        if (selectedIndex >= slots.Length)
            selectedIndex = 0;

        UpdateSlotObjects();
        OnSelectedSlotChanged?.Invoke(selectedIndex);
    }

    public void SelectPrevious()
    {
        selectedIndex--;
        if (selectedIndex < 0)
            selectedIndex = slots.Length - 1;

        UpdateSlotObjects();
        OnSelectedSlotChanged?.Invoke(selectedIndex);
    }

    private void UpdateSlotObjects()
    {
        for (int i = 0; i < slotGrabbables.Length; i++)
        {
            ObjectGrabbable obj = slotGrabbables[i];
            ItemData item = slots[i].itemData;
            if (obj == null) continue;

            if (i == selectedIndex)
            {
                obj.gameObject.SetActive(true);
                if (item != null && item.itemType == ItemData.ItemType.Weapon)
                {
                    if (weaponGrabPointTransform != null)
                        obj.Grab(weaponGrabPointTransform);
                }
                else
                if (objectGrabPointTransform != null)
                    obj.Grab(objectGrabPointTransform);
            }
            else
            {
                obj.Drop();
                obj.gameObject.SetActive(false);
            }
        }
    }

    public void SetSelectedItem(ItemData itemData)
    {
        if (selectedIndex < 0 || selectedIndex >= slots.Length)
            return;

        slots[selectedIndex].SetItem(itemData);
        OnInventoryChanged?.Invoke();
    }

    public ItemData GetSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= slots.Length)
            return null;

        return slots[selectedIndex].itemData;
    }

    public void SetSlotGrabbable(int index, ObjectGrabbable obj)
    {
        if (index < 0 || index >= slotGrabbables.Length)
            return;

        slotGrabbables[index] = obj;
        UpdateSlotObjects();
    }

    public ObjectGrabbable GetSlotGrabbable(int index)
    {
        if (index < 0 || index >= slotGrabbables.Length)
            return null;

        return slotGrabbables[index];
    }

    public void ClearSelectedSlot()
    {
        if (selectedIndex < 0 || selectedIndex >= slots.Length)
            return;

        slotGrabbables[selectedIndex] = null;
        slots[selectedIndex].Clear();

        OnInventoryChanged?.Invoke();
        UpdateSlotObjects();
    }

    public ItemData RemoveSelectedItem()
    {
        if (selectedIndex < 0 || selectedIndex >= slots.Length)
            return null;

        ItemData item = slots[selectedIndex].itemData;

        slotGrabbables[selectedIndex] = null;
        slots[selectedIndex].Clear();

        OnInventoryChanged?.Invoke();
        UpdateSlotObjects();

        return item;
    }
}