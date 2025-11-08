using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Guns/GunData")]
public class GunData : ScriptableObject
{
    [Tooltip("Bullet prefab (must have Bullet script & Rigitbody)")]
    public GameObject bulletPreFab;

    [Tooltip("Shots per second (ex: 5 shot/sec)")]
    public float fireRate = 5f;

    [Tooltip("Number of projectiles per shot ")]
    public int bulletsPershot = 1;

    [Tooltip("Max spread angle (degrees)")]
    public float spreadAngle = 5f;
    public float bulletSpeed = 20f;
    public float bulletLife = 2f;
    public LayerMask hitMask = ~0;
    [Header("Damage")]
    public float damge = 10f;
    [Header("Infor Amount")]
    public float bagAmountSize = 120;
    public float magazine = 30;
    public float reloadTimer = 3;
}
