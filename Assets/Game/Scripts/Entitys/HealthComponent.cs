using Photon.Pun;
using UnityEngine;

public class HealthComponent : MonoBehaviourPun
{
    [SerializeField] private float health;

    [PunRPC]
    public void OnHitted(float damageTaken)
    {
        if(TakeDamage(damageTaken) <= 0)
        {
            Destroy(gameObject);
        }
    }

    private float TakeDamage(float damage)
    {
        health -= damage;
        return health;
    }
}
