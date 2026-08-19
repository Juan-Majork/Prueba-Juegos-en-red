using Photon.Pun;
using UnityEngine;

public class CustomTransformSync : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float smoothTransition;

    private Vector3 networkPos;
    private Quaternion networkRotation;

    private void Update()
    {
        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, networkPos, smoothTransition * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, smoothTransition * Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {   
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            networkPos = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
