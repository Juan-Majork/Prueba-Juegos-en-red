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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log(collision.gameObject.name);
            
        }
        else
        {
            var other = collision.gameObject.GetComponent<PhotonView>();
            if(!other.IsMine)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    PlayerController player = collision.gameObject.GetComponent<PlayerController>();

                    Debug.Log(collision.gameObject.name);
                    player.TakeDamage(30f);
                }
            }   
        }

        Destroy(gameObject);
    }
}
