using UnityEngine;
using UnityEngine.AI;

public class SeekCover : State
{
    public SeekCover(NavMeshAgent agent, Transform player, Health health, Combat combat) : base(agent, player, health, combat)
    {
    }

    Flag Flag;
    private bool isInCover = false;
    private float coverTimer = 0f;
    private float coverDuration = 5f; // Resta in copertura per 3 secondi

    public override void OnEnterState()
    {
        base.OnEnterState();
        Debug.Log("Seeking cover");
        Flag = GameObject.FindAnyObjectByType<Flag>(FindObjectsInactive.Include);
        isInCover = false;
        coverTimer = 0f;
    }

    public override void OnUpdateState()
    {
        Debug.Log("SEEKING COVER");

        // Controlla se il player sta catturando la zona
        if (Flag != null && Flag.IsPlayerCapturing)
        {
            nextState = new AttackPlayer(agent, player, Health, Combat);
            Phase = StatePhase.EXIT;
            return;
        }

        if (!isInCover)
        {
            // Cerca copertura dall'altra parte del player
            Vector3 coverPosition = FindCoverPosition();

            if (coverPosition != Vector3.zero)
            {
                agent.SetDestination(coverPosition);

                // Controlla se ha raggiunto la copertura
                if (Vector3.Distance(agent.transform.position, coverPosition) < agent.stoppingDistance)
                {
                    isInCover = true;
                    agent.SetDestination(agent.transform.position); // Fermati
                    Debug.Log("Reached cover position - staying for " + coverDuration + " seconds");
                }
            }
        }
        else
        {
            // Resta in copertura per alcuni secondi
            coverTimer += Time.deltaTime;

            if (coverTimer >= coverDuration)
            {
                Debug.Log("Cover time expired - changing zone");
                nextState = new StateSelector(agent, player, Health, Combat);
                Phase = StatePhase.EXIT;
            }
        }
    }

    public Vector3 FindCoverPosition()
    {
        Vector3 playerPosition = player.position;
        Vector3 flagPosition = Flag.transform.position;
        // Calcola la direzione opposta rispetto al player
        Vector3 oppositeDirection = flagPosition - playerPosition;
        oppositeDirection.Set(oppositeDirection.x, 0f, oppositeDirection.z);
        oppositeDirection = oppositeDirection.normalized;
        //Debug.Log("Opposite direction: " + oppositeDirection);
        Vector3 coverPosition;

        if (Physics.Raycast(flagPosition, oppositeDirection, out RaycastHit hit, 30f))
        {
            Debug.DrawRay(flagPosition, oppositeDirection * 10f, Color.blue, 2f);
            // Primo raycast per il lato più vicino
            Vector3 frontHit = hit.point;
            //Debug.Log("Front hit point: " + frontHit);

            if (Physics.Raycast(frontHit, oppositeDirection, out RaycastHit backHit, 20f))
            {
                Debug.DrawRay(frontHit, oppositeDirection * 20f, Color.yellow, 2f);
                Vector3 backHitPoint = backHit.point;
                //Debug.Log("Back hit point: " + backHitPoint);

                if (NavMesh.SamplePosition(backHitPoint, out NavMeshHit backNavHit, 0.5f, NavMesh.AllAreas) &&
                    IsPathReachable(agent.transform.position, backNavHit.position))
                {
                    coverPosition = backNavHit.position;
                }
                else
                {
                    // Fallback al punto frontale se il punto dietro non è raggiungibile
                    if (NavMesh.SamplePosition(frontHit, out NavMeshHit frontNavHit, 1f, NavMesh.AllAreas) &&
                        IsPathReachable(agent.transform.position, frontNavHit.position))
                    {
                        coverPosition = frontNavHit.position;
                    }
                    else
                    {
                        coverPosition = frontHit;
                    }
                }
            }
            else
            {
                // Fallback se non trova il retro
                if (NavMesh.SamplePosition(frontHit, out NavMeshHit frontNavHit, 1f, NavMesh.AllAreas) &&
                    IsPathReachable(agent.transform.position, frontNavHit.position))
                {
                    coverPosition = frontNavHit.position;
                }
                else
                {
                    coverPosition = frontHit;
                }
            }
            return coverPosition;
        }

        // il raycast non ha trovato nulla
        Debug.LogError("Raycast failed to find cover, returning Vector3.zero");
        return Vector3.zero;
    }

    private bool IsPathReachable(Vector3 startPosition, Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        bool hasPath = NavMesh.CalculatePath(startPosition, targetPosition, NavMesh.AllAreas, path);

        // Verifica se il percorso è completo (raggiunge effettivamente la destinazione)
        return hasPath && path.status == NavMeshPathStatus.PathComplete;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        Debug.Log("Exiting SeekCover state");
    }
}