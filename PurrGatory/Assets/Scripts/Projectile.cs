using UnityEngine;

public class Projectile : MonoBehaviour
{
    float lifeTime = 5f; // Lifetime of the projectile in seconds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime); // Destroy the projectile after its lifetime expires
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<BastHealth>().TakeDamage(4);
            Destroy(gameObject); // Destroy the projectile on collision

        }
        if(collision.CompareTag("Walls"))
        {
            Destroy(gameObject); // Destroy the projectile on collision
        }
        

    }
}

