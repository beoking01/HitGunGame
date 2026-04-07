using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[DefaultExecutionOrder(-500)]
public class TruckStateManager : MonoBehaviour
{
    public static TruckStateManager Instance { get; private set; }

    [SerializeField] private TruckState truckState = new TruckState();

    [Header("Optional Disk Persistence")]
    [SerializeField] private bool loadFromDiskOnAwake;
    [SerializeField] private bool saveOnApplicationQuit;
    [SerializeField] private string saveFileName = "truck_state.json";

    public TruckState State => truckState;

    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (loadFromDiskOnAwake)
        {
            LoadFromDisk();
        }
    }

    public void UpsertFromCargo(LootItem lootItem, Transform cargoRoot)
    {
        if (lootItem == null || cargoRoot == null)
            return;

        bool isChildOfCargo = lootItem.transform.IsChildOf(cargoRoot);

        TruckItemState state = new TruckItemState
        {
            cargoItemId = lootItem.CargoItemId,
            itemId = lootItem.ItemId,
            localPosition = isChildOfCargo ? lootItem.transform.localPosition : cargoRoot.InverseTransformPoint(lootItem.transform.position),
            localRotation = isChildOfCargo ? lootItem.transform.localRotation : Quaternion.Inverse(cargoRoot.rotation) * lootItem.transform.rotation,
            localScale = lootItem.transform.localScale
        };

        truckState.Upsert(state);
    }

    public void CaptureSnapshot(Transform cargoRoot)
    {
        if (cargoRoot == null)
            return;

        truckState.Clear();

        for (int i = 0; i < cargoRoot.childCount; i++)
        {
            Transform child = cargoRoot.GetChild(i);
            LootItem lootItem = child.GetComponent<LootItem>();
            if (lootItem == null || !lootItem.HasValidItemId)
                continue;

            TruckItemState state = new TruckItemState
            {
                cargoItemId = lootItem.CargoItemId,
                itemId = lootItem.ItemId,
                localPosition = child.localPosition,
                localRotation = child.localRotation,
                localScale = child.localScale
            };

            truckState.Upsert(state);
        }
    }

    public bool RemoveByCargoId(string cargoItemId)
    {
        return truckState.RemoveByCargoId(cargoItemId);
    }

    public void SaveToDisk()
    {
        CaptureRuntimeSnapshotIfPossible();

        TruckSavePayload payload = new TruckSavePayload
        {
            items = truckState.CopyAll()
        };
        Debug.Log("Saving TruckState to disk at " + SavePath + " with " + payload.items.Count + " items.");

        string json = JsonUtility.ToJson(payload, true);
        File.WriteAllText(SavePath, json);
    }

    public void LoadFromDisk()
    {
        if (!File.Exists(SavePath))
            return;

        string json = File.ReadAllText(SavePath);
        if (string.IsNullOrEmpty(json))
            return;

        TruckSavePayload payload = JsonUtility.FromJson<TruckSavePayload>(json);

        truckState.Clear();

        if (payload == null || payload.items == null)
            return;

        Debug.Log("Loading TruckState from disk at " + SavePath + " with " + payload.items.Count + " items.");

        for (int i = 0; i < payload.items.Count; i++)
        {
            truckState.Upsert(payload.items[i]);
        }
    }

    private void OnApplicationQuit()
    {
        if (saveOnApplicationQuit)
        {
            SaveToDisk();
        }
    }

    private void CaptureRuntimeSnapshotIfPossible()
    {
        TruckCargoZone cargoZone = FindFirstObjectByType<TruckCargoZone>();
        if (cargoZone != null)
        {
            cargoZone.CaptureSnapshotNow();
        }
    }

    [Serializable]
    private class TruckSavePayload
    {
        public List<TruckItemState> items = new List<TruckItemState>();
    }
}
