using System.Collections;
using UnityEngine;

public interface IAmmoDisplaySource
{
    bool TryGetAmmoDisplay(out int bulletInMagazine, out int bulletInBag);
}

public class GunWeaponAction : MonoBehaviour, IWeaponAction, IReloadableAction, IAmmoDisplaySource
{
    [SerializeField] private WorldGun worldGun;
    [SerializeField] private BulletPools pools;

    private float bulletInMagazine;
    private float bulletInBag;
    private float nextFireTime;
    private bool canShoot = true;
    private float reloadTimer = 3f;

    private void Awake()
    {
        if (worldGun == null)
            worldGun = GetComponent<WorldGun>();

        if (pools == null)
            pools = GetComponentInChildren<BulletPools>(true);

        GunData data = worldGun != null ? worldGun.Data : null;
        if (data == null)
            return;

        bulletInMagazine = data.magazine;
        bulletInBag = data.bagAmountSize;
        reloadTimer = data.reloadTimer;

        if (pools != null && pools.bulletPrefab == null && data.bulletPreFab != null)
            pools.bulletPrefab = data.bulletPreFab;
    }

    public void PrimaryAttack(GameObject user)
    {
        GunData data = worldGun != null ? worldGun.Data : null;
        Transform gunBarrel = worldGun != null ? worldGun.GunBarrel : null;
        if (data == null || gunBarrel == null || pools == null)
            return;

        if (!canShoot || Time.time < nextFireTime)
            return;

        if (bulletInMagazine <= 0f)
        {
            SoundManager.Instance?.PlaySound("EmptyBullet");
            return;
        }

        SoundManager.Instance?.PlaySound("Fire");
        bulletInMagazine--;

        float delay = data.fireRate > 0f ? 1f / data.fireRate : 0.2f;
        nextFireTime = Time.time + delay;

        for (int i = 0; i < data.bulletsPershot; i++)
        {
            Vector3 shootDirection = GetDirectionWithSpread(data, gunBarrel, user);
            GameObject bullet = pools.GetBullet();
            if (bullet == null)
                return;

            Bullet bulletComponent = bullet.GetComponent<Bullet>();
            if (bulletComponent != null)
            {
                bulletComponent.damage = data.damge;
                bulletComponent.lifeTime = data.bulletLife;
            }

            bullet.transform.position = gunBarrel.position;
            bullet.transform.rotation = Quaternion.LookRotation(shootDirection);
            bullet.SetActive(true);

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
                bulletRigidbody.linearVelocity = shootDirection * data.bulletSpeed;
        }

        UpdateAmmoDisplay(user);
    }

    public void Reload(GameObject user)
    {
        GunData data = worldGun != null ? worldGun.Data : null;
        if (data == null || !canShoot)
            return;

        if (bulletInMagazine >= data.magazine || bulletInBag <= 0f)
            return;

        if (bulletInMagazine <= 0f)
            SoundManager.Instance?.PlaySound("ReloadEmpty");
        else
            SoundManager.Instance?.PlaySound("Reload");

        canShoot = false;
        UpdateAmmoDisplay(user);
        StartCoroutine(WaitReload(data, user));
    }

    private IEnumerator WaitReload(GunData data, GameObject user)
    {
        yield return new WaitForSeconds(reloadTimer);

        float need = data.magazine - bulletInMagazine;
        float amount = Mathf.Min(need, bulletInBag);

        bulletInMagazine += amount;
        bulletInBag -= amount;
        canShoot = true;
        UpdateAmmoDisplay(user);
    }

    public bool TryGetAmmoDisplay(out int magazine, out int bag)
    {
        magazine = Mathf.Max(0, Mathf.FloorToInt(bulletInMagazine));
        bag = Mathf.Max(0, Mathf.FloorToInt(bulletInBag));
        return worldGun != null && worldGun.Data != null;
    }

    private void UpdateAmmoDisplay(GameObject user)
    {
        PlayerUI playerUI = user != null ? user.GetComponent<PlayerUI>() : null;
        if (playerUI == null)
            return;

        
    }

    private Vector3 GetDirectionWithSpread(GunData data, Transform gunBarrel, GameObject user)
    {
        Vector3 baseDirection = GetAimDirection(gunBarrel, user);

        if (data.spreadAngle <= 0f)
            return baseDirection;

        Quaternion spreadRotation = Quaternion.Euler(
            Random.Range(-data.spreadAngle, data.spreadAngle),
            Random.Range(-data.spreadAngle, data.spreadAngle),
            0f
        );

        return spreadRotation * baseDirection;
    }

    private Vector3 GetAimDirection(Transform gunBarrel, GameObject user)
    {
        if (user != null)
        {
            PlayerLook playerLook = user.GetComponent<PlayerLook>();
            if (playerLook != null && playerLook.cam != null)
            {
                Transform camTransform = playerLook.cam.transform;
                Ray aimRay = new Ray(camTransform.position, camTransform.forward);

                if (Physics.Raycast(aimRay, out RaycastHit hitInfo, 500f, ~0, QueryTriggerInteraction.Ignore))
                    return (hitInfo.point - gunBarrel.position).normalized;

                return camTransform.forward.normalized;
            }
        }

        return gunBarrel.forward.normalized;
    }
}
