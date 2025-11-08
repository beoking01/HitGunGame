using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private float money = 0;
    public void AddMoney(float Money)
    {
        money += Money;
    }
    public float showMoney()
    {
        return money;
    }
}
