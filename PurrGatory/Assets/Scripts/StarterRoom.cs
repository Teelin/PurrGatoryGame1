using UnityEngine;

public class StarterRoom : MonoBehaviour
{
    private Vector2Int roomPos;
    [SerializeField] private GameObject sunBargePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomPos = GameManager.Instance.GetStartingPosition();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAdjacentRooms()
    {
        int[,] levelGrid = GameManager.Instance.GetLevelGrid();
        Debug.Log("Checking adjacent rooms for sun barge placement...");
        Debug.Log(levelGrid[roomPos.y, roomPos.x]);
    
    Vector2Int[] directions = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborPos = roomPos + direction;
            if (levelGrid[neighborPos.y, neighborPos.x] != 1 && levelGrid[neighborPos.y, neighborPos.x] != 3)
            {
                Debug.Log("Placing sun barge at: " + neighborPos);
                Instantiate(sunBargePrefab, new Vector3(neighborPos.x * 16, neighborPos.y * 9, 0), Quaternion.identity);
                break; // Only need to place one sun barge for the first adjacent empty space
            }

        }
    }
}
