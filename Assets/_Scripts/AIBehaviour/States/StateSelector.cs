using UnityEngine;
using UnityEngine.AI;

public class StateSelector : State
{
    // In questo stato, l'IA deciderà a quale stato passare in base alle condizioni attuali del gioco.
    // Valuterà la situazione e selezionerà l'azione più appropriata da intraprendere.
    // StateSelector --> al posto del solito "Idle" state

    public StateSelector(NavMeshAgent _agent, Transform _player, Health health, Combat combat) : base(_agent, _player, health, combat)
    {
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
    }

    public override void OnUpdateState()
    {
        // selezione il miglior stato successivo in base a delle priorità
        // 1. conquista zona
        // 2. difesa zona
        // 3. attacco player se minaccia zona o per liberare la zona dal player
        // 4. cerca cura con probabilità
        Flag flag = GameObject.FindAnyObjectByType<Flag>(FindObjectsInactive.Include);

        // se la currenthealth è minore del 50% della maxhealth, cerca una cura
        if (Health.CurrentHealth < Health.maxHealth * 0.5f)
        {
            if (Random.Range(0f, 1f) < 0.25f) // 25% di probabilità di cercare una cura
            {
                nextState = new GoToHealthPack(agent, player, Health, Combat);
                Phase = StatePhase.EXIT;
                return;
            }
        }

        if (flag.ZoneCaptured != ZoneCaptured.Enemy)
        {
            // vai a conquistare la zona
            nextState = new GoToFlag(agent, player, Health, Combat);
            Phase = StatePhase.EXIT;
            return;
        }

        if (flag.ZoneCaptured == ZoneCaptured.Enemy)
        {
            if (flag.IsPlayerCapturing)
            {
                // attacca il player (- quindi la zona)
                nextState = new AttackPlayer(agent, player, Health, Combat);
                Phase = StatePhase.EXIT;
                return;
            }

            // difendi la zona - cerca copertura
            Debug.Log("Defending the zone");
            nextState = new SeekCover(agent, player, Health, Combat);
            Phase = StatePhase.EXIT;
            return;
        }

        // non dovrei mai arrivare qui
        Phase = StatePhase.EXIT;
    }

    public override void OnExitState()
    {
        base.OnExitState();
        // Cleanup or reset any variables if necessary
    }
}
