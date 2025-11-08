using UnityEngine;

public class ImmortalAbility : EnemyAbility
{
    private bool trigger = false;
    private float timer = 0;
    public override void Perform()
    {
        if (enemy.EnemyHealth == null)
        {
            Debug.Log("Is nUll");
        }
        if (!trigger && enemy.EnemyHealth.IsHealth() <= 50)
            {
                trigger = true;
                timer = 0;
                enemy.EnemyHealth.IsGiveDamege(false);
            }
        if (trigger)
        {
            timer += Time.deltaTime;
            if (timer >= 5)
            {
                enemy.EnemyHealth.IsGiveDamege(true);
            }
        }
    }
}
