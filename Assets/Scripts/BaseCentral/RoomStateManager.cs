using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[DefaultExecutionOrder(-500)]
public class RoomStateManager : MonoBehaviour
{
    public static RoomStateManager Instance { get; private set; }

    [SerializeField] private RoomState roomState = new RoomState();

    [Header("Optional Disk Persistence")]
    [SerializeField] private bool loadFromDiskOnAwake = true;
    [SerializeField] private bool saveOnApplicationQuit = true;
    [SerializeField] private string saveFileName = "room_state.json";

    public RoomState State => roomState;

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

    public void UpsertFromRoom(LootItem lootItem, Transform roomRoot)
    {
        if (lootItem == null || roomRoot == null)
            return;

        lootItem.EnsureCargoItemId();
        if (!lootItem.HasValidItemId)
            return;

        RoomItemState state = new RoomItemState
        {
            placementId = lootItem.CargoItemId,
            itemId = lootItem.ItemId,
            localPosition = roomRoot.InverseTransformPoint(lootItem.transform.position),
            localRotation = Quaternion.Inverse(roomRoot.rotation) * lootItem.transform.rotation,
            localScale = lootItem.transform.localScale
        };

        roomState.Upsert(state);
    }

    public void CaptureSnapshot(Transform roomRoot)
    {
        if (roomRoot == null)
            return;

        roomState.Clear();

        for (int i = 0; i < roomRoot.childCount; i++)
        {
            Transform child = roomRoot.GetChild(i);
            LootItem lootItem = child.GetComponent<LootItem>();
            if (lootItem == null || !lootItem.HasValidItemId)
                continue;

            lootItem.EnsureCargoItemId();

            RoomItemState state = new RoomItemState
            {
                placementId = lootItem.CargoItemId,
                itemId = lootItem.ItemId,
                localPosition = child.localPosition,
                localRotation = child.localRotation,
                localScale = child.localScale
            };

            roomState.Upsert(state);
        }
    }

    public bool RemoveByPlacementId(string placementId)
    {
        return roomState.RemoveByPlacementId(placementId);
    }

    public void SaveToDisk()
    {
        RoomSavePayload payload = new RoomSavePayload
        {
            items = roomState.CopyAll()
        };

        string json = JsonUtility.ToJson(payload, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Saving RoomState to disk at " + SavePath + " with " + payload.items.Count + " items.");
    }

    public void LoadFromDisk()
    {
        if (!File.Exists(SavePath))
            return;

        string json = File.ReadAllText(SavePath);
        if (string.IsNullOrEmpty(json))
            return;

        RoomSavePayload payload = JsonUtility.FromJson<RoomSavePayload>(json);

        roomState.Clear();

        if (payload == null || payload.items == null)
            return;

        for (int i = 0; i < payload.items.Count; i++)
        {
            roomState.Upsert(payload.items[i]);
        }

        Debug.Log("Loading RoomState from disk at " + SavePath + " with " + payload.items.Count + " items.");
    }

    private void OnApplicationQuit()
    {
        if (saveOnApplicationQuit)
        {
            SaveToDisk();
        }
    }

    [Serializable]
    private class RoomSavePayload
    {
        public List<RoomItemState> items = new List<RoomItemState>();
    }
}