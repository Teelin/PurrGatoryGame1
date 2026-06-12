//using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]

    [SerializeField] private int minRooms, maxRooms;
    [SerializeField] private int levelMaxWidth, levelMaxHeight;
    [SerializeField] private GameObject roomPrefab, bossPrefab, startPrefab;

    private int[,] levelGrid;
    private int targetRoomCount;

    private GameObject startRoom;

    private void OnEnable()
    {
        targetRoomCount = Random.Range(minRooms, maxRooms + 1);
        GenerateLevel();
        PlaceRooms();
    }
    private void Start()
    {
        GameManager.Instance.SetLevelGrid(levelGrid);
    }

    private void GenerateLevel()
    {
        levelGrid = new int[levelMaxHeight, levelMaxWidth]; // Reset the grid
        Vector2Int currentPos = new Vector2Int(levelMaxWidth / 2, levelMaxHeight/2); // Start at the middle of the grid
        levelGrid[currentPos.y, currentPos.x] = 2;
        GameManager.Instance.SetStartingPosition(currentPos);

        while (targetRoomCount > 0)
        {
            Vector2Int nextRoom = Vector2Int.zero;
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {

                Vector2Int neighborPos = currentPos + direction;
                if (neighborPos.x < 0 || neighborPos.x >= levelMaxWidth || neighborPos.y < 0 || neighborPos.y >= levelMaxHeight) continue;// Skip if out of bounds
                if (levelGrid[neighborPos.y, neighborPos.x] == 1 || levelGrid[neighborPos.y, neighborPos.x] == 2) continue; // Skip if room already exists

                if (Random.value < 0.5f) // 50% chance to place a room
                {
                    levelGrid[neighborPos.y, neighborPos.x] = 1; // Mark the grid cell as occupied
                    targetRoomCount--;
                    if (targetRoomCount <= 0) break; // Stop if we've placed enough rooms
                    nextRoom = neighborPos;
                }

            }
            if (nextRoom != Vector2Int.zero)
                currentPos = nextRoom; // Move to the next room position
            else
                foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                {
                    Vector2Int neighborPos = currentPos + direction;
                    if (neighborPos.x < 0 || neighborPos.x >= levelMaxWidth || neighborPos.y < 0 || neighborPos.y >= levelMaxHeight) continue;// Skip if out of bounds
                    if (levelGrid[neighborPos.y, neighborPos.x] == 1)
                    {
                        currentPos = neighborPos; // Move to the next room position
                        break;
                    }
                }
        }
        GenerateBoss(currentPos);
        

    }

    private void PlaceRooms()
    {
        for (int i = 0; i < levelMaxHeight; i++)
        {
            for (int j = 0; j < levelMaxWidth; j++)
            { 
                int pos = levelGrid[i, j];
                if (pos == 1)
                {
                    // Instantiate room prefab
                    Instantiate(roomPrefab, new Vector3(i * 16, j * 9, 0), Quaternion.identity);
                }
                else if (pos == 2)
                {
                    // Instantiate start room prefab
                    startRoom = Instantiate(startPrefab, new Vector3(i * 16, j * 9, 0), Quaternion.identity);
                }
                else if (pos == 3)
                {
                    // Instantiate boss room prefab
                    Instantiate(bossPrefab, new Vector3(i * 16, j * 9, 0), Quaternion.identity);
                }

            }
        }

        startRoom.GetComponent<StarterRoom>().CheckAdjacentRooms(); // Check for adjacent rooms to the starting room
    }
      
    private void GenerateBoss(Vector2Int position)
    {
        bool bossPlaced = false;
        foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
        {
            if(bossPlaced) break; // Stop if boss room has been placed)
            Vector2Int tempBoss = position + direction;
            if (tempBoss.x < 0 || tempBoss.x >= levelMaxWidth || tempBoss.y < 0 || tempBoss.y >= levelMaxHeight)
                continue; // Skip if out of bounds
            if (levelGrid[tempBoss.y, tempBoss.x] == 1 || levelGrid[tempBoss.y, tempBoss.x] == 2)
                continue; // Room already exists

            levelGrid[tempBoss.y, tempBoss.x] = 3; // Mark the grid cell as occupied by boss room
            bossPlaced=true;
        }
        if (!bossPlaced)
        {
            GenerateLevel(); // If we couldn't place the boss room, regenerate the level
        }

}
}
