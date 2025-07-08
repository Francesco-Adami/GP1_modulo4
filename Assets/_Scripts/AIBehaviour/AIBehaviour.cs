using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour
{
    private State currentState;

    void Update()
    {
        if (GameManager.Instance.IsGamePaused) return;

        currentState = currentState?.Process();
    }

    public void SetState(State newState)
    {
        currentState = newState;
    }
}
