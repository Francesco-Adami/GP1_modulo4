using UnityEngine;

public interface IStateAI
{
    void OnEnterState();
    void OnUpdateState();
    void OnExitState();
}
