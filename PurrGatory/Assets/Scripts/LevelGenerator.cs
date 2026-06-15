using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]

    [SerializeField] private int minRooms, maxRooms;
    [SerializeField] private int levelMaxWidth, levelMaxHeight;
    [SerializeField] private GameObject roomPrefab, bossPrefab, startPrefab, sunBargePrefab;

    private int[,] levelGrid, roomIds;
    private int targetRoomCount, currentRoomId;

    private void Awake()
    {
        targetRoomCount = Random.Range(minRooms, maxRooms + 1);
        currentRoomId = 1; // Start room IDs from 1
        GenerateLevel();
        PlaceRooms();
    }
   
    private void GenerateLevel()
    {
        levelGrid = new int[levelMaxHeight, levelMaxWidth]; // Reset the grid
        roomIds = new int[levelMaxHeight, levelMaxWidth]; // Reset the room IDs
        int roomsPlaced = 0; // Reset the number of rooms placed


        Vector2Int currentPos = new Vector2Int(levelMaxWidth / 2, levelMaxHeight/2); // Start at the middle of the grid
        levelGrid[currentPos.x, currentPos.y] = 2; // Mark the starting room in the grid
        levelGrid[currentPos.x , currentPos.y - 1] = 4; // Mark the SunBarge room in the grid
        roomIds[currentPos.x, currentPos.y] = currentRoomId; // Assign a room ID to the starting room
        currentRoomId++; // Increment the room ID for the next room
        GameManager.Instance.SetStartingPosition(currentPos);

        for (roomsPlaced = 0; roomsPlaced < targetRoomCount; roomsPlaced++)
        {

            Vector2Int nextRoom = Vector2Int.zero;
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentPos + direction;

                if (neighborPos.x < 0 || neighborPos.x >= levelMaxWidth || neighborPos.y < 0 || neighborPos.y >= levelMaxHeight) continue;// Skip if out of bounds
                if (levelGrid[neighborPos.y, neighborPos.x] == 1 || levelGrid[neighborPos.y, neighborPos.x] == 2 || levelGrid[neighborPos.y, neighborPos.x] == 4) continue; // Skip if room already exists

                if (Random.value < 0.5f) // 50% chance to place a room
                {
                    levelGrid[neighborPos.y, neighborPos.x] = 1; // Mark the grid cell as occupied
                    roomIds[neighborPos.y, neighborPos.x] = currentRoomId; // Assign a room ID to the new room
                    currentRoomId++; // Increment the room ID for the next room
                    roomsPlaced++;
                    if (roomsPlaced >= targetRoomCount) break; // Stop if we've placed enough rooms
                    nextRoom = neighborPos;
                }
            }


            if (nextRoom != Vector2Int.zero) currentPos = nextRoom; // Move to the next room position
            else
            {
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
        }
        if(roomsPlaced < targetRoomCount) 
        {
            GenerateLevel(); // If we couldn't place enough rooms, regenerate the level
        }
        else
        {
            GenerateBoss(currentPos);
        }      


    }
    


    private void PlaceRooms()
    {
        foreach (int value in levelGrid)
        {
            Debug.Log(value);
        }
    
        for (int i = 0; i < levelMaxHeight; i++)
        {
            for (int j = 0; j < levelMaxWidth; j++)
            { 
                int pos = levelGrid[i, j];
                if (pos == 1)
                {
                    // Instantiate room prefab
                    GameObject room = Instantiate(roomPrefab, new Vector3(i * 16, j * 9, 0), Quaternion.identity);
                    room.GetComponent<Room>().roomId = roomIds[i, j]; // Assign the room ID to the Room component
                }
                else if (pos == 2)
                {
                    // Instantiate start room prefab
                    GameObject startRoom = Instantiate(startPrefab, new Vector3(i * 16, j * 9, 0), Quaternion.identity);
                    startRoom.GetComponent<Room>().roomId = roomIds[i, j]; // Assign the room ID to the Room component
                }
                else if (pos == 3)
                {
                    // Instantiate boss room prefab
                    GameObject bossRoom = Instantiate(bossPrefab, new Vector3(i * 16, j * 9, 0), Quaternion.identity);
                    bossRoom.GetComponent<Room>().roomId = roomIds[i, j]; // Assign the room ID to the Room component

                }
                else if(pos == 4)
                {
                    // Instantiate sun barge room prefab
                    GameObject sunBargeRoom = Instantiate(sunBargePrefab, new Vector3(i * 16, j * 9, 0), Quaternion.identity);
                    sunBargeRoom.GetComponent<Room>().roomId = roomIds[i, j]; // Assign the room ID to the Room component
                }

            }
        }
        GameManager.Instance.SetLevelGrid(levelGrid);
        
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
            if (levelGrid[tempBoss.y, tempBoss.x] == 1 || levelGrid[tempBoss.y, tempBoss.x] == 2 || levelGrid[tempBoss.y, tempBoss.x] == 4)
                continue; // Room already exists

            levelGrid[tempBoss.y, tempBoss.x] = 3; // Mark the grid cell as occupied by boss room
            roomIds[tempBoss.y, tempBoss.x] = currentRoomId; // Assign a room ID to the boss room
            currentRoomId++; // Increment the room ID for the next room
            bossPlaced =true;
        }
        if (!bossPlaced)
        {
            GenerateLevel(); // If we couldn't place the boss room, regenerate the level
        }

}
}
