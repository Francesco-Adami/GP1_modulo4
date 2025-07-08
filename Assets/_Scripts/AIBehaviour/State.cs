using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class State : IStateAI
{
    protected Health Health { get; private set; }
    protected Combat Combat { get; private set; }

    public enum StatePhase
    {
        ENTER, UPDATE, EXIT
    }
    public StatePhase Phase { get; protected set; } = StatePhase.ENTER;

    protected Transform player;
    protected State nextState;
    protected NavMeshAgent agent;

    protected float visionDistance = 15.0f;
    protected float visionAngle = 60.0f;

    public State(NavMeshAgent _agent, Transform _player, Health health, Combat combat)
    {
        agent = _agent;
        player = _player;
        Phase = StatePhase.ENTER;
        Health = health;
        Combat = combat;
    }

    public virtual void OnEnterState()
    {
        Phase = StatePhase.UPDATE;
    }

    public virtual void OnUpdateState()
    {
        Phase = StatePhase.UPDATE;
    }

    public virtual void OnExitState()
    {
        Phase = StatePhase.EXIT;
    }

    public State Process()
    {
        if (Phase == StatePhase.ENTER) OnEnterState();
        else if (Phase == StatePhase.UPDATE)
        {
            OnUpdateState();
        }
        else if (Phase == StatePhase.EXIT)
        {
            OnExitState();
            return nextState;
        }

        return this;
    }

    protected bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (player.position - agent.transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(agent.transform.position, player.position);

        // Controllo distanza
        if (distanceToPlayer > visionDistance)
        {
            return false;
        }

        // Controllo angolo
        float angle = Vector3.Angle(agent.transform.forward, directionToPlayer);
        if (angle > visionAngle / 2.0f)
        {
            return false;
        }

        // Raycast per controllare gli ostacoli
        if (Physics.Raycast(agent.transform.position + Vector3.up * 0.6f, directionToPlayer, out RaycastHit hit, distanceToPlayer))
        {
            Debug.DrawRay(agent.transform.position + Vector3.up * 0.6f, directionToPlayer * distanceToPlayer, Color.green, 1f);
            // Se il raycast colpisce il giocatore, può vederlo
            return hit.collider.TryGetComponent<Player>(out var _);
        }

        return false;
    }
}
