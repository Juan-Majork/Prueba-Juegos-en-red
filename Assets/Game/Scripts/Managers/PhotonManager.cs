using UnityEngine;
using Photon.Pun;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;

    [SerializeField] public string lobbyName;

    private Action OnRoom;

    private bool isMaster;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to lobby");
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: lobbyName);
    }

    public override void OnJoinedRoom()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        isMaster = PhotonNetwork.IsMasterClient;

        if (isMaster)
        {
            GameManager.Instance.InitializeGame();
        }

        GameManager.Instance.SpawnPlayer();
    }

    public void SpawnObject(string name, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate(name, position, rotation, group: 0);
    }
}
