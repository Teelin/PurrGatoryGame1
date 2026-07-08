using NavMeshPlus.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;


public class SacredFire : MonoBehaviour
{

    Light2D fireLight;
    bool playerNearby = false;

    public static UnityEvent fireLit = new UnityEvent();
    public static UnityEvent fireDoused = new UnityEvent();
    
    NavMeshModifierVolume navObstacle;


    private void Awake()
    {
        fireLight = GetComponent<Light2D>();
        fireLight.enabled = false;
        navObstacle = GetComponent<NavMeshModifierVolume>();
        navObstacle.enabled = false;
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
                navObstacle.enabled = true;
                fireLit?.Invoke();
                

            }
            else
            {
                navObstacle.enabled = false;
                fireDoused?.Invoke();
                
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
