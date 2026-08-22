using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletSpawn;
    private Vector3 direction;

    void Update()
    {
        if (!photonView.IsMine) return;
        transform.Translate(direction * speed * Time.deltaTime);

        Attack();
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
        direction.z = direction.y;
        direction.y = 0f;
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PhotonNetwork.InstantiateRoomObject(bullet.name, bulletSpawn.transform.position, bulletSpawn.transform.rotation, group: 0);
        }
    }
}
