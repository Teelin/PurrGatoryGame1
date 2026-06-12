using UnityEngine;

public class CameraController : MonoBehaviour
{

 public void UpdateCameraPosition()
    {
        transform.position = new Vector3(GameManager.Instance.GetPlayerRoom().x * 16, GameManager.Instance.GetPlayerRoom().y * 9, -10);
    }
}
