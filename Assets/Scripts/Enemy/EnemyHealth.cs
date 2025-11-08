using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private float health;
    private float lerpTimer;
    private float maxHealth = 100f;
    private float chipSpeed = 2f;
    private bool isDeath = false;
    private bool giveDamege = true;
    [Header("Health UI")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();
        if (health == 0)
        {
            isDeath = true;
        }
    }

    void LateUpdate()
    {
        frontHealthBar.canvas.transform.LookAt(frontHealthBar.canvas.transform.position + Camera.main.transform.forward);
    }

    public void UpdateHealthUI()
    {
        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }
        else if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }

        if (!giveDamege) frontHealthBar.color = Color.yellow;
        else frontHealthBar.color = Color.green;
    }
    public void TakeDamage(float damage)
    {
        if (!giveDamege) return;
        health -= damage;
        lerpTimer = 0f;
    }
    public void Heal(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
        lerpTimer = 0f;
    }
    public bool IsDeath()
    {
        return isDeath;
    }

    public float IsHealth()
    {
        return health;
    }
    public void IsGiveDamege(bool tmp)
    {
        giveDamege = tmp;
    }
    public void MaxHealth(float tmp)
    {
        maxHealth = tmp;
        health = maxHealth;
    }
}
