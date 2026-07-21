using System.Collections;
using UnityEngine;

public class BastHealth : MonoBehaviour
{

    [SerializeField] int maxDamage = 8;
    int maxLives = 9;
    int currentLives;

    int currentDamage = 0;

    float ghostTime = 5f;
    [SerializeField] SpriteRenderer spriteRenderer;

    float sacrificeCooldown = 10f;
    float timer = 0;
    bool canSacrifice = true;

    [SerializeField] LayerMask normalLayerMask, ghostLayerMask;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip damageSound;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentLives = maxLives;
    }

    // Update is called once per frame
    void Update()
    {
        if(!canSacrifice)
        {
            timer += Time.deltaTime;
            if (timer >= sacrificeCooldown)
            {
                canSacrifice = true;
                timer = 0;
            }
        }
    }

    public void TakeDamage(int Damage)
    {
        currentDamage += Damage;
        if (currentDamage >= maxDamage)
        {
            currentLives--;
            currentDamage = 0;
            if (currentLives <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine("GhostTime");

            }
        }
        GameManager.Instance.UpdateLives(currentLives);
        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(damageSound);
        }
    }

    public void TakeLife()
    {
        if (canSacrifice)
        {
            currentLives--;
            currentDamage = 0;
            LevelManager.Instance.KittenSaved();
            GameManager.Instance.KittenSaved();

            if (currentLives <= 0)
            {
                Die();
            }
            else
            {
                canSacrifice = false;
                StartCoroutine("GhostTime");

            }
        }
        GameManager.Instance.UpdateLives(currentLives);
        
    }

    void Die()
    {
        // Handle death logic here
        Debug.Log("Bast has died.");
        GameManager.Instance.RoundOver();
        //Destroy(gameObject);
        
    }

    IEnumerator GhostTime()
    {
        // Handle ghost time logic here
        spriteRenderer.color = Color.azure; // Change color to indicate ghost time
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f); // Make the sprite semi-transparent
        //GetComponent<Collider2D>().enabled = false; // Disable the collider to make the player invincible and walk through walls
        GetComponent<Collider2D>().excludeLayers = ghostLayerMask;

        // TODO : need to look at collision layers to make sure player cant walk off map but can get through hidden doors and avoid enemies

        yield return new WaitForSeconds(ghostTime);
        spriteRenderer.color = Color.white; // Change color back to normal
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f); // Restore the sprite's opacity
        GetComponent<Collider2D>().excludeLayers = normalLayerMask; // Re-enable the collider
    }

    public int GetCurrentDamage()
    {
        return currentDamage;
    }

    public int GetMaxDamage()
    {
        return maxDamage;
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public int GetMaxLives()
    {
        return maxLives;
    }

    public float GetSacrificeCooldownTimer()
    {
        return timer;
    }
    public float GetSacrificeCooldown()
    {
        return sacrificeCooldown;
    }

}


