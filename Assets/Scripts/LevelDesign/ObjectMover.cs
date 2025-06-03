using NaughtyAttributes;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Vector3 direction = Vector3.forward;
    public float speed = 5f;
    public bool isAccelerating = true;
    [ShowIf("isAccelerating")]
    public float acceleration = 1f;

    private float currentSpeed;

    void Start()
    {
        currentSpeed = speed;
    }

    void Update()
    {
        if (!isAccelerating)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime;
            transform.Translate(direction.normalized * currentSpeed * Time.deltaTime);
        }
    }
}
