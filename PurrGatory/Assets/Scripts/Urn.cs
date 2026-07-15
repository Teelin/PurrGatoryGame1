using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Urn : MonoBehaviour
{

    bool playerNearby = false;
    [SerializeField] GameObject KittenPrefab;
    [SerializeField] TextMeshProUGUI popUp;
    Transform player;
    Animator animator;
    AudioSource audioSource;

    private void Awake()
    {
        PlayerController.rattleAction += Rattle;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
    }
    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>().transform;
    }
    private void OnDisable()
    {
        PlayerController.rattleAction -= Rattle;
        
    }

    private void Update()
    {
        playerNearby = Vector2.Distance(transform.position, player.position) < player.GetComponent<PlayerController>().GetRattleRange();
        if (playerNearby)
        {
            popUp.enabled = true;
        }
        else
        {
            popUp.enabled = false;
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
