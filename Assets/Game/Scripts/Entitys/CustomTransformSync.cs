using Photon.Pun;
using UnityEngine;

public class CustomTransformSync : MonoBehaviourPun, IPunObservable
{
    private Vector3 networkPos;
    private Quaternion networkRotation;

    private void Update()
    {
        if (!photonView.IsMine)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, networkPos, Time.deltaTime);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, networkRotation, Time.deltaTime);
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
