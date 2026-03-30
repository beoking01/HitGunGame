using Mono.Cecil;
using UnityEngine;

public class AttackState : BaseState
{
    private float moveTimer;
    private float losePlayerTimer;
    private float shotTimer;
    public override void Enter()
    {

    }

    public override void Exit()
    {
        
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            moveTimer += Time.deltaTime;
            losePlayerTimer = 0;
            shotTimer += Time.deltaTime;

            Vector3 lookPos = enemy.Player.transform.position - enemy.transform.position;
            lookPos.y = 0; // Giữ y cố định để không bị ngẩng/gục đầu
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, rotation, Time.deltaTime * 5f);
            
            if (shotTimer > enemy.fireRate)
            {
                Shoot();
            }
            if (moveTimer > Random.Range(5, 7))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 1));
                moveTimer = 0;
            }
            enemy.LastKnowPos = enemy.Player.transform.position;
        }
        else // lose sight of player
        {
            losePlayerTimer += Time.deltaTime;
            if (losePlayerTimer > 5)
            {
                // Change to the search state.
                stateMachine.ChangeState(new SearchState());
            }
        }
    }
    public void Shoot()
    {
        SoundManager.Instance.PlaySound("Fire");
        //Store refferent to the gun barrel
        Transform gunBarrel = enemy.gunBarrel;

        // instantiate a new ballet 

        // GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet") as GameObject, gunBarrel.position, enemy.transform.rotation);
        GameObject bullet = enemy.pools.GetBullet();
        bullet.transform.position = gunBarrel.position;
        bullet.transform.rotation = enemy.transform.rotation;
        // caculate the direction to the Player
        Vector3 shootDirection = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;
        // add force rigitbody of the bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = Quaternion.AngleAxis(Random.Range(-3f, 3f), Vector3.up) * shootDirection * 40;
        bullet.SetActive(true);
        shotTimer = 0;
    }
}
