using UnityEngine;

public class Room : MonoBehaviour
{
    private Vector2 roomPos;

    public int roomId;

    [SerializeField] bool isBossRoom, isStartRoom, isSunBargeRoom;
    [SerializeField] GameObject barrierTop, barrierBottom, barrierLeft, barrierRight;



    private void Start()
    {
        SetBossRoomBarrier();
    }


    private void SetBossRoomBarrier()
    {
        if (isBossRoom)
        {
            foreach (Room room in GameManager.Instance.GetRoomList())
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

    public void OpenBarrier(Vector2 dir)
    {
        if (!isBossRoom && !isStartRoom && !isSunBargeRoom)
        {
            if (dir.x > 0)
            {
                barrierRight.SetActive(false);
            }
            else if (dir.x < 0)
            {
                barrierLeft.SetActive(false);
            }
            else if (dir.y > 0)
            {
                barrierTop.SetActive(false);
            }
            else if (dir.y < 0)
            {
                barrierBottom.SetActive(false);
            }
        }
    }




}
