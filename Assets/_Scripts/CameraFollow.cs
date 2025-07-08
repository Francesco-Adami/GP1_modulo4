using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // The target to follow
    [SerializeField] private Transform targetSpawn;
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothing speed

    [SerializeField] private Vector3 offset; // Offset from the target position

    private void LateUpdate()
    {
        Vector3 target = Vector3.zero;
        if (!player.gameObject.activeInHierarchy)
        {
            target = targetSpawn.position;
        }
        else
        {
            target = player.position;
        }

        Vector3 desiredPosition = target + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
