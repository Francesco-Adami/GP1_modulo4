using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("GameManager")]
    [SerializeField] protected GameManager gameManager;

    [Header("Character Components")]
    public Health health;
    [SerializeField] protected Combat combat;


    protected float characterSpeed;

    protected Flag flag;

    protected virtual void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogError("GameManager instance not found!");
            return;
        }

        health.maxHealth = gameManager.CharacterHealth;
        health.CurrentHealth = gameManager.CharacterHealth;
        characterSpeed = gameManager.CharacterSpeed;

        flag = FindAnyObjectByType<Flag>(FindObjectsInactive.Include);
    }

    protected void FireCooldown()
    {
        if (combat.fireRate == 0) return;

        combat.fireRate -= Time.deltaTime;
        if (combat.fireRate < 0) combat.fireRate = 0;
    }
}
