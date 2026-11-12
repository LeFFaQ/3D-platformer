using UnityEngine;

public class CollectableAnimation : MonoBehaviour
{
    public float flySpeed = 2f;
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    public float changeDirectionTime = 2f;

    private Vector3 currectDirection = Vector3.up;
    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            currectDirection = -currectDirection;
            timer = changeDirectionTime;
        }

        transform.Translate(currectDirection * flySpeed * Time.deltaTime, Space.World);

        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}
