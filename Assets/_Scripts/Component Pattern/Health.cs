using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float currentHealth;
    [SerializeField] public float maxHealth;

    [Header("UI Settings")]
    [SerializeField] private Slider healthBarPrefab;

    private void Awake()
    {
        // i nemici avranno già lo slider assegnato dentro il prefab
        if (healthBarPrefab != null) return;

        // solo per il player
        healthBarPrefab = FindAnyObjectByType<HudUI>(FindObjectsInactive.Include).healthBarSlider;
    }

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (value == currentHealth) return;

            currentHealth = Mathf.Clamp(value, 0, maxHealth);

            // GESTISCI UI
            if (healthBarPrefab != null)
            {
                healthBarPrefab.value = currentHealth / maxHealth;
            }
        }
    }

    public System.Action OnDeath;

    public void TakeDamage(float amount)
    {
        if (amount <= 0) return;

        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0) return;

        CurrentHealth += amount;
    }
}
