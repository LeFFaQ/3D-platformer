using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    [Header("References")] 
    [SerializeField] private Transform yawPoint; // вокруг объекта
    [SerializeField] private Transform pitchPoint; // вверх-вниз камеры


    [Header("Settings")]
    [SerializeField] private float sensitivity = 1f;

    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 75f;

    private PlayerInputHandler input;

    private float yaw;
    private float pitch;

    void Awake()
    {
        input = GetComponent<PlayerInputHandler>();

        if (yawPoint == null)
            yawPoint = transform;
        
        if (pitchPoint == null) 
            pitchPoint = yawPoint;
    }

        void LateUpdate()
    {
        Vector2 lookInput = input.Look;

        yaw += lookInput.x * sensitivity;
        pitch -= lookInput.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        yawPoint.rotation = Quaternion.Euler(0f, yaw, 0f);
        pitchPoint.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    public Transform GetMovementReference()
    {
        return yawPoint;
    }
}
