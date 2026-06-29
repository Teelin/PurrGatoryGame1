using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Room : MonoBehaviour
{
    private Vector2Int roomPos;

    public int roomId;

    [SerializeField] bool isBossRoom, isStartRoom, isSunBargeRoom; 
    
    [SerializeField] GameObject barrierTop, barrierBottom, barrierLeft, barrierRight;

    [SerializeField] bool hasSacredFire = false, isScaredFireActive;

    [SerializeField] GameObject sacredFirePrefab;


    private void Awake()
    {
        LevelGenerator.levelGenerated += OpenBarriers;
    }
    private void Start()
    {
        SetBossRoomBarrier();
       
    }
    private void OnDisable()
    {
        LevelGenerator.levelGenerated -= OpenBarriers;
    }

    private void Update()
    {
        if (isStartRoom)
        {
            if(LevelManager.Instance.IsBossDefeated())
            {
                barrierBottom.SetActive(false);
            }
        }
        if(isScaredFireActive)
        {
            // Add logic for when the sacred fire is active
            sacredFirePrefab.SetActive(true);
        }
    }


    private void SetBossRoomBarrier()
    {
        if (isBossRoom)
        {
            foreach (Room room in LevelManager.Instance.GetRoomList())
            {
                if (room.roomId == roomId - 1)
                {
                    Vector2 direction = room.transform.position - transform.position;
                    if (direction.x > 0)
                    {
                        barrierRight.SetActive(false);
                    }
                    else if (direction.x < 0)
                    {
                        barrierLeft.SetActive(false);
                    }
                    else if (direction.y > 0)
                    {
                        barrierTop.SetActive(false);
                    }
                    else if (direction.y < 0)
                    {
                        barrierBottom.SetActive(false);
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
                        }
                        else if (direction == Vector2Int.down)
                        {
                            barrierBottom.SetActive(false);
                        }
                        else if (direction == Vector2Int.left)
                        {
                            barrierLeft.SetActive(false);
                        }
                        else if (direction == Vector2Int.right)
                        {
                            barrierRight.SetActive(false);
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
