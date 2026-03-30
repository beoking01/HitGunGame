using System.Collections.Generic;
using UnityEngine;

public interface IPointObserver
{
    void OnPointsChanged(float newPoints);
}

public class PointManager : MonoBehaviour
{
    public static PointManager Instance { get; private set; }

    private readonly List<IPointObserver> observers = new List<IPointObserver>();
    [SerializeField] private float points = 0f;

    public float Points => points;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        points += amount;
        NotifyObservers();
    }

    public void SetPoints(float newPoints)
    {
        points = newPoints;
        NotifyObservers();
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

            observers[i].OnPointsChanged(points);
        }
    }
    public float GetPoints() { return points; }
}