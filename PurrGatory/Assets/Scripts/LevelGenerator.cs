//using System;
using UnityEngine;
using UnityEngine.UIElements;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private int minRooms, maxRooms;
    [SerializeField] private int levelMaxWidth, levelMaxHeight;
    [SerializeField] private GameObject roomPrefab;

    private int[,] levelGrid;
    private int targetRoomCount;

    private void Start()
    {
        targetRoomCount = Random.Range(minRooms, maxRooms + 1);
        GenerateLevel();

    }

    private void GenerateLevel()
    {
        levelGrid = new int[levelMaxHeight, levelMaxWidth]; // Reset the grid


        Vector2Int nextRoom = Vector2Int.zero;
        Vector2Int currentPos = new Vector2Int(levelMaxWidth / 2, levelMaxHeight / 2); // Start in the middle of the grid
        while (targetRoomCount > 0)
        {
            PlaceRoom(currentPos);

            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                Vector2Int neighborPos = currentPos + direction;
                if (neighborPos.x < 0 || neighborPos.x >= levelMaxWidth || neighborPos.y < 0 || neighborPos.y >= levelMaxHeight) continue;// Skip if out of bounds
                if (levelGrid[neighborPos.y, neighborPos.x] == 1) continue; // Skip if room already exists

                if (Random.value < 0.5f) // 50% chance to place a room
                {
                    PlaceRoom(neighborPos);
                    if (targetRoomCount <= 0) break; // Stop if we've placed enough rooms
                    nextRoom = neighborPos;
                }

            }
            if (nextRoom != Vector2Int.zero)
                currentPos = nextRoom; // Move to the next room position
            else
                currentPos += Vector2Int.up; // If no new room was placed, just move up to continue the process
        }
        Debug.Log($"Generated level with {targetRoomCount} rooms.");
        foreach(int pos in levelGrid)
        {
            Debug.Log(pos);
        }



    }

    private void PlaceRoom(Vector2Int position)
    {
        if (levelGrid[position.y, position.x] == 1)
            return; // Room already exists
        levelGrid[position.y, position.x] = 1; // Mark the grid cell as occupied

        Instantiate(roomPrefab, new Vector3(position.x , position.y , 0), Quaternion.identity); // Instantiate room prefab
        targetRoomCount--;
        

    }

}
