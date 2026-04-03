using UnityEngine;
using TMPro;
// process UI displayed in player screen
public class PlayerUI : MonoBehaviour,IPointObserver
{

    [SerializeField]
    private TextMeshProUGUI promtText;
    [SerializeField]
    private TextMeshProUGUI ammoText;
    public TextMeshProUGUI pointText;
    private PlayerInventory playerInventory;
    [SerializeField] private InventorySystem inventorySystem;
    private bool inventoryEventsSubscribed;

    private void Awake()
    {
        ResolveInventorySystemReference();
    }

    void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
        ResolveInventorySystemReference();

        // Đăng ký observer
        PointManager.Instance.AddObserver(this);
        // Cập nhật UI ban đầu
        OnPointsChanged(PointManager.Instance.GetPoints());

        SubscribeInventoryEvents();

        RefreshAmmoDisplayFromSelectedItem();
    }

    void Update()
    {
    }
    public void UpdateText(string promtMessage)
    {
        promtText.text = promtMessage;
    }


    private void OnDestroy()
    {
        // Hủy đăng ký
        if (PointManager.Instance != null)
        {
            PointManager.Instance.RemoveObserver(this);
        }

        if (inventorySystem != null && inventoryEventsSubscribed)
        {
            inventorySystem.OnSelectedSlotChanged -= OnSelectedSlotChanged;
            inventorySystem.OnInventoryChanged -= OnInventoryChanged;
            inventoryEventsSubscribed = false;
        }
    }

    public void OnPointsChanged(float newPoints)
    {
        if (pointText != null)
        {
            pointText.text =  newPoints.ToString("F0");
        }
    }

    public void SetAmmoDisplay(int bulletInMagazine, int bulletInBag)
    {
        if (ammoText == null)
            return;

        ammoText.gameObject.SetActive(true);
        ammoText.text = $"{bulletInMagazine}/{bulletInBag}";
    }

    public void HideAmmoDisplay()
    {
        if (ammoText == null)
            return;

        ammoText.text = string.Empty;
        ammoText.gameObject.SetActive(false);
    }

    public void RefreshAmmoDisplayFromSelectedItem()
    {
        if (inventorySystem == null)
        {
            ResolveInventorySystemReference();
            SubscribeInventoryEvents();
        }

        if (inventorySystem == null)
        {
            HideAmmoDisplay();
            return;
        }

        ItemData selectedItem = inventorySystem.GetSelectedItem();
        if (selectedItem == null || selectedItem.itemType != ItemData.ItemType.Weapon)
        {
            HideAmmoDisplay();
            return;
        }

        ObjectGrabbable selectedObject = inventorySystem.GetSlotGrabbable(inventorySystem.selectedIndex);
        if (selectedObject == null)
        {
            HideAmmoDisplay();
            return;
        }

        IAmmoDisplaySource ammoSource = ResolveAmmoSource(selectedObject);
        if (ammoSource != null && ammoSource.TryGetAmmoDisplay(out int bulletInMagazine, out int bulletInBag))
            SetAmmoDisplay(bulletInMagazine, bulletInBag);
        else
            HideAmmoDisplay();
    }

    private void OnSelectedSlotChanged(int _)
    {
        RefreshAmmoDisplayFromSelectedItem();
    }

    private void OnInventoryChanged()
    {
        RefreshAmmoDisplayFromSelectedItem();
    }

    private IAmmoDisplaySource ResolveAmmoSource(ObjectGrabbable selectedObject)
    {
        IAmmoDisplaySource ammoSource = selectedObject.GetComponent<IAmmoDisplaySource>();
        if (ammoSource != null)
            return ammoSource;

        ammoSource = selectedObject.GetComponentInChildren<IAmmoDisplaySource>(true);
        if (ammoSource != null)
            return ammoSource;

        return selectedObject.GetComponentInParent<IAmmoDisplaySource>();
    }

    private void ResolveInventorySystemReference()
    {
        if (inventorySystem != null)
            return;

        inventorySystem = GetComponent<InventorySystem>();
        if (inventorySystem != null)
            return;

        inventorySystem = GetComponentInParent<InventorySystem>();
        if (inventorySystem != null)
            return;

        inventorySystem = GetComponentInChildren<InventorySystem>(true);
        if (inventorySystem != null)
            return;

        inventorySystem = FindFirstObjectByType<InventorySystem>();
    }

    private void SubscribeInventoryEvents()
    {
        if (inventorySystem == null || inventoryEventsSubscribed)
            return;

        inventorySystem.OnSelectedSlotChanged += OnSelectedSlotChanged;
        inventorySystem.OnInventoryChanged += OnInventoryChanged;
        inventoryEventsSubscribed = true;
    }
}
