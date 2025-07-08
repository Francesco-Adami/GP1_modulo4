using UnityEngine;

public class Combat : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint; // Punto da cui vengono sparati i proiettili
    public float fireRate;

    private void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;

        if (fireRate > 0)
        {
            fireRate -= Time.deltaTime;
            if (fireRate < 0) fireRate = 0; // Assicurati che non vada sotto zero
        }
    }

    public void Fire()
    {
        if (bullet == null || firePoint == null) return;
        if (fireRate > 0) return;

        // Istanzia un proiettile
        GameObject newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        fireRate = GameManager.Instance.FireRate;
    }
}
