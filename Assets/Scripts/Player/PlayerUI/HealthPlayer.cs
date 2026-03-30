using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthPlayer : MonoBehaviour
{
    private float health;
    private float lerpTimer;

    [Header("Stats")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float chipSpeed = 2f;

    [Header("Health UI")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Damage Overlay")]
    [SerializeField] private Image overlay;
    [SerializeField] private float duration = 0.5f;   // how long the image stay full dame
    [SerializeField] private float fadeSpeeds = 1.5f; // how quickly the imgae will fade
    private float durationTimer;

    void Start()
    {
        health = maxHealth;
        healthText.text = $"{(int)health}/{(int)maxHealth}";

        // Set transparent red
        overlay.color = new Color(175f / 255f, 40f / 255f, 20f / 255f, 0f);
    }

    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        // Handle damage overlay fade
        if (overlay.color.a > 0)
        {
            if (health < 30)
                return;
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeeds;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void UpdateHealthUI()
    {
        healthText.text = $"{(int)health}/{(int)maxHealth}";

        float fillF = frontHealthBar.fillAmount;
        float fillB = backHealthBar.fillAmount;
        float hFraction = health / maxHealth;

        // Lerp back bar (red) when taking damage
        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);
        }

        // Lerp front bar (green) when healing
        else if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.green;

            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f;
        durationTimer = 0f;

        // Show red overlay at full alpha
        overlay.color = new Color(175f / 255f, 40f / 255f, 20f / 255f, 0.2f);
        if (health <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    public void RestoreHealth(float heal)
    {
        health += heal;
        health = Mathf.Clamp(health, 0, maxHealth);
        lerpTimer = 0f;
    }
}
