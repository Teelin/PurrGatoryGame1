using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SacredFire : MonoBehaviour
{

    Light2D fireLight;
    bool playerNearby = false;

    public static event System.Action<SacredFire> fireLit;
    public static event System.Action<SacredFire> fireDoused;


    private void Awake()
    {
        fireLight = GetComponent<Light2D>();
        fireLight.enabled = false;
    }
    private void Start()
    {
        PlayerController.useAction += LightFire;
    }

    void LightFire()
    {
        if(playerNearby)
        {
            fireLight.enabled = !fireLight.enabled;
            if (fireLight.enabled)
            {
                fireLit?.Invoke(this);
            }
            else
            {
                fireDoused?.Invoke(this);
            }
        }
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
}
