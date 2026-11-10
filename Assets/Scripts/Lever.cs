using UnityEngine;

public class Lever : MonoBehaviour
{

    // TODO:
    // СКРИПТ ПОД ОПТИМИЗАЦИЮ
    private bool isActivated = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isActivated = !isActivated;
            transform.Rotate(0, 0, isActivated ? 45f : -45f);
            Debug.Log("Рычаг переключен");
        }
    }
}
