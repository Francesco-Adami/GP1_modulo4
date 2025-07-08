using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Player,
        Enemy
    }

    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private BulletType bulletType;


    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing on the bullet.");
        }
    }

    private void Start()
    {
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Log the collision for debugging
        //Debug.Log($"Bullet collided with: {collision.gameObject.name}");

        if (collision.gameObject.CompareTag("Enemy") && bulletType == BulletType.Player)
        {
            // Assuming the enemy has a Health component
            if (collision.gameObject.TryGetComponent<Health>(out var enemyHealth))
            {
                enemyHealth.TakeDamage(damage); // Adjust damage as needed
            }
        }
        else if (collision.gameObject.CompareTag("Player") && bulletType == BulletType.Enemy)
        {
            // Assuming the player has a Health component
            if (collision.gameObject.TryGetComponent<Health>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage); // Adjust damage as needed
            }
        }

        Destroy(gameObject);
    }
}
