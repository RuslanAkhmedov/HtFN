using UnityEngine;
using Unity.Cinemachine;


public class CameraController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public CinemachineCamera virtualCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор
        Cursor.visible = false; // Скрываем курсор
        virtualCamera.Lens.FieldOfView = 90f; 
    }


}
