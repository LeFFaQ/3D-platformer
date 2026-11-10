using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    public float lowerDistance = 3f;
    public float speed = 2f;

    private Vector3 originalPosition;
    private bool isOpening = false;

    private PlayerSystem playerSystem;

    void Start()
    {
        originalPosition = transform.position;

        playerSystem = PlayerSystem.Instance;
        playerSystem.interactAction.canceled += OnInteract;

        //playerSystem.interactAction.started;   - действие началось или нажатие
        //playerSystem.interactAction.performed; - нажатие с небольшой задержкой        
        // playerSystem.interactAction.canceled; - отпускание         

    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log("нажатие");
        if (Vector3.Distance(playerSystem.self.transform.position, transform.position) < 3f)
        {
            StartCoroutine(LowerDoor());
        }

    }

    IEnumerator LowerDoor()
    {
        Vector3 targetPosition =
            originalPosition - new Vector3(0, lowerDistance, 0);

        while(Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = 
                Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    speed * Time.deltaTime
                );

            yield return null;
        }
    }
}
