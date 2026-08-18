using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject playerSpawn;
    [SerializeField] private List<GameObject> obstSpawn;

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
        for (int i = 0; i < obstSpawn.Count; i++)
        {
            PhotonManager.Instance.SpawnObject(obstacle.name, obstSpawn[i].transform.position, Quaternion.identity);
        }
    }

    public void SpawnPlayer()
    {
        PhotonManager.Instance.SpawnObject(player.name, playerSpawn.transform.position, Quaternion.identity);
    }
}
