using System;
using UnityEngine;


public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]

    private int minRooms, maxRooms;
    private int levelMaxSize;
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject roomPrefab, bossPrefab, startPrefab, sunBargePrefab;
    [SerializeField] private GameObject[] EasyRooms, MediumRooms, HardRooms;

    private int[,] levelGrid, roomIds;
    private int targetRoomCount, currentRoomId;

    private GameObject previousRoom, newRoom;

    public static event Action levelGenerated;

    private void Awake()
    {
        InitialiseLevel();
    }

    private void Start()
    {
        levelGenerated?.Invoke();
        Debug.Log($"Level generated with {targetRoomCount} rooms.");
    }

    public void InitialiseLevel()
    {
        Debug.Log(GameManager.Instance.GetCurrentLevel() );
        /*
        if (GameManager.Instance.GetCurrentLevel() <= 3)
        {
            if (EasyRooms != null && EasyRooms.Length > 0)
                roomPrefabs = EasyRooms;
        }
        else if (GameManager.Instance.GetCurrentLevel() <= 6)
        {
            if (MediumRooms != null && MediumRooms.Length > 0)
                roomPrefabs = MediumRooms;
        }
        else
        {
            if (HardRooms != null && HardRooms.Length > 0)
                roomPrefabs = HardRooms;
        }*/

        
        LevelManager.Instance.DestroyRooms();
        minRooms = GameManager.Instance.GetMinRooms();
        maxRooms = GameManager.Instance.GetMaxRooms();
        targetRoomCount = UnityEngine.Random.Range(minRooms, maxRooms + 1);
        GenerateLevel();
        LevelManager.Instance.SetLevelGrid(levelGrid);
        PlaceRooms();
        LevelManager.Instance.SetRoomList();
        levelGenerated?.Invoke();
        GameManager.Instance.SetGameState(GameManager.GameState.Playing);

    }
    public int CalculateStructuralGrid(int maxRooms)
    {
        // 1. Take the square root of the room count
        float sqrt = Mathf.Sqrt(maxRooms);

        // 2. Round UP to the nearest whole integer so we don't truncate needed space
        int gridSide = Mathf.CeilToInt(sqrt);

        // 3. Return a clean square matrix dimension (e.g., 4x4)
        return gridSide;
    }

    private void GenerateLevel()
    {
        levelMaxSize = CalculateStructuralGrid(targetRoomCount) * 2;
        levelGrid = new int[levelMaxSize, levelMaxSize]; // Reset the grid
        roomIds = new int[levelMaxSize, levelMaxSize]; // Reset the room IDs
        int roomsPlaced = 0; // Reset the number of rooms placed

        currentRoomId = 1; // Start room IDs from 1


        Vector2Int currentPos = new Vector2Int(levelMaxSize / 2, levelMaxSize / 2); // Start at the middle of the grid
        levelGrid[currentPos.x, currentPos.y] = 2; // Mark the starting room in the grid
        levelGrid[currentPos.x , currentPos.y -1 ] = 4; // Mark the SunBarge room in the grid
        roomIds[currentPos.x, currentPos.y] = currentRoomId; // Assign a room ID to the starting room

        currentRoomId++; // Increment the room ID for the next room
        LevelManager.Instance.SetStartingPosition(currentPos);
        /*int roomsPlacedThisIteration = 0;
        for (roomsPlaced = 0; roomsPlaced < targetRoomCount; roomsPlaced++)
        {

            Vector2Int nextRoom = Vector2Int.zero;
            roomsPlacedThisIteration = 0;
            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentPos + direction;

                if (neighborPos.x < 0 || neighborPos.x >= levelMaxSize || neighborPos.y < 0 || neighborPos.y >= levelMaxSize) continue;// Skip if out of bounds
                if (levelGrid[neighborPos.y, neighborPos.x] == 1 || levelGrid[neighborPos.y, neighborPos.x] == 2 || levelGrid[neighborPos.y, neighborPos.x] == 4) continue; // Skip if room already exists

                if (UnityEngine.Random.value < 0.5f) // 50% chance to place a room
                {
                    levelGrid[neighborPos.y, neighborPos.x] = 1; // Mark the grid cell as occupied
                    roomIds[neighborPos.y, neighborPos.x] = currentRoomId; // Assign a room ID to the new room
                
                    currentRoomId++; // Increment the room ID for the next room
                    //roomsPlaced++;
                    roomsPlacedThisIteration++;
                    if (roomsPlaced >= targetRoomCount) break; // Stop if we've placed enough rooms
                    nextRoom = neighborPos;
                    break; // Move to the next room position
                }
            }


            if (nextRoom != Vector2Int.zero) currentPos = nextRoom; // Move to the next room position
            else
            {
                
                foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                {
                    Vector2Int neighborPos = currentPos + direction;
                    if (neighborPos.x < 0 || neighborPos.x >= levelMaxSize || neighborPos.y < 0 || neighborPos.y >= levelMaxSize) continue;// Skip if out of bounds
                    if (levelGrid[neighborPos.y, neighborPos.x] == 1)
                    {
                        currentPos = neighborPos; // Move to the next room position
                        newRoom = previousRoom; // Update the new room reference to the previous room
                        break;
                    }
                }
            }
        }
        if (roomsPlaced < targetRoomCount) 
            {
                GenerateLevel(); // If we couldn't place enough rooms, regenerate the level
            }
            else
            {
                GenerateBoss(currentPos);
            }      
        */

        //Updated while loop from Copilot to ensure we don't get stuck in an infinite loop if we can't place enough rooms

        int maxAttempts = targetRoomCount * 10; // Safety limit to prevent infinite loops
        int attempts = 0;

        while (roomsPlaced < targetRoomCount && attempts < maxAttempts)
        {
            attempts++;
            Vector2Int nextRoom = Vector2Int.zero;
            bool foundValidNeighbor = false;

            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentPos + direction;

                if (neighborPos.x < 0 || neighborPos.x >= levelMaxSize || neighborPos.y < 0 || neighborPos.y >= levelMaxSize) continue;// Skip if out of bounds
                if (levelGrid[neighborPos.x, neighborPos.y] == 1 || levelGrid[neighborPos.x, neighborPos.y] == 2 || levelGrid[neighborPos.x, neighborPos.y] == 4) continue; // Skip if room already exists

                foundValidNeighbor = true; // We found at least one valid spot

                if (UnityEngine.Random.value < 0.5f) // 50% chance to place a room
                {
                    levelGrid[neighborPos.x, neighborPos.y] = 1; // Mark the grid cell as occupied
                    roomIds[neighborPos.x, neighborPos.y] = currentRoomId; // Assign a room ID to the new room

                    currentRoomId++; // Increment the room ID for the next room
                    roomsPlaced++;
                    if (roomsPlaced >= targetRoomCount) break; // Stop if we've placed enough rooms
                    nextRoom = neighborPos;
                    break; // Move to the next room position
                }
            }

            if (nextRoom != Vector2Int.zero)
            {
                currentPos = nextRoom; // Move to the next room position
            }
            else if (foundValidNeighbor)
            {
                // Valid spots exist but random chance failed, stay at current position and retry
                continue;
            }
            else
            {
                // No valid neighbors found, try to backtrack to an existing room
                bool foundBacktrack = false;
                foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
                {
                    Vector2Int neighborPos = currentPos + direction;
                    if (neighborPos.x < 0 || neighborPos.x >= levelMaxSize || neighborPos.y < 0 || neighborPos.y >= levelMaxSize) continue;// Skip if out of bounds
                    if (levelGrid[neighborPos.x, neighborPos.y] == 1)
                    {
                        currentPos = neighborPos; // Move to the next room position
                        newRoom = previousRoom; // Update the new room reference to the previous room
                        foundBacktrack = true;
                        break;
                    }
                }

                // If we can't backtrack either, we're stuck - regenerate
                if (!foundBacktrack)
                {
                    Debug.LogWarning($"Could not find valid backtrack position. Placed {roomsPlaced}/{targetRoomCount} rooms. Regenerating...");
                    GenerateLevel();
                    return;
                }
            }
        }

        // Check if we failed to place enough rooms
        if (roomsPlaced < targetRoomCount)
        {
            Debug.LogWarning($"Could not place enough rooms. Placed {roomsPlaced}/{targetRoomCount}. Regenerating...");
            GenerateLevel();
            return;
        }

        GenerateBoss(currentPos);
        Output2DArray(levelGrid); // Output the grid to the console for debugging

    }



    private void PlaceRooms()
    {
        // Safety check to ensure roomPrefabs is valid
        if (roomPrefabs == null || roomPrefabs.Length == 0)
        {
            Debug.LogError("roomPrefabs is null or empty! Make sure to assign room prefabs in the Inspector.");
            return;
        }

        for (int i = 0; i < levelMaxSize; i++)
        {
            for (int j = 0; j < levelMaxSize; j++)
            {
                int pos = levelGrid[j, i];
                if (pos == 1)
                {
                    // Instantiate room prefab
                    GameObject roomToSpawn = roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Length)];



                    GameObject room = Instantiate(roomToSpawn, new Vector3(j * 16, i * 9, 0), Quaternion.identity);
                    room.GetComponent<Room>().roomId = roomIds[j, i]; // Assign the room ID to the Room component
                    room.GetComponent<Room>().SetRoomPos(new Vector2Int(j, i)); // Set the room position in the Room component
                }
                else if (pos == 2)
                {
                    // Instantiate start room prefab
                    GameObject startRoom = Instantiate(startPrefab, new Vector3(j * 16, i * 9, 0), Quaternion.identity);
                    startRoom.GetComponent<Room>().roomId = roomIds[j, i]; // Assign the room ID to the Room component
                    startRoom.GetComponent<Room>().SetRoomPos(new Vector2Int(j, i)); // Set the room position in the Room component
                }
                else if (pos == 3)
                {
                    // Instantiate boss room prefab
                    GameObject bossRoom = Instantiate(bossPrefab, new Vector3(j * 16, i * 9, 0), Quaternion.identity);
                    bossRoom.GetComponent<Room>().roomId = roomIds[j, i]; // Assign the room ID to the Room component
                    bossRoom.GetComponent<Room>().SetRoomPos(new Vector2Int(j, i)); // Set the room position in the Room component
                }
                else if (pos == 4)
                {
                    // Instantiate sun barge room prefab
                    GameObject sunBargeRoom = Instantiate(sunBargePrefab, new Vector3(j * 16, i * 9, 0), Quaternion.identity);
                    sunBargeRoom.GetComponent<Room>().roomId = roomIds[j, i]; // Assign the room ID to the Room component
                    sunBargeRoom.GetComponent<Room>().SetRoomPos(new Vector2Int(j, i)); // Set the room position in the Room component
                }

            }
        }
        LevelManager.Instance.SetLevelGrid(levelGrid);

    }

    private void GenerateBoss(Vector2Int position)
    {
        bool bossPlaced = false;
        foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
        {
            if (bossPlaced) break; // Stop if boss room has been placed)
            Vector2Int tempBoss = position + direction;
            if (tempBoss.x < 0 || tempBoss.x >= levelMaxSize || tempBoss.y < 0 || tempBoss.y >= levelMaxSize)
                continue; // Skip if out of bounds
            if (levelGrid[tempBoss.x, tempBoss.y] == 1 || levelGrid[tempBoss.x, tempBoss.y] == 2 || levelGrid[tempBoss.x, tempBoss.y] == 4)
                continue; // Room already exists

            levelGrid[tempBoss.x, tempBoss.y] = 3; // Mark the grid cell as occupied by boss room
            roomIds[tempBoss.x, tempBoss.y] = currentRoomId; // Assign a room ID to the boss room
            currentRoomId++; // Increment the room ID for the next room
            bossPlaced = true;
        }
        if (!bossPlaced)
        {
            GenerateLevel(); // If we couldn't place the boss room, regenerate the level
        }

    }

    public void Output2DArray(int[,] array)
    {
        string output = "";
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                output += array[j, i] + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }


}
