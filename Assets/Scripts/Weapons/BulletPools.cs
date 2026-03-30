using UnityEngine;
using System.Collections.Generic;

public class BulletPools : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int initiaSize = 30;
    private List<GameObject> pool = new List<GameObject>();
    void Awake()
    {
        for (int i = 0; i < initiaSize; i++)
        {
            var b = Instantiate(bulletPrefab, transform);
            b.SetActive(false);
            pool.Add(b);
        }
    }
    public GameObject GetBullet()
    {
        for (int i = 0; i < pool.Count; i++)
            if (!pool[i].activeInHierarchy) return pool[i];

        var nb = Instantiate(bulletPrefab, transform);
        nb.SetActive(false);
        pool.Add(nb);
        return nb;
    }
}
