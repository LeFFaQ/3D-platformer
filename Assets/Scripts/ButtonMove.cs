using UnityEngine;

public class ButtonMove : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isPressed = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPressed) 
        {
            isPressed = true;
            transform.position = originalPosition + new Vector3(0, -0.1f, 0);
            Debug.Log("Кнопка нажата"); 
        }
    }
}
