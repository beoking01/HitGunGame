using UnityEngine;

public class MeleeWeaponAction : MonoBehaviour, IWeaponAction
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float range = 2f;
    [SerializeField] private float radius = 0.35f;
    [SerializeField] private float cooldown = 0.4f;
    [SerializeField] private LayerMask hitMask = ~0;
    [SerializeField] private Transform attackOrigin;

    private float nextAttackTime;

    public void PrimaryAttack(GameObject user)
    {
        if (Time.time < nextAttackTime)
            return;

        nextAttackTime = Time.time + cooldown;

        Transform origin = attackOrigin != null ? attackOrigin : transform;
        if (Physics.SphereCast(origin.position, radius, origin.forward, out RaycastHit hit, range, hitMask, QueryTriggerInteraction.Ignore))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
                enemyHealth.TakeDamage(damage);
        }
    }
}
