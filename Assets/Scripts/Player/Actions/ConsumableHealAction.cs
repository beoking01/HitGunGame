using UnityEngine;

public class ConsumableHealAction : MonoBehaviour, IConsumableAction
{
    [SerializeField] private float healAmountOverride = -1f;

    public bool Consume(GameObject user)
    {
        if (user == null)
            return false;

        HealthPlayer healthPlayer = user.GetComponent<HealthPlayer>();
        if (healthPlayer == null)
            return false;

        float healAmount = healAmountOverride > 0f ? healAmountOverride : GetDefaultHealAmount();
        if (healAmount <= 0f)
            return false;

        healthPlayer.RestoreHealth(healAmount);
        return true;
    }

    private float GetDefaultHealAmount()
    {
        WorldItem worldItem = GetComponent<WorldItem>();
        if (worldItem != null && worldItem.itemData != null)
            return worldItem.itemData.healAmount;

        return 0f;
    }
}
