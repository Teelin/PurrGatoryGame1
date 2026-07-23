using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]bool followPlayer = false;

    private void Update()
    {
        if (followPlayer)
        {
            transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 0, -10);
        }
    }
    public void UpdateCameraPosition()
    {
        transform.position = new Vector3(LevelManager.Instance.GetPlayerRoom().x * 16, (LevelManager.Instance.GetPlayerRoom().y * 9) + 0.55f, -10);
    }
}
