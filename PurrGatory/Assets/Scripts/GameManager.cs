using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private int[,] levelGrid;
    private Vector2Int startingPosition;
    private Vector2Int playerRoom;

    public static GameManager Instance { get; private set; }

    Room[] roomList;

    int currentLevel = 1;
    int lives = 9;
    int kittensSaved = 0;
    int kittensSavedThisLevel = 0;

    private int minRooms, maxRooms;
    private int defaultMinRooms = 5, defaultMaxRooms = 10;

    public static UnityEvent OnLevelComplete = new UnityEvent();

    float timeTillDawn = 2400f; // 40 minutes in seconds
    float timeLastLevel =0f;

    float playerAttackDamage= 5f;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(Instance == null)
        {
            Instance = this;
        }
        else { Destroy(gameObject); } 

        maxRooms = defaultMaxRooms + (currentLevel - 1) * 2;
        minRooms = defaultMinRooms + (currentLevel - 1);

        GhostKitten.KittenSaved.AddListener(KittenSaved);
    }
    private void Update()
    {
       
    }
    private void OnDisable()
    {
        GhostKitten.KittenSaved.RemoveListener(KittenSaved);
    }


    //public int[,]GetLevelGrid()
    //{
    //    return levelGrid;
    //}

    //public void SetLevelGrid(int[,] newGrid)
    //{
    //    levelGrid = newGrid;
    //}

    //public Vector2Int GetStartingPosition()
    //{
    //    return startingPosition;
    //}
    //public void SetStartingPosition(Vector2Int newPosition)
    //{
    //    startingPosition = newPosition;
    //}

    //public Vector2Int GetPlayerRoom()
    //{
    //    return playerRoom;
    //}
    //public void SetPlayerRoom(Vector2Int newPosition)
    //{
    //    playerRoom = newPosition;
    //}

    //public void SetRoomList()
    //{
    //    roomList = null;
    //    roomList = FindObjectsByType<Room>();
    //}
    
    //public Room[] GetRoomList()
    //{
    //    return roomList;
    //}
    //public void DestroyRooms()
    //{
    //    var rooms = FindObjectsByType<Room>();
    //    if (rooms != null)
    //    {
    //        foreach (Room room in rooms)
    //        {
    //            Destroy(room.gameObject);
    //        }
    //    }
    //}

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void ProgressLevel()
    {
        currentLevel++;
    }
    
    public int GetLives()
    {
        return lives;
    }
    public void UpdateLives(int newLives)
    {
        lives = newLives;
    }

    public int GetMinRooms()
    {
        return minRooms;
    }

    public int GetMaxRooms()
    {
        return maxRooms;
    }

    public void LevelComplete()
    {

        ProgressLevel();
        maxRooms = defaultMaxRooms + (currentLevel - 1) * 2;
        minRooms = defaultMinRooms + (currentLevel - 1);
        OnLevelComplete?.Invoke();
        SceneManager.LoadScene("TestLevel");
    }

    public void RoundOver()
    {
        currentLevel = 1;
        lives = 9;
        maxRooms = defaultMaxRooms;
        minRooms = defaultMinRooms;
        kittensSaved = 0;
        LevelManager.Instance.ResetLevel();
    }

    public void KittenSaved() 
    {
        kittensSaved++;
    }
    public void SetKittensSavedThisLevel(int kittens)
    {
        kittensSavedThisLevel = kittens;
    }

    public int GetKittensSavedThisLevel()
    {
        return kittensSavedThisLevel;
    }
    public int GetKittensSaved()
    {
        return kittensSaved;
    }

    public void SetTimeLastLevel(float time)
    {
        timeLastLevel = time;
        timeTillDawn -= time;
    }

    public float GetTimeLastLevel()
    {
        return timeLastLevel;
    }
    public float GetTimeTillDawn()
    {
        return timeTillDawn;
    }

    public float GetPlayerAttackDamage()
    {
        return playerAttackDamage;
    }
}
