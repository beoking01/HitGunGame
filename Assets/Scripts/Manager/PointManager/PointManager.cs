using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IPointObserver
{
    void OnPointsChanged(float newPoints);
}

public class PointManager : MonoBehaviour
{
    public static PointManager Instance { get; private set; }

    private readonly List<IPointObserver> observers = new List<IPointObserver>();
    [SerializeField] private float money = 0f;

    [Header("Optional Disk Persistence")]
    [SerializeField] private bool loadFromDiskOnAwake = true;
    [SerializeField] private bool saveOnApplicationQuit = true;
    [SerializeField] private bool saveOnPointsChanged;
    [SerializeField] private string saveFileName = "money_state.json";

    public float Points => money;
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

    public void AddObserver(IPointObserver observer)
    {
        if (observer == null) return;
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(IPointObserver observer)
    {
        if (observer == null) return;
        observers.Remove(observer);
    }

    public void AddPoints(float amount)
    {
        money += amount;
        NotifyObservers();

        if (saveOnPointsChanged)
        {
            SaveToDisk();
        }
    }

    public void SetPoints(float newPoints)
    {
        money = newPoints;
        NotifyObservers();

        if (saveOnPointsChanged)
        {
            SaveToDisk();
        }
    }

    public void SaveToDisk()
    {
        PointSavePayload payload = new PointSavePayload
        {
            money = money
        };

        string json = JsonUtility.ToJson(payload, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("Saving PointState to disk at " + SavePath + " with money " + payload.money);
    }

    public void LoadFromDisk()
    {
        if (!File.Exists(SavePath))
            return;

        string json = File.ReadAllText(SavePath);
        if (string.IsNullOrEmpty(json))
            return;

        PointSavePayload payload = JsonUtility.FromJson<PointSavePayload>(json);
        if (payload == null)
            return;

        money = payload.money;
        NotifyObservers();

        Debug.Log("Loading PointState from disk at " + SavePath + " with money " + money);
    }

    private void NotifyObservers()
    {
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            if (observers[i] == null)
            {
                observers.RemoveAt(i);
                continue;
            }

            observers[i].OnPointsChanged(money);
        }
    }

    private void OnApplicationQuit()
    {
        if (saveOnApplicationQuit)
        {
            SaveToDisk();
        }
    }

    public float GetPoints() { return money; }

    [Serializable]
    private class PointSavePayload
    {
        public float money;
    }
}