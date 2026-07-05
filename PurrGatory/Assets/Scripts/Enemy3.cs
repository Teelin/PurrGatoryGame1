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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            // Handle collision with player
            Debug.Log("Enemy collided with Player!");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Handle trigger with player
            Debug.Log("Enemy triggered with Player!");
        }
    }
}



