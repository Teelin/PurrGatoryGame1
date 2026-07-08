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
    GameObject[] enemySpawns;
    GameObject[] urnSpawns;



    void Awake()
    {
        LevelGenerator.levelGenerated += OnLevelGenerated;
        
    }
    void GetSpawnLocations()
    {
        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");
        urnSpawns = GameObject.FindGameObjectsWithTag("UrnSpawn");
    }

    void SpawnEnemies()
    {
        enemyCount = LevelManager.Instance.GetEnemyCount(); 
        for (int i = 0; i < enemyCount; )
        {
            if (enemySpawns.Length == 0) return;
            int randomIndex = Random.Range(0, enemySpawns.Length);
            if(enemySpawns[randomIndex] == null) 
            { 
                continue;
            }
            GameObject randomSpawn = enemySpawns[randomIndex];
            Vector3 spawnPosition = randomSpawn.transform.position;
            var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemySpawns[randomIndex] = null; // Mark this spawn point as used
            i++;
        }
    }

    void SpawnUrns()
    {
        urnCount = LevelManager.Instance.GetKittensThisLevel();
        for (int i = 0; i < urnCount; )
        {
            
            if (urnSpawns.Length == 0) return;
            int randomIndex = Random.Range(0, urnSpawns.Length);
            if(urnSpawns[randomIndex] == null) 
            { 
                continue;
            }
            GameObject randomSpawn = urnSpawns[randomIndex];
            Vector3 spawnPosition = randomSpawn.transform.position;
            Instantiate(urnPrefab, spawnPosition, Quaternion.identity);
            urnSpawns[randomIndex] = null; // Mark this spawn point as used
            i++;
        }
    }

    void SpawnSacredFire()
    {
        sacredFireCount = 1; // Example: Always spawn 1 sacred fire per level, can be adjusted later based on level or difficulty
        for (int i = 0; i < sacredFireCount; i++)
        {
            bool sacredFireSpawned = false;
            while (!sacredFireSpawned)
            {
                GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
                if (rooms.Length == 0) return;
                GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
                if (randomRoom.GetComponent<Room>().GetSacredFireStatus())
                {
                    randomRoom.GetComponent<Room>().SetSacredFireActive(true); // Mark the room as having a sacred fire 
                    sacredFireSpawned = true;
                }
            }
    
            
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
        GetSpawnLocations();
        SpawnEnemies();
        SpawnUrns();
        SpawnSacredFire();
    }


}
