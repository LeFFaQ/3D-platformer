using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public Transform target;
    public float distance = 5f;
    public float height = 1f;
    public float sensitivity = 1f;

    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 75f;

    private PlayerInputHandler input;

    private float horizontalAngle;
    private float verticalAngle;

    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        horizontalAngle += input.Look.x * sensitivity;
        verticalAngle -= input.Look.y * sensitivity;

        verticalAngle = Mathf.Clamp(
            verticalAngle,
            minVerticalAngle,
            maxVerticalAngle
        );     

        Quaternion rotation = Quaternion.Euler(
            verticalAngle,
            horizontalAngle,
            0
        );  

        Vector3 targetPosition = target.position + Vector3.up * height;

        Camera.main.transform.position = 
            targetPosition + rotation * Vector3.back * distance;

        Camera.main.transform.LookAt(targetPosition);
    }
}
