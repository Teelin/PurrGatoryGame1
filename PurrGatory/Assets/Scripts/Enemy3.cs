using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [SerializeField]  GameObject[] patrolPoints ;
    [SerializeField] float patrolSpeed;

    private void Start()
    {
        foreach (GameObject point in patrolPoints)
        {
            point.transform.parent = null; // Detach patrol points from the enemy
        }
    }
    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < patrolPoints.Length; )
        {
            transform.position = Vector2.MoveTowards(transform.position, patrolPoints[i].transform.position, patrolSpeed * Time.deltaTime);
            if (patrolPoints[i].transform.position == transform.position)
            {
                i++;
                if (i >= patrolPoints.Length)
                {
                    i = 0;
                }
            }
        }
    }
}

