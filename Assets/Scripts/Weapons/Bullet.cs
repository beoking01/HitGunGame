using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;
    public float damage = 30f;
    private Rigidbody rb;
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        Invoke(nameof(DisableSelf), lifeTime);
    }
    void OnDisable()
    {
        CancelInvoke();
        if (rb) rb.linearVelocity = Vector3.zero;
    }
    void DisableSelf()
    {
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<HealthPlayer>().TakeDamage(10);
            DisableSelf();
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponentInParent<EnemyHealth>().TakeDamage(damage);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    IEnumerator DisableDelay()
    {
        yield return null;
        DisableSelf();
    }
}
