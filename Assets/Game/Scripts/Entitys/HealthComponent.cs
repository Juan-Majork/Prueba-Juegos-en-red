using Photon.Pun;
using UnityEngine;

public class HealthComponent : MonoBehaviourPun
{
    [SerializeField] private float health;

    public void OnHitted(float damageTaken)
    {
        if(TakeDamage(damageTaken) <= 0)
        {
            Application.Quit();
        }
    }

    private float TakeDamage(float damage)
    {
        health -= damage;
        return health;
    }
}
