using Photon.Pun;
using UnityEngine;

public class BulletMovement : MonoBehaviourPun
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity += transform.forward * speed;
    }
}
