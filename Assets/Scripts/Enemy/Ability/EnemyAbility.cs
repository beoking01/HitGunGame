using UnityEngine;

public abstract class EnemyAbility : MonoBehaviour
{
    protected Enemy enemy;
    public virtual void Initialise(Enemy e)
    {
        enemy = e;
    }
    public abstract void Perform();
}
