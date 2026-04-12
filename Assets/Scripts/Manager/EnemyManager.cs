using System.Collections.Generic;
using UnityEngine;

public interface IEnemyObserver
{
    void OnTimeExpired(Vector3 playerPosition);
}

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private readonly List<IEnemyObserver> observers = new List<IEnemyObserver>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void Subscribe(IEnemyObserver observer)
    {
        if (observer == null)
            return;

        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void Unsubscribe(IEnemyObserver observer)
    {
        if (observer == null)
            return;

        observers.Remove(observer);
    }

    public void NotifyTimeExpired(Vector3 playerPosition)
    {
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            if (observers[i] == null)
            {
                observers.RemoveAt(i);
                continue;
            }

            observers[i].OnTimeExpired(playerPosition);
        }
    }
}
