using System.Collections;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    int kittensSaved = 0;
    int kittensToSave = 0;
    int kittensNeedSaving = 0;
    int enemyCount = 0;

    private int[,] levelGrid;
    private Vector2Int startingPosition;
    private Vector2Int playerRoom;
    Room[] roomList;

    public bool isBossDefeated = false;
    private bool isLevelComplete = false;

    float timeToCompleteLevel = 0f;

    [SerializeField] GameObject hudUI, endLevelUI;

    [Header("Apophis Loading Screen")]
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private GameObject apophisLoadingScreen;

    public static LevelManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        GhostKitten.KittenSaved.AddListener(KittenSaved);
        GameManager.OnLevelComplete.AddListener(ResetLevel);
        CalculateSpawns();
        Time.timeScale = 1f;

    }
    private void OnDisable()
    {
        GhostKitten.KittenSaved.RemoveListener(KittenSaved);
        GameManager.OnLevelComplete.RemoveListener(ResetLevel);
    }
    private void Update()
    {
        if (!isLevelComplete)
        {
            timeToCompleteLevel += Time.deltaTime;

            if (timeToCompleteLevel + (GameManager.Instance.GetMaxTimeTillDawn() - GameManager.Instance.GetTimeTillDawn()) >= GameManager.Instance.GetMaxTimeTillDawn())
            {
                GameManager.Instance.SetGameState(GameManager.GameState.Menu);
                apophisLoadingScreen.SetActive(true);
                SceneManager.LoadSceneAsync("Apophis");
                Time.timeScale = 0f;
            }
        }
    }

    IEnumerator LoadApophisSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(5f); // Wait for 5 seconds
        SceneManager.LoadSceneAsync(sceneName);
    }
    public void KittenSaved()
    {
        kittensSaved++;
    }

    public void ResetLevel()
    {
        kittensSaved = 0;
        CalculateSpawns();
    }

    void CalculateSpawns()
    {
        kittensToSave = GameManager.Instance.GetCurrentLevel() + 3;  // Example: Increase urn count based on current level TODO: Make this a more complex formula based on level and difficulty
        enemyCount = GameManager.Instance.GetCurrentLevel() * 2; // Example: Increase enemy count based on current level TODO: Make this a more complex formula based on level and difficulty
        kittensNeedSaving = kittensToSave/2; // Example: Half of the kittens need saving, the rest are optional. TODO: Make this a more complex formula based on level and difficulty
    }

    public float GetTimeToCompleteLevel() { return timeToCompleteLevel; }

    public int GetKittensThisLevel() { return kittensToSave; }
    public int GetKittensSaved() { return kittensSaved; }
    public int GetKittensNeedSaving() { return kittensNeedSaving; }
    public int GetEnemyCount() { return enemyCount;}

    public bool IsBossDefeated() { return isBossDefeated; }

    public int[,] GetLevelGrid() { return levelGrid; }

    public void SetLevelGrid(int[,] newGrid) { levelGrid = newGrid; }

    public Vector2Int GetStartingPosition() { return startingPosition; }
    public void SetStartingPosition(Vector2Int newPosition) { startingPosition = newPosition; }

    public Vector2Int GetPlayerRoom() { return playerRoom; }
    public void SetPlayerRoom(Vector2Int newPosition) { playerRoom = newPosition; }

    public void SetRoomList()
    {
        roomList = null;
        roomList = FindObjectsByType<Room>();
    }
    public Room[] GetRoomList() { return roomList; }
    public void DestroyRooms()
    {
        var rooms = FindObjectsByType<Room>();
        if (rooms != null)
        {
            foreach (Room room in rooms)
            {
                Destroy(room.gameObject);
            }
        }
    }

    public void LevelCompletedCheck()
    {
        if (kittensSaved >= kittensNeedSaving && isBossDefeated)
        {
            isLevelComplete = true;
            GameManager.Instance.SetTimeLastLevel(timeToCompleteLevel);
            GameManager.Instance.SetKittensSavedThisLevel(kittensSaved);
            GameManager.Instance.SetGameState(GameManager.GameState.Menu);
            Time.timeScale = 0f;
            hudUI.SetActive(false);
            endLevelUI.SetActive(true);
        }
    }
}
