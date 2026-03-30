using UnityEngine;

public class WorldGun : MonoBehaviour
{
    [SerializeField] private GunData data;
    [SerializeField] private Transform gunBarrel;

    public GunData Data => data;
    public Transform GunBarrel => gunBarrel;
}