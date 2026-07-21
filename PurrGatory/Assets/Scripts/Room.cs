using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Room : MonoBehaviour
{
    private Vector2Int roomPos;

    public int roomId;

    [SerializeField] bool isBossRoom, isStartRoom, isSunBargeRoom; 
    
    [SerializeField] GameObject barrierTop, barrierBottom, barrierLeft, barrierRight;
    [SerializeField] GameObject sconceTop, sconceBottom, sconceLeft, sconceRight;

    [SerializeField] bool hasSacredFire = false, isScaredFireActive;

    [SerializeField] GameObject sacredFirePrefab;

    GameObject bossDoor;
    bool playerInBossRoom = false;

    public static UnityEvent BossRoomEntered = new UnityEvent();

    [SerializeField] GameObject miniMapMask;
    [SerializeField] TextMeshProUGUI exitPopUpText;




    private void Awake()
    {
        LevelGenerator.levelGenerated += OpenBarriers;
        if(!isBossRoom && !isSunBargeRoom)
        {
            sconceTop.SetActive(false);
            sconceLeft.SetActive(false);
            sconceRight.SetActive(false);
            sconceBottom.SetActive(false);
        }
        

    }
    private void Start()
    {
        
        miniMapMask.SetActive(false);
       
    }
    private void OnDisable()
    {
        LevelGenerator.levelGenerated -= OpenBarriers;
    }

    private void Update()
    {
        if (isStartRoom )
        {
            if(LevelManager.Instance.IsBossDefeated())
            {
                barrierBottom.SetActive(false);
                exitPopUpText.text = "Boss Defeated! You can now exit.";
            }
        }
        if (isSunBargeRoom)
        {
            if (LevelManager.Instance.IsBossDefeated())
            {
                barrierTop.SetActive(false);
            }
        }
        if (isScaredFireActive)
        {
            // Add logic for when the sacred fire is active
            sacredFirePrefab.SetActive(true);
        }

        if (isBossRoom)
        {
            if (LevelManager.Instance.GetPlayerRoom() == roomPos && LevelManager.Instance.isBossDefeated == false && !playerInBossRoom)
            {
                bossDoor.SetActive(true);
                BossRoomEntered?.Invoke();
                playerInBossRoom = true;
                GameManager.Instance.SetGameState(GameManager.GameState.BossFight);
            }
            if(LevelManager.Instance.isBossDefeated == true)
            {
                bossDoor.SetActive(false);
                GameManager.Instance.SetGameState(GameManager.GameState.Playing);
            }
            if (LevelManager.Instance.GetKittensSaved() >= LevelManager.Instance.GetKittensNeedSaving())
            {
                SetBossRoomBarrier();
            }
        
        }
        if(LevelManager.Instance.GetPlayerRoom() == roomPos)
            miniMapMask.SetActive(true);
            

    }


    private void SetBossRoomBarrier()
    {
        if (isBossRoom)
        {
            var levelGrid = LevelManager.Instance.GetLevelGrid();

            int x = roomPos.x;
            int y = roomPos.y;

            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                int newX = x + direction.x;
                int newY = y + direction.y;
                if (newX >= 0 && newX < levelGrid.GetLength(0) && newY >= 0 && newY < levelGrid.GetLength(1))
                {
                    if (levelGrid[newX, newY] == 1)
                    {
                        if (direction == Vector2Int.up)
                        {
                            barrierTop.SetActive(false);
                            bossDoor = barrierTop;
                            break;
                        }
                        else if (direction == Vector2Int.down)
                        {
                            barrierBottom.SetActive(false);
                            bossDoor = barrierBottom;
                            break;
                        }
                        else if (direction == Vector2Int.left)
                        {
                            barrierLeft.SetActive(false);
                            bossDoor = barrierLeft;
                            break;
                        }
                        else if (direction == Vector2Int.right)
                        {
                            barrierRight.SetActive(false);
                            bossDoor = barrierRight;
                            break;
                        }
                    }
                }
            }
            
        }
    }

    void OpenBarriers()
    {
        

        if (!isBossRoom  && !isSunBargeRoom)
        {
            var levelGrid = LevelManager.Instance.GetLevelGrid();

            int x = roomPos.x;
            int y = roomPos.y;
            

            foreach (Vector2Int direction in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
            {
                int newX = x + direction.x;
                int newY = y + direction.y;
                if (newX >= 0 && newX < levelGrid.GetLength(0) && newY >= 0 && newY < levelGrid.GetLength(1))
                {
                    if (levelGrid[newX, newY] == 1 || levelGrid[newX, newY] == 2 || levelGrid[newX, newY] == 3)
                    {
                        if (direction == Vector2Int.up)
                        {
                            barrierTop.SetActive(false);
                            sconceTop.SetActive(true);
                        }
                        else if (direction == Vector2Int.down)
                        {
                            barrierBottom.SetActive(false);
                            sconceBottom.SetActive(true);
                        }
                        else if (direction == Vector2Int.left)
                        {
                            barrierLeft.SetActive(false);
                            sconceLeft.SetActive(true);
                        }
                        else if (direction == Vector2Int.right)
                        {
                            barrierRight.SetActive(false);
                            sconceRight.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void SetRoomPos(Vector2Int pos)
    {
        roomPos = pos;
    }

    public bool GetStartRoomStatus()
    {
        return isStartRoom;
    }

    public void SetSacredFireActive(bool status)
    {
        isScaredFireActive = status;
    }

    public bool GetSacredFireStatus()
    {
        return hasSacredFire;
    }

}
