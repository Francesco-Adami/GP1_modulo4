using System;
using UnityEngine;

public class Player : Character
{
    [Header("Player Components")]
    [SerializeField] private PlayerMovement movement;

    protected override void Start()
    {
        base.Start();

        health.OnDeath = null; // Reset to avoid multiple subscriptions
        health.OnDeath += OnDeathHandle;
    }

    private void OnDeathHandle()
    {
        Debug.Log("Player has died.");
        FindAnyObjectByType<Flag>().IsPlayerCapturing = false;
        StartCoroutine(gameManager.RespawnRoutine(this));
    }

    private void Update()
    {
        if (gameManager.IsGamePaused) return;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // rotazione dove punta il mouse
        RotateTowardsMouse();

        movement.Move(direction, characterSpeed);

        if (Input.GetButton("Fire1") && combat.fireRate <= 0)
        {
            combat.Fire();
        }
        else
        {
            FireCooldown();
        }
    }

    private void RotateTowardsMouse()
    {
        // Ottieni la posizione del mouse in coordinate schermo
        Vector3 mousePosition = Input.mousePosition;

        // Converti la posizione del mouse in coordinate del mondo
        // Usa la camera principale e proietta sul piano Y del player
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldMousePosition = ray.GetPoint(distance);

            // Calcola la direzione dal player al mouse
            Vector3 lookDirection = (worldMousePosition - transform.position).normalized;
            lookDirection.y = 0; // Mantieni solo la rotazione orizzontale

            // Ruota il player verso quella direzione
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Flag"))
        {
            flag.IsPlayerCapturing = true;

            if (flag.IsEnemyCapturing) return;
            flag.PlayerCaptureProgress += Time.deltaTime;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Flag"))
        {
            flag.IsPlayerCapturing = false;
        }
    }
}
