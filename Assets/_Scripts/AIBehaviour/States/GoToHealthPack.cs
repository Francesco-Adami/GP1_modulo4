using UnityEngine;
using UnityEngine.AI;

public class GoToHealthPack : State
{
    public GoToHealthPack(NavMeshAgent _agent, Transform _player, Health health, Combat combat) : base(_agent, _player, health, combat)
    {
    }

    private HealthPack currentHealthPack;

    public override void OnEnterState()
    {
        base.OnEnterState();

        agent.isStopped = false;

        currentHealthPack = HealthPackManager.Instance.GetClosestHealthPack(agent.transform.position);
        agent.SetDestination(currentHealthPack.transform.position);
    }

    public override void OnUpdateState()
    {
        Debug.Log("GOING TO HEALTH PACK");

        if (CanSeePlayer())
        {
            nextState = new AttackPlayer(agent, player, Health, Combat);
            Phase = StatePhase.EXIT;
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            nextState = new StateSelector(agent, player, Health, Combat);
            Phase = StatePhase.EXIT;
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
