using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthPack : MonoBehaviour
{
    [Header("Health Pack Settings")]
    [SerializeField] private float healthAmount = 20f; // Amount of health restored by the pack
    [SerializeField] private float respawnTime = 10f; // Time before the health pack respawns
    public bool isAvailable = true; // Indicates if the health pack is available for pickup
    private Collider healtCollider;

    [Header("Visuals")]
    [SerializeField] private GameObject healthPackModel; // Model of the health pack
    [SerializeField] private GameObject RespawnObj;
    [SerializeField] private Image respawnClock;

    private void Start()
    {
        healtCollider = GetComponent<Collider>();

        HealthPackManager.Instance.RegisterHealthPack(this);
    }

    private void OnDestroy()
    {
        HealthPackManager.Instance.UnregisterHealthPack(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Health>(out var health) && isAvailable)
        {
            if (health != null && health.CurrentHealth != health.maxHealth)
            {
                health.Heal(healthAmount);

                SetHealtPackActive(false);

                StartCoroutine(RespawnHealthPack());
            }
        }
    }

    private void SetHealtPackActive(bool active)
    {
        isAvailable = active;
        healthPackModel.SetActive(active);
        healtCollider.enabled = active;
    }

    private IEnumerator RespawnHealthPack()
    {
        float elapsedTime = 0f;

        RespawnObj.SetActive(true);
        respawnClock.fillAmount = 0f; // Reset the fill amount

        while (elapsedTime < respawnTime)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Clamp(elapsedTime, 0f, respawnTime);

            respawnClock.fillAmount = elapsedTime / respawnTime; // Update the fill amount
            yield return null; // Wait for the next frame
        }

        RespawnObj.SetActive(false);
        SetHealtPackActive(true);
    }
}
