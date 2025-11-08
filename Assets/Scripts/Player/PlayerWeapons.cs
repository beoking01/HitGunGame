using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public GunData data;
    public Transform gunBarrel;
    public BulletPools pools;
    private float bulletInMagazine;
    private float bulletInBag;
    private float nextFireTime = 0f;
    private bool canShoot = true;
    private float reloadTimer = 3;
    [SerializeField] private TextMeshProUGUI bulletDisplay;

    void Awake()
    {
        bulletInMagazine = data.magazine;
        bulletInBag = data.bagAmountSize;
        bulletDisplay.text = $"{(int)bulletInMagazine}/{(int)bulletInBag}";
        reloadTimer = data.reloadTimer;
    }
    void Update()
    {
        bulletDisplay.text = $"{(int)bulletInMagazine}/{(int)bulletInBag}";
    }
    public void Shoot()
    {
        if (!canShoot || Time.time < nextFireTime) return;
        if (bulletInMagazine > 0)
        {
            SoundManager.Instance.PlaySound("Fire");
            bulletInMagazine--;
            float delay = data.fireRate > 0f ? 1f / data.fireRate : 0.2f;
            nextFireTime = Time.time + delay;

            for (int i = 0; i < data.bulletsPershot; i++)
            {
                Vector3 dir = GetDirectionWithSpread();

                GameObject bullet = pools.GetBullet();
                bullet.GetComponent<Bullet>().damage = data.damge;
                bullet.transform.position = gunBarrel.position;
                bullet.transform.rotation = Quaternion.LookRotation(dir);
                bullet.SetActive(true);

                var rb = bullet.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = dir * data.bulletSpeed;
                }
                var bLife = bullet.GetComponent<Bullet>();
                if (bLife != null)
                    bLife.lifeTime = data.bulletLife;
            }
        }
        else
        {
            SoundManager.Instance.PlaySound("EmptyBullet");
        }
    }
    public Vector3 GetDirectionWithSpread()
    {
        if (data.spreadAngle <= 0f)
            return gunBarrel.forward;

        Quaternion spreadRot = Quaternion.Euler(
            Random.Range(-data.spreadAngle, data.spreadAngle),
            Random.Range(-data.spreadAngle, data.spreadAngle),
            0f
        );
        return spreadRot * gunBarrel.forward;
    }
    public void ReloadBullet()
    {
        if (bulletInMagazine == data.magazine || !canShoot)
            return;
        if (bulletInMagazine == 0)
            SoundManager.Instance.PlaySound("ReloadEmpty");
        else
            SoundManager.Instance.PlaySound("Reload");
        canShoot = false;
        StartCoroutine(WaitReload());
    }
    IEnumerator WaitReload()
    {
        yield return new WaitForSeconds(reloadTimer);
        canShoot = true;
        float bulletCanReload = data.magazine - bulletInMagazine;
        if (bulletCanReload > bulletInBag)
        {
            bulletInMagazine += bulletInBag;
        }
        else
        {
            bulletInBag -= bulletCanReload;
            bulletInMagazine += bulletCanReload;
        }
    }
}