using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [Header("Level Spawner Settings")]
    [SerializeField] int enemyCount;
    [SerializeField] int urnCount;
    [SerializeField] int sacredFireCount;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject urnPrefab;
    [SerializeField] GameObject sacredFire;



    void Awake()
    {
        LevelGenerator.levelGenerated += OnLevelGenerated;
        
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Debug.Log("Spawning enemy " + (i + 1) + " of " + enemyCount);
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            if (rooms.Length == 0) return;
            GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
            Vector3 spawnPosition = randomRoom.transform.position + new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnUrns()
    {
        for (int i = 0; i < urnCount; i++)
        {
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            if (rooms.Length == 0) return;
            GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
            Vector3 spawnPosition = randomRoom.transform.position + new Vector3(Random.Range(-4f, 4f), Random.Range(-4f, 4f), 0);
            Instantiate(urnPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnSacredFire()
    {
        for (int i = 0; i < sacredFireCount; i++)
        {
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            if (rooms.Length == 0) return;
            GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
            Vector3 spawnPosition = randomRoom.transform.position;
            Instantiate(sacredFire, spawnPosition, Quaternion.identity);
        }
    }
    void resetLevel()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        GameObject[] urns = GameObject.FindGameObjectsWithTag("Urn");
        foreach (GameObject urn in urns)
        {
            Destroy(urn);
        }
        GameObject[] sacredFires = GameObject.FindGameObjectsWithTag("SacredFire");
        foreach (GameObject fire in sacredFires)
        {
            Destroy(fire);
        }
    }

    void OnLevelGenerated()
    {
        resetLevel();
        SpawnEnemies();
        SpawnUrns();
        SpawnSacredFire();
    }


}
