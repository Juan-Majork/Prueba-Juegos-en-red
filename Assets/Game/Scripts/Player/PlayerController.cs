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
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        if (direction.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.LookRotation(direction);

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
            PhotonManager.Instance.SpawnObject(bullet.name, bulletSpawn.transform.position, bulletSpawn.transform.rotation);
        }
    }
}
