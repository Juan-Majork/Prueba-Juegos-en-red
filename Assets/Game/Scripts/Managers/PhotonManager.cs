using UnityEngine;
using Photon.Pun;
using System;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    public static PhotonManager Instance;
    
    private GameManager gm;

    [SerializeField] public string lobbyName;
    private bool isMaster;

    private Action OnRoom;

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
        gm = GameManager.Instance;
        gm.Pm = this;

        OnRoom += MasterGameStart;

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
        OnRoom?.Invoke();
    }

    private void MasterGameStart()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;

        isMaster = PhotonNetwork.IsMasterClient;

        if (isMaster)
        {
            //gm.InitializeGame();
        }

        if (playerCount < 4)
        {
            gm.SpawnPlayer(playerCount);
        }
        else
        {
            Application.Quit();
        }
    }

    public void SpawnObject(string name, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.Instantiate(name, position, rotation, group: 0);
    }

    public void SpawnRoomObject(string name, Vector3 position, Quaternion rotation)
    {
        PhotonNetwork.InstantiateRoomObject(name, position, rotation, group: 0);
    }
}
