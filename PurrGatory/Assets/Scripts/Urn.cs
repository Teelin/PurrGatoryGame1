using UnityEngine;

public class Urn : MonoBehaviour
{

    bool playerNearby = false;
    [SerializeField] GameObject KittenPrefab;
    Animator animator;
    AudioSource audioSource;

    private void Awake()
    {
        PlayerController.rattleAction += Rattle;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnDisable()
    {
        PlayerController.rattleAction -= Rattle;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
    private void Rattle()
    {
        if (playerNearby)// Implement the logic for what happens when the player uses the rattle near the urn
        {
            //spawn kitten ghost
            audioSource.Play();
            animator.Play("Urn_Smashed");
            Instantiate(KittenPrefab, transform.position, Quaternion.identity);
            
        }
        
    }
}
