using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Manager Settings")]
    [SerializeField] private bool isGamePaused = false;

    [Header("Characters Settings")]
    [SerializeField] private float characterHealth = 100.0f;
    [SerializeField] private float characterSpeed = 5.0f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float characterRespawn = 3.0f;

    [Header("Map References")]
    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    private void Awake()
    {
        IsGamePaused = true;
    }

    public bool IsGamePaused
    {
        set
        {
            isGamePaused = value;
            if (isGamePaused) Time.timeScale = 0f; // Pause the game
            else Time.timeScale = 1f; // Resume the game
        }
        get { return isGamePaused; }
    }

    public float CharacterHealth
    {
        private set { characterHealth = value; }
        get { return characterHealth; }
    }

    public float CharacterSpeed
    {
        private set { characterSpeed = value; }
        get { return characterSpeed; }
    }

    public float FireRate
    {
        private set { fireRate = value; }
        get { return fireRate; }
    }

    internal IEnumerator RespawnRoutine(Character character)
    {
        // Disabilita i componenti e imposta lo stato del personaggio
        character.gameObject.GetComponent<CharacterController>().enabled = false;
        NavMeshAgent nav = character.gameObject.GetComponent<NavMeshAgent>();
        if (nav != null)
        {
            nav.enabled = false;
        }

        foreach (var component in character.GetComponentsInChildren<MonoBehaviour>(true))
        {
            component.enabled = false;
        }

        foreach (Transform t in character.transform)
        {
            t.gameObject.SetActive(false);
        }

        // Attendi il tempo di respawn
        yield return new WaitForSecondsRealtime(characterRespawn);

        character.health.CurrentHealth = characterHealth;
        if (character is Player player)
        {
            player.transform.SetPositionAndRotation(playerSpawnPoint.position, playerSpawnPoint.rotation);
        }
        else
        {
            character.transform.SetPositionAndRotation(enemySpawnPoint.position, enemySpawnPoint.rotation);
        }

        // Riabilita i componenti e imposta lo stato del personaggio
        character.gameObject.GetComponent<CharacterController>().enabled = true;
        if (nav != null)
        {
            nav.enabled = true;
        }

        foreach (var component in character.GetComponentsInChildren<MonoBehaviour>(true))
        {
            component.enabled = true;
        }

        foreach (Transform t in character.transform)
        {
            t.gameObject.SetActive(true);
        }
    }
}
