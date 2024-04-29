using UnityEngine;

public class DamageScript : MonoBehaviour
{
    [Header("Damage Controller")]
    [SerializeField]
    public int Damage = 1; // Damage dealt to the player

    // When a collider triggers collision with this object
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player") // If the collider is the player
        {
            FindAnyObjectByType<HealthControl>().damagePlayer(Damage); // Deal damage to the player equal to Damage
        }
    }
}
