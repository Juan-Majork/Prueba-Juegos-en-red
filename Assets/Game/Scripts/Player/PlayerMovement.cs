using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    void Update()
    {
        if (!photonView.IsMine) return;
    }

    private void OnMove()
    {

    }
}
