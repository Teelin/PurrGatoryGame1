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
        enemyCount = LevelManager.Instance.GetEnemyCount(); 
        for (int i = 0; i < enemyCount; i++)
        {
            Debug.Log("Spawning enemy " + (i + 1) + " of " + enemyCount);
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            if (rooms.Length == 0) return;
            GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
            Vector3 spawnPosition = randomRoom.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnUrns()
    {
        urnCount = LevelManager.Instance.GetKittensThisLevel();
        for (int i = 0; i < urnCount; )
        {
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
            if (rooms.Length == 0) return;
            GameObject randomRoom = rooms[Random.Range(0, rooms.Length)];
            if (randomRoom.GetComponent<Room>().GetStartRoomStatus()) continue; // Skip spawning urns in the starting room
            Vector3 spawnPosition = randomRoom.transform.position + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0);
            Instantiate(urnPrefab, spawnPosition, Quaternion.identity);
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
        SpawnEnemies();
        SpawnUrns();
        SpawnSacredFire();
    }


}
