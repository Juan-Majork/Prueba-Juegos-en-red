using UnityEngine;
using Photon.Pun;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public string lobbyName;

    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject obstaclesPrefab;

    public static PhotonManager instance;

    private Action OnRoom;

    private bool IsMaster;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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

        IsMaster = PhotonNetwork.IsMasterClient;

        if (IsMaster)
        {
            SpawnObjects();
        }

        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity, group: 0);
    }

    public void SpawnObjects()
    {
        PhotonNetwork.Instantiate(obstaclesPrefab.name, new Vector3 (0, 0.5f, 0), Quaternion.identity, group: 0);


    }
}
