using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GoToFlag : State
{
    public GoToFlag(NavMeshAgent _agent, Transform _player, Health health, Combat combat) : base(_agent, _player, health, combat)
    {
    }

    Flag flag;

    public override void OnEnterState()
    {
        base.OnEnterState();
        flag = GameObject.FindAnyObjectByType<Flag>(FindObjectsInactive.Include);
    }

    public override void OnUpdateState()
    {
        Debug.Log("GOING TO FLAG");

        base.OnUpdateState();

        if (CanSeePlayer() || flag.IsPlayerCapturing)
        {
            nextState = new AttackPlayer(agent, player, Health, Combat);
            Phase = StatePhase.EXIT;
            return;
        }

        if (flag.ZoneCaptured == ZoneCaptured.Enemy)
        {
            nextState = new SeekCover(agent, player, Health, Combat);
            Phase = StatePhase.EXIT;
        }

        if (flag != null && !agent.hasPath)
        {
            Vector3 randomPoint = GetRandomPointInCollider(flag.GetComponent<Collider>());

            agent.isStopped = false;
            agent.SetDestination(randomPoint);
            return;
        }

        if (flag.IsEnemyCapturing && !agent.hasPath)
        {
            // sono dentro la zona
            agent.ResetPath();

            nextState = new StateSelector(agent, player, Health, Combat);
            Phase = StatePhase.EXIT;
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }

    private Vector3 GetRandomPointInCollider(Collider collider)
    {
        Vector3 randomPoint = Vector3.zero;
        NavMeshHit hit;

        // Ottieni i bounds del collider
        Bounds bounds = collider.bounds;

        // Prova fino a 30 volte per trovare un punto valido
        for (int i = 0; i < 30; i++)
        {
            // Genera un punto random all'interno dei bounds
            Vector3 randomPos = new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                bounds.center.y,
                Random.Range(bounds.min.z, bounds.max.z)
            );

            // Verifica se il punto è effettivamente all'interno del collider
            if (IsPointInsideCollider(randomPos, collider))
            {
                // Cerca il punto più vicino sulla NavMesh
                if (NavMesh.SamplePosition(randomPos, out hit, 5f, NavMesh.AllAreas))
                {
                    randomPoint = hit.position;
                    break;
                }
            }
        }

        // Se non trova un punto valido, usa la posizione del flag
        if (randomPoint == Vector3.zero)
        {
            randomPoint = flag.transform.position;
        }

        return randomPoint;
    }

    private bool IsPointInsideCollider(Vector3 point, Collider collider)
    {
        // Metodo più preciso per verificare se un punto è dentro un collider
        return collider.bounds.Contains(point) &&
               Vector3.Distance(point, collider.ClosestPoint(point)) < 0.01f;
    }
}