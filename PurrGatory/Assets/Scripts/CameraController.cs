using UnityEngine;

public class CameraController : MonoBehaviour
{

 public void UpdateCameraPosition()
    {
        transform.position = new Vector3(LevelManager.Instance.GetPlayerRoom().x * 16, (LevelManager.Instance.GetPlayerRoom().y * 9)+0.4f, -10);
    }
}
