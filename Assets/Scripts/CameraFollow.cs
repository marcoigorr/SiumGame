using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private Vector3 CameraOffset;

    public void FixedUpdate()
    {
        Vector3 desiredPosition = _target.position + CameraOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(_target);
    }
}
