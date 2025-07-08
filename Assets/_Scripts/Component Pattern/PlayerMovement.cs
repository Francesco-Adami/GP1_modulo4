using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = 9.81f;

    protected bool isJumping = false;
    private float verticalVelocity = 0f; // Velocità verticale persistente

    private void Awake()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                throw new Exception("CharacterController component is missing on the player GameObject.");
            }
        }
    }

    private void Update() // Cambiato da FixedUpdate a Update
    {
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            // Reset della velocità verticale quando tocchiamo terra
            verticalVelocity = -2f; // Piccolo valore negativo per restare "attaccati" al suolo
            isJumping = false;
        }
        else
        {
            // Applica gravità alla velocità verticale
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Applica il movimento verticale
        Vector3 verticalMove = new Vector3(0, verticalVelocity * Time.deltaTime, 0);
        characterController?.Move(verticalMove);
    }

    internal void Move(Vector3 direction, float playerSpeed)
    {
        // Solo movimento orizzontale, la gravità è gestita separatamente
        Vector3 horizontalMove = new Vector3(direction.x, 0, direction.z) * playerSpeed * Time.deltaTime;
        characterController?.Move(horizontalMove);
    }

    internal void Jump()
    {
        if (!isJumping)
        {
            Debug.Log("Jumping!");
            verticalVelocity = jumpForce; // Imposta direttamente la velocità verticale
            isJumping = true;
        }
    }
}