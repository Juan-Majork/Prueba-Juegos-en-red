using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletMovement : MonoBehaviourPun
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    private float timeLimit = 2;
    private float currentTime = 0;

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = transform.forward * speed;
        currentTime += Time.deltaTime;
        
        if (currentTime > timeLimit)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!photonView.IsMine) return;

        var other = collision.gameObject.GetComponent<PhotonView>();

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !other.IsMine)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            player.TakeDamage(30f);
            Destroy(gameObject);
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
