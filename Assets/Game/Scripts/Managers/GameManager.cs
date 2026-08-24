using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private PhotonManager pm;
    public PhotonManager Pm { set { pm = value; } }

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private List<GameObject> obstSpawn;
    [SerializeField] private int obstLimit;
    [SerializeField] private List<GameObject> playerXSpawners; //0,1,2,3 => P1,P2,P3,P4

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
        pm.SpawnRoomObject(obstacle.name, obstSpawn[Random.Range(0, obstSpawn.Count)].transform.position, Quaternion.identity);
    }

    public void SpawnPlayer(int ID)
    {
        pm.SpawnObject(player.name, playerXSpawners[ID].transform.position, Quaternion.identity);
    }

    public void SomePlayerWon()
    {
        //Ir a una sala de espera (Otra escena)
        //Tras unos segundos mostrados en una UI conjunta, se vuelve al mapa.
        //Se vuelven a crear objetos en el mapa, de forma random.
    }
}
