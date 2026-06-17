using UnityEngine;


public class GameManager : MonoBehaviour
{
    private int[,] levelGrid;
    private Vector2Int startingPosition;
    private Vector2Int playerRoom;

    public static GameManager Instance { get; private set; }

    Room[] roomList;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public int[,]GetLevelGrid()
    {
        return levelGrid;
    }

    public void SetLevelGrid(int[,] newGrid)
    {
        levelGrid = newGrid;
    }

    public Vector2Int GetStartingPosition()
    {
        return startingPosition;
    }
    public void SetStartingPosition(Vector2Int newPosition)
    {
        startingPosition = newPosition;
    }

    public Vector2Int GetPlayerRoom()
    {
        return playerRoom;
    }
    public void SetPlayerRoom(Vector2Int newPosition)
    {
        playerRoom = newPosition;
    }

    public void SetRoomList()
    {
        roomList = FindObjectsByType<Room>();
    }
    
    public Room[] GetRoomList()
    {
        return roomList;
    }
    public void DestroyRooms()
    {
        if (roomList != null)
        {
            foreach (Room room in roomList)
            {
                Destroy(room.gameObject);
            }
        }
    }

}
