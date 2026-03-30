using UnityEngine;
[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float maxHealth;
    public float fireRate;
    public float damage;
    public bool isImmortal = false;
}
