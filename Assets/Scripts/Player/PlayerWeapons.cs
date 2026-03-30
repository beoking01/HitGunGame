using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour{
    public GunData data;
    public Transform gunBarrel;
    public BulletPools pools;

    private float bulletInMagazine;
    private float bulletInBag;
    private float nextFireTime;
    private bool canShoot = true;
    private float reloadTimer = 3f;

    [SerializeField] private TextMeshProUGUI bulletDisplay;

    private void Awake()
    {
        if (data == null)
            return;

        bulletInMagazine = data.magazine;
        bulletInBag = data.bagAmountSize;
        reloadTimer = data.reloadTimer;
        UpdateBulletDisplay();
    }

    private void Update()
    {
        UpdateBulletDisplay();
    }

    public void Shoot()
    {
        if (data == null || gunBarrel == null || pools == null)
            return;

        if (!canShoot || Time.time < nextFireTime)
            return;

        if (bulletInMagazine <= 0)
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
            Vector3 dir = GetDirectionWithSpread();

            GameObject bullet = pools.GetBullet();
            Bullet bulletComp = bullet.GetComponent<Bullet>();
            if (bulletComp != null)
            {
                bulletComp.damage = data.damge;
                bulletComp.lifeTime = data.bulletLife;
            }

            bullet.transform.position = gunBarrel.position;
            bullet.transform.rotation = Quaternion.LookRotation(dir);
            bullet.SetActive(true);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = dir * data.bulletSpeed;
            }
        }
    }

    public Vector3 GetDirectionWithSpread()
    {
        if (data == null || data.spreadAngle <= 0f)
            return gunBarrel != null ? gunBarrel.forward : transform.forward;

        Quaternion spreadRot = Quaternion.Euler(
            Random.Range(-data.spreadAngle, data.spreadAngle),
            Random.Range(-data.spreadAngle, data.spreadAngle),
            0f
        );
        return spreadRot * gunBarrel.forward;
    }

    public void Reload()
    {
        if (data == null || !canShoot)
            return;

        if (bulletInMagazine >= data.magazine || bulletInBag <= 0)
            return;

        if (bulletInMagazine <= 0)
            SoundManager.Instance?.PlaySound("ReloadEmpty");
        else
            SoundManager.Instance?.PlaySound("Reload");

        canShoot = false;
        StartCoroutine(WaitReload());
    }

    private IEnumerator WaitReload()
    {
        yield return new WaitForSeconds(reloadTimer);
        canShoot = true;

        float need = data.magazine - bulletInMagazine;
        float amount = Mathf.Min(need, bulletInBag);

        bulletInMagazine += amount;
        bulletInBag -= amount;
    }

    private void UpdateBulletDisplay()
    {
        if (bulletDisplay != null)
            bulletDisplay.text = $"{(int)bulletInMagazine}/{(int)bulletInBag}";
    }
}
