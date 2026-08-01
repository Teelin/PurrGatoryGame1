using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class KittenManager : MonoBehaviour
{

    List<GameObject> spawnedKittens = new List<GameObject>();
    [SerializeField] private float pointSpacing = 0.5f;

    [SerializeField] GameObject KittenPrefab;

    // Stores the sequence of positions Bastet has walked through
    public List<Vector3> positionHistory { get; private set; } = new List<Vector3>();

    private Vector3 lastRecordedPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Urn.kittenSpawned.AddListener(OnKittenSpawned);
        positionHistory.Add(transform.position);
        lastRecordedPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has moved far enough to drop a new trail node
        if (Vector3.Distance(transform.position, lastRecordedPosition) >= pointSpacing)
        {
            positionHistory.Add(transform.position);
            lastRecordedPosition = transform.position;
        }
    }

    private void OnKittenSpawned(GameObject kitten)
    {
        // Handle the kitten spawn event
        spawnedKittens.Add(kitten);
        int lineIndex = spawnedKittens.Count - 1;
        kitten.GetComponent<GhostKitten>().InitializeFollower(this, lineIndex);

    }

    public List<GameObject> GetSpawnedKittens()
    {
        return spawnedKittens;
    }
    public void RemoveKitten(GameObject kitten)
    {
        if (spawnedKittens.Contains(kitten))
        {
            spawnedKittens.Remove(kitten);
            // Re-index remaining kittens so there are no gaps in the line
            for (int i = 0; i < spawnedKittens.Count; i++)
            {
                spawnedKittens[i].GetComponent<GhostKitten>().InitializeFollower(this, i);
            }
        }
    }
    public void ClearKittens()
    {
        spawnedKittens.Clear();
    }
    public int GetKittenCount()
    {
        return spawnedKittens.Count;
    }

    public void DestroyAllKittens()
    {
        foreach (var kitten in spawnedKittens)
        {
            if (kitten != null)
            {
                StartCoroutine(kitten.GetComponent<GhostKitten>().DestroyKitten());
            }
        }
        spawnedKittens.Clear();
    }

    public void AddKitten(GameObject kitten)
    {
        if (!spawnedKittens.Contains(kitten))
        {
            spawnedKittens.Add(kitten);
        }
    }
    public int GetKittenIndex(GameObject kitten)
    {
        return spawnedKittens.IndexOf(kitten);
    }

    public void SpawnNewKitten()
    {
        GameObject kitten = Instantiate(KittenPrefab, transform.position, Quaternion.identity);
        OnKittenSpawned(kitten);
    }

}
