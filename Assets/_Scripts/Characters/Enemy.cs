using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [Header("Enemy Components")]
    [SerializeField] private AIBehaviour AIBehaviour;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;

    [SerializeField] private Canvas enemyCanvas;

    protected override void Start()
    {
        base.Start();

        health.OnDeath = null; // Reset to avoid multiple subscriptions
        health.OnDeath += OnDeathHandle;

        playerTransform = FindAnyObjectByType<Player>().transform;

        AIBehaviour.SetState(new StateSelector(agent, playerTransform, health, combat));
    }

    private void Update()
    {
        if (gameManager.IsGamePaused) return;

        if (enemyCanvas.transform.rotation != Camera.main.transform.rotation)
            enemyCanvas.transform.rotation = Camera.main.transform.rotation;
    }

    private void OnDeathHandle()
    {
        Debug.Log("Player has died.");
        FindAnyObjectByType<Flag>().IsEnemyCapturing = false;
        AIBehaviour.SetState(new StateSelector(agent, playerTransform, health, combat));

        StartCoroutine(gameManager.RespawnRoutine(this));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Flag"))
        {
            flag.IsEnemyCapturing = true;

            if (flag.IsPlayerCapturing) return;
            flag.EnemyCaptureProgress += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Flag"))
        {
            flag.IsEnemyCapturing = false;
        }
    }
}
