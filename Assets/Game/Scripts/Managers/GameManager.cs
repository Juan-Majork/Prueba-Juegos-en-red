using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PhotonManager pm;
    public PhotonManager Pm { set { pm = value; } }

    [Header("Player Related")]
    [SerializeField] private GameObject playerPrefab;
    /// <summary>
    /// Index numbers: 0,1,2,3 => P1,P2,P3,P4
    /// </summary>
    [SerializeField] private List<GameObject> playerSpawners;

    [Header("Obstacle Related")]
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private List<GameObject> obstSpawners;
    [SerializeField] private int obstLimit;

    [Header("Display for Players")]
    [SerializeField] private TMP_InputField tmpInput;
    [SerializeField] private TextMeshProUGUI chatDisplay;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void InitializeGame()
    { 
        for(int i = 0; i < obstLimit; i++)
        pm.SpawnRoomObject(obstaclePrefab.name, obstSpawners[Random.Range(0, obstSpawners.Count)].transform.position, Quaternion.identity);
    }

    public void SpawnPlayer(int ID)
    {
        GameObject currentPlayer = pm.SpawnGameObject(playerPrefab.name, playerSpawners[ID].transform.position, Quaternion.identity);


        TypingController typingController = currentPlayer.AddComponent<TypingController>();
        typingController.Init(tmpInput, chatDisplay, currentPlayer.GetComponent<PlayerController>());
    }

    public void SomePlayerWon()
    {
        //Ir a una sala de espera (Otra escena)
        //Tras unos segundos mostrados en una UI conjunta, se vuelve al mapa.
        //Se vuelven a crear objetos en el mapa, de forma random.
    }
}
