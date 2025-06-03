using UnityEngine;

public class Hook : MonoBehaviour
{
    private FixedJoint fixedJoint;
    [SerializeField] private GrapplingGun gun;

    [HideInInspector] public GameObject collisionObject;


    private void OnCollisionEnter(Collision collision)
    {

        // Debug.Log(this.name + " collided with " + collision.gameObject.name);
        if (collision.gameObject.tag == "Wall" || collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            fixedJoint = gameObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = collision.gameObject.GetComponent<Rigidbody>();

        }
    }

    public void DestroyJoint()
    {
        Destroy(fixedJoint);
    }
}
