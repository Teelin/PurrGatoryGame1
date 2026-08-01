using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField]  GameObject[] patrolPoints ;
    int targetPointIndex = 0;
    [SerializeField] float patrolSpeed;
    bool changingTarget = false;
    [SerializeField] AudioSource AudioSource;
    

    private void Start()
    {
        foreach (GameObject point in patrolPoints)
        {
            point.transform.parent = null; // Detach patrol points from the enemy
        }
        
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[targetPointIndex].transform.position, patrolSpeed * Time.deltaTime);
        if (patrolPoints[targetPointIndex].transform.position == transform.position && !changingTarget)
        {
            changingTarget = true;
            StartCoroutine(ChangeTarget());
            
        }
    }

    IEnumerator ChangeTarget()
    {
        yield return new WaitForSeconds(1f);
        targetPointIndex++;
        if (targetPointIndex >= patrolPoints.Length)
        {
            targetPointIndex = 0;
        }
        changingTarget = false;
        AudioSource.Play();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Handle trigger with player
            collision.GetComponent<BastHealth>().TakeDamage(8); 
        }
        if(collision.CompareTag("GhostKitten"))
        {
            StartCoroutine(collision.gameObject.GetComponent<GhostKitten>().DestroyKitten());
            Debug.Log("GhostKitten destroyed by Enemy3");
        }
    }
}



