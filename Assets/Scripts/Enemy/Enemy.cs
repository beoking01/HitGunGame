using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private EnemyHealth enemyHealth;
    [SerializeField] private GameObject player;
    public EnemyHealth EnemyHealth { get => enemyHealth; }
    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    [SerializeField] private EnemyData enemyData;
    private Vector3 lastKnowPos;
    public Vector3 LastKnowPos { get => lastKnowPos; set => lastKnowPos = value; }
    [Header("Sight Value")]
    public PatrolPath path;
    public float eyesHeight;
    [SerializeField] float sightDistance = 20f; // Distance of enemy can detect player
    [SerializeField] float fieldOfView = 85f;   // Angle at which enemis can detect the player
    [Header("Weapoon Value")]
    public Transform gunBarrel;                 // This position spwan bullet
    public float fireRate;
    public float damage;
    public BulletPools pools;
    [SerializeField] private string currentState;
    private EnemyAbility[] enemyAbilities;
    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        enemyHealth = GetComponent<EnemyHealth>();
        stateMachine.Initialise();

        enemyHealth.MaxHealth(enemyData.maxHealth);
        fireRate = enemyData.fireRate;
        damage = enemyData.damage;

        if (enemyData.isImmortal) gameObject.AddComponent<ImmortalAbility>().Initialise(this);
        
        enemyAbilities = GetComponents<EnemyAbility>();
        foreach (var ab in enemyAbilities)
            ab.Initialise(this);
    }

    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
        foreach (var ability in enemyAbilities)
            ability.Perform();
        if (enemyHealth.IsDeath()) Destroy(gameObject);
    }
    public bool CanSeePlayer()
    {
        if (player != null)
        {
            // is the player close enough to be seen?
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                // Vector enemy toward player 
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyesHeight);

                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward); // Góc giữa mặt hướng của enemy, enemy to player
                // Giới hạn góc nhìn của enemy -85 - 85
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    // Kẻ đường Ray từ Enemy đến Player
                    Ray ray = new Ray(transform.position + (Vector3.up * eyesHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    // Nếu Player nằm trong khoảng sightDistance và là Player thì kẻ Ray đến gameObject
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

}
