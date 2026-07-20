using UnityEngine;

public class Enemy2 : MonoBehaviour
{

    [SerializeField] Transform capturePosition;
    bool isKittenClose = false;
    [SerializeField]ContactFilter2D contactFilter;
    [SerializeField]float captureRadius = 0.75f;
    Collider2D[] results = new Collider2D[10];
    [SerializeField] AudioSource audioSource;

    private void Update()
    {
        isKittenClose = Physics2D.OverlapCircle(capturePosition.position, captureRadius, contactFilter, results) > 0;

        if (isKittenClose)
        {
            results[0].gameObject.GetComponent<GhostKitten>().IsCaptured(transform.position);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
            this.enabled = false;
        }


    }


}
