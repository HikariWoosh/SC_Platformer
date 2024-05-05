using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [Header("Damage Controller")]

    [SerializeField]
    public int Damage = 1;

    // When a collider triggers collision with this object
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // Deal damage to the player equal to the passed value
            FindAnyObjectByType<HealthControl>().damagePlayer(Damage);
        }
    }
}
