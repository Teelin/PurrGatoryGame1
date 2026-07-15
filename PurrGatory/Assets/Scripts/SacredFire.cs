using NavMeshPlus.Components;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class SacredFire : MonoBehaviour
{

    Light2D fireLight;
    bool playerNearby = false;

    public static UnityEvent fireLit = new UnityEvent();
    public static UnityEvent fireDoused = new UnityEvent();
    [SerializeField] TextMeshProUGUI popUp;
    GameObject player;
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
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        playerNearby = Vector2.Distance(transform.position, player.transform.position) < player.GetComponent<PlayerController>().GetRattleRange();
        if (playerNearby)
        {
            popUp.enabled = true;
        }
        else
        {
            popUp.enabled = false;
        }
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
    

}
