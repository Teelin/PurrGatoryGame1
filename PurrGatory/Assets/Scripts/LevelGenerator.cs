using System;
using System.Text;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]
    private int minRooms, maxRooms;
    private int levelMaxSize;
    [SerializeField] private GameObject[] roomPrefabs;
    [SerializeField] private GameObject bossPrefab, startPrefab, sunBargePrefab;
    [SerializeField] private GameObject[] EasyRooms, MediumRooms, HardRooms;

    private int[,] levelGrid, roomIds;
    private int targetRoomCount, currentRoomId;

    public static event Action levelGenerated;

    private void Awake()
    {
        InitialiseLevel();
    }

    private void Start()
    {
        Debug.Log($"Level generated with {targetRoomCount} rooms.");
        levelGenerated?.Invoke();
    }

    public void InitialiseLevel()
    {
        LevelManager.Instance.DestroyRooms();
        minRooms = GameManager.Instance.GetMinRooms();
        maxRooms = GameManager.Instance.GetMaxRooms();
        targetRoomCount = UnityEngine.Random.Range(minRooms, maxRooms + 1);

        GenerateLevel();

        LevelManager.Instance.SetLevelGrid(levelGrid);
        PlaceRooms();
        LevelManager.Instance.SetRoomList();

        
        GameManager.Instance.SetGameState(GameManager.GameState.Playing);
    }

    public int CalculateStructuralGrid(int maxRooms)
    {
        float sqrt = Mathf.Sqrt(maxRooms);
        return Mathf.CeilToInt(sqrt);
    }

    private void GenerateLevel()
    {
        levelMaxSize = Mathf.Max(CalculateStructuralGrid(targetRoomCount) * 2, 4); // Minimum grid boundary safety
        levelGrid = new int[levelMaxSize, levelMaxSize];
        roomIds = new int[levelMaxSize, levelMaxSize];

        int roomsPlaced = 0;
        currentRoomId = 1;

        Vector2Int currentPos = new Vector2Int(levelMaxSize / 2, levelMaxSize / 2);

        // Starting Room
        levelGrid[currentPos.x, currentPos.y] = 2;
        roomIds[currentPos.x, currentPos.y] = currentRoomId++;

        // SunBarge Room placement with bounds check
        Vector2Int sunBargePos = currentPos + Vector2Int.down;
        if (sunBargePos.y >= 0)
        {
            levelGrid[sunBargePos.x, sunBargePos.y] = 4;
            roomIds[sunBargePos.x, sunBargePos.y] = currentRoomId++;
        }

        LevelManager.Instance.SetStartingPosition(currentPos);

        int maxAttempts = targetRoomCount * 10;
        int attempts = 0;

        while (roomsPlaced < targetRoomCount && attempts < maxAttempts)
        {
            attempts++;
            Vector2Int nextRoom = Vector2Int.zero;
            bool foundValidNeighbor = false;

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

            foreach (Vector2Int direction in directions)
            {
                Vector2Int neighborPos = currentPos + direction;

                if (neighborPos.x < 0 || neighborPos.x >= levelMaxSize || neighborPos.y < 0 || neighborPos.y >= levelMaxSize) continue;
                if (levelGrid[neighborPos.x, neighborPos.y] != 0) continue; // Any occupied cell

                foundValidNeighbor = true;

                if (UnityEngine.Random.value < 0.5f)
                {
                    levelGrid[neighborPos.x, neighborPos.y] = 1;
                    roomIds[neighborPos.x, neighborPos.y] = currentRoomId++;
                    roomsPlaced++;

                    if (roomsPlaced >= targetRoomCount) break;
                    nextRoom = neighborPos;
                    break;
                }
            }

            if (nextRoom != Vector2Int.zero)
            {
                currentPos = nextRoom;
            }
            else if (foundValidNeighbor)
            {
                continue;
            }
            else
            {
                // Backtracking logic
                bool foundBacktrack = false;
                foreach (Vector2Int direction in directions)
                {
                    Vector2Int neighborPos = currentPos + direction;
                    if (neighborPos.x < 0 || neighborPos.x >= levelMaxSize || neighborPos.y < 0 || neighborPos.y >= levelMaxSize) continue;

                    if (levelGrid[neighborPos.x, neighborPos.y] == 1)
                    {
                        currentPos = neighborPos;
                        foundBacktrack = true;
                        break;
                    }
                }

                if (!foundBacktrack)
                {
                    Debug.LogWarning("Stuck in generation. Restarting level creation...");
                    GenerateLevel();
                    return;
                }
            }
        }

        if (roomsPlaced < targetRoomCount)
        {
            Debug.LogWarning("Could not place enough rooms. Restarting generation...");
            GenerateLevel();
            return;
        }

        GenerateBoss(currentPos);
        Output2DArray(levelGrid);
    }

    private void PlaceRooms()
    {
        if (roomPrefabs == null || roomPrefabs.Length == 0)
        {
            Debug.LogError("roomPrefabs is null or empty!");
            return;
        }

        for (int i = 0; i < levelMaxSize; i++)
        {
            for (int j = 0; j < levelMaxSize; j++)
            {
                int pos = levelGrid[j, i];
                if (pos == 0) continue;

                GameObject prefabToSpawn = pos switch
                {
                    1 => roomPrefabs[UnityEngine.Random.Range(0, roomPrefabs.Length)],
                    2 => startPrefab,
                    3 => bossPrefab,
                    4 => sunBargePrefab,
                    _ => null
                };

                if (prefabToSpawn != null)
                {
                    GameObject room = Instantiate(prefabToSpawn, new Vector3(j * 16, i * 9, 0), Quaternion.identity);
                    if (room.TryGetComponent<Room>(out var roomComp))
                    {
                        roomComp.roomId = roomIds[j, i];
                        roomComp.SetRoomPos(new Vector2Int(j, i));
                    }
                }
            }
        }
        LevelManager.Instance.SetLevelGrid(levelGrid);
    }

    private void GenerateBoss(Vector2Int position)
    {
        bool bossPlaced = false;
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int tempBoss = position + direction;
            if (tempBoss.x < 0 || tempBoss.x >= levelMaxSize || tempBoss.y < 0 || tempBoss.y >= levelMaxSize) continue;
            if (levelGrid[tempBoss.x, tempBoss.y] != 0) continue;

            levelGrid[tempBoss.x, tempBoss.y] = 3;
            roomIds[tempBoss.x, tempBoss.y] = currentRoomId++;
            bossPlaced = true;
            break;
        }

        if (!bossPlaced)
        {
            GenerateLevel();
        }
    }

    public void Output2DArray(int[,] array)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                sb.Append(array[j, i]).Append(" ");
            }
            sb.AppendLine();
        }
        Debug.Log(sb.ToString());
    }
}