using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : State
{
    public AttackPlayer(NavMeshAgent _agent, Transform _player, Health health, Combat combat) : base(_agent, _player, health, combat)
    {
    }

    float maxTimer = 3f;
    float timer = 0f;

    public override void OnEnterState()
    {
        base.OnEnterState();
        agent.isStopped = false;
    }

    public override void OnUpdateState()
    {
        Debug.Log("ATTACKING PLAYER");


        // se sto combattendo posso scappare per cercare una cura
        if (Health.CurrentHealth < Health.maxHealth * 0.5f)
        {
            if (Random.Range(0f, 1f) < 0.001f) // 0.1% di probabilità di cercare una cura
            {
                nextState = new GoToHealthPack(agent, player, Health, Combat);
                Phase = StatePhase.EXIT;
                return;
            }
        }

        // se vedo il player, attacco
        if (CanSeePlayer())
        {
            // Ruota usando il NavMeshAgent
            Vector3 direction = (player.position - agent.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * 20f);

            Combat.Fire();
        }
        else
        {
            // ruoto per guardare il player
            Vector3 direction = (player.position - agent.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, lookRotation, Time.deltaTime * 20f);

            agent.SetDestination(player.position);
            timer += Time.deltaTime;
            if (timer >= maxTimer)
            {
                nextState = new StateSelector(agent, player, Health, Combat);
                Phase = StatePhase.EXIT;
            }
        }
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }
}
