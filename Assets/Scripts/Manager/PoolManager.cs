using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private Transform poolRoot;
    [SerializeField] private int defaultWarmSize = 20;

    private readonly Dictionary<int, List<GameObject>> pools = new Dictionary<int, List<GameObject>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (poolRoot == null)
            poolRoot = transform;

        DontDestroyOnLoad(gameObject);
    }

    public static PoolManager GetOrCreate()
    {
        if (Instance != null)
            return Instance;

        PoolManager existing = FindFirstObjectByType<PoolManager>();
        if (existing != null)
            return existing;

        GameObject managerObject = new GameObject("PoolManager");
        return managerObject.AddComponent<PoolManager>();
    }

    public void Prewarm(GameObject prefab, int size = -1)
    {
        if (prefab == null)
            return;

        List<GameObject> pool = GetOrCreatePool(prefab);
        int targetSize = size > 0 ? size : defaultWarmSize;

        while (pool.Count < targetSize)
            pool.Add(CreateNew(prefab));
    }

    public GameObject Get(GameObject prefab)
    {
        if (prefab == null)
            return null;

        List<GameObject> pool = GetOrCreatePool(prefab);
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }

        GameObject newObject = CreateNew(prefab);
        pool.Add(newObject);
        return newObject;
    }

    private List<GameObject> GetOrCreatePool(GameObject prefab)
    {
        int prefabKey = prefab.GetInstanceID();
        if (!pools.TryGetValue(prefabKey, out List<GameObject> pool))
        {
            pool = new List<GameObject>();
            pools.Add(prefabKey, pool);
        }

        return pool;
    }

    private GameObject CreateNew(GameObject prefab)
    {
        GameObject spawned = Instantiate(prefab, poolRoot);
        spawned.SetActive(false);
        return spawned;
    }
}
