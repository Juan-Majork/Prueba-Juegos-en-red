using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private float speed;
    private Vector3 direction;

    void Update()
    {
        if (!photonView.IsMine) return;
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnMove(InputValue value)
    {
        direction = value.Get<Vector2>();
        direction.z = direction.y;
        direction.y = 0f;
    }
}
