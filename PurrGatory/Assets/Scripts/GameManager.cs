using System;
using Unity.AppUI.UI;
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
    int kittensAvailableToSpend = 0;

    private int minRooms, maxRooms;
    private int defaultMinRooms = 5, defaultMaxRooms = 10;

    public static UnityEvent OnLevelComplete = new UnityEvent();

    float timeTillDawn = 2400f; // 40 minutes in seconds
    float timeLastLevel =0f;

    
    [SerializeField] AudioClip menuMusic, gameMusic, bossMusic, miniBossMusic;
    AudioSource mainMusicSource;

    public enum GameState { Menu, Playing, Paused, GameOver, BossFight, MiniBossFight };

    static GameState gameState;
    bool musicPlaying = false;

    float speed = 5f, sightRange = 3f, rattleRange = 2f, rattleCooldown = 5f, playerAttackDamage = 5f;


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
        mainMusicSource = GetComponent<AudioSource>();
        mainMusicSource.clip = menuMusic; 
        mainMusicSource.Play();
        musicPlaying = true;
    }
    private void Update()
    {
        switch (gameState)
        {
            case GameState.Menu:
                mainMusicSource.clip = menuMusic;
                
                break;
            case GameState.Playing:
                mainMusicSource.clip = gameMusic;
                break;
            case GameState.BossFight:
                mainMusicSource.clip = bossMusic;
                break;
            case GameState.MiniBossFight:
                mainMusicSource.clip = miniBossMusic;
                break;
            case GameState.GameOver:
                mainMusicSource.clip = menuMusic;
                break;
            case GameState.Paused:
                mainMusicSource.clip = gameMusic;
                break;
        }
        if (musicPlaying == false)
        {
            mainMusicSource.Play();
            musicPlaying = true;
        }
    }

    public float GetSpeed()
    {
        return speed;
    }
    public float GetSightRange()
    {
        return sightRange;
    }
    public float GetRattleRange()
    {
        return rattleRange;
    }
    public float GetRattleCooldown()
    {
        return rattleCooldown;
    }
    public float GetDamage()
    {
        return playerAttackDamage;
    }

    public void SetGameState(GameState newState)
    {
        gameState = newState;
        musicPlaying = false;

    }
    private void OnDisable()
    {
        GhostKitten.KittenSaved.RemoveListener(KittenSaved);
    }

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
        SetGameState(GameState.Playing);
    }

    public void RoundOver()
    {
        currentLevel = 1;
        lives = 9;
        maxRooms = defaultMaxRooms;
        minRooms = defaultMinRooms;
        if(SceneManager.GetActiveScene().name != "EndLevel")
        {
            SceneManager.LoadScene("EndLevel");
        }
        else
        {
            SceneManager.LoadScene("TestLevel");
        }
    }

    public void KittenSaved() 
    {
        kittensSaved++;
    }
    public void SetKittensSavedThisLevel(int kittens)
    {
        kittensSavedThisLevel = kittens;
        kittensAvailableToSpend += kittens;
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

    public bool SpendKittens(int kittens)
    {
        if (kittens <= kittensAvailableToSpend)
        {
            kittensAvailableToSpend -= kittens;
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough kittens available to spend.");
            return false;
        }
    }
    public int GetKittensAvailableToSpend()
    {
        return kittensAvailableToSpend;
    }
    public void AddLife()
    {
        lives++;
    }

    public void UpdateDamage(float newDamage)
    {
        playerAttackDamage = newDamage;
    }
    public void UpdateRattleRange(float newRange)
    {
        rattleRange = newRange;
    }
    public void UpdateRattleCooldown(float newCooldown)
    {
        rattleCooldown = newCooldown;
    }
    public void UpdateSightRange(float newRange)
    {
        sightRange = newRange;
    }
    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
