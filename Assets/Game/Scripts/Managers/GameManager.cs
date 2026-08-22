using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
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
        List<int> ints = new List<int>();
        SpawnerSelector(ints);
        foreach (int i in ints)
            pm.SpawnRoomObject(obstacle.name, obstSpawn[i].transform.position, Quaternion.identity);
    }

    private List<int> SpawnerSelector(List<int> ints)
    {
        int insert = Random.Range(0, obstSpawn.Count - 1);

        if (ints.Count < obstLimit)
        {
            if (!ints.Contains(insert))
            {
                ints.Add(insert);
            }
            else
            {
                return SpawnerSelector(ints);
            }
        }
        else
        {
            return ints;
        }

        return null;
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
