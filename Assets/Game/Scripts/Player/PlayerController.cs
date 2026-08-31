using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawn;
    [SerializeField] private HealthComponent health;

    public bool typingState = false;

    private float attackCD = 2;
    private float counterCD = 0;
    
    private Vector3 direction;

    void Update()
    {
        if (!photonView.IsMine) return;

        if (!typingState)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }

            counterCD += Time.deltaTime;
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetTypingState(true);
        }
        
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
        direction.z = direction.y;
        direction.y = 0f;
    }


    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && counterCD > attackCD)
        {
            counterCD = 0;
            PhotonManager.Instance.SpawnObject(bullet.name, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        }
    }

    public bool SetTypingState(bool state)
    {
        return typingState = state;
    }

    public void TakeDamage(float damage)
    {
        photonView.RPC(nameof(health.OnHitted), RpcTarget.All, damage);
    }


}
