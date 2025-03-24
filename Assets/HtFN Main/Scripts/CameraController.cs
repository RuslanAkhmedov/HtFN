using UnityEngine;
using Unity.Cinemachine;
public class CameraController : MonoBehaviour
{
    public GameObject playerModel;
    public CinemachineCamera virtualCamera;
    public CinemachineInputAxisController axisController;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Блокируем курсор
        Cursor.visible = false; // Скрываем курсор
        virtualCamera.Lens.FieldOfView = PlayerPrefs.GetFloat("FOV", 60f);
        float savedSensitivity = PlayerPrefs.GetFloat("CameraSensitivity", 1f);
        SetSensitivity(savedSensitivity);

        float invertY = PlayerPrefs.GetFloat("InvertY", 0f);
        bool isInverted = invertY > 0f;
        ApplyInvertY(!isInverted);
    }

    void Update()
    {
        // Берём направление вперёд из камеры
        Vector3 forward = Camera.main.transform.forward;
        // Сбрасываем наклон, чтобы не заваливаться вперёд/назад
        forward.y = 0f;
        forward.Normalize();
        // Если есть направление, делаем поворот
        if (forward.sqrMagnitude > 0.001f)
        {
            playerModel.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        }
    }

    public void SetSensitivity(float newSensitivity)
    {
        if (axisController == null)
        {
            Debug.LogError("axisController is null");
            return;
        }

        bool isYInverted = PlayerPrefs.GetFloat("InvertY", 0f) > 0f;
        bool foundX = false, foundY = false;

        foreach (var c in axisController.Controllers)
        {
            Debug.Log($"Controller found: {c.Name}");

            if (c.Name == "Look X (Pan)")
            {
                c.Input.Gain = newSensitivity;
                foundX = true;
                Debug.Log($"Set X gain to {newSensitivity}");
            }
            else if (c.Name == "Look Y (Tilt)")
            {
                // Применяем инвертирование согласно настройкам
                c.Input.Gain = isYInverted ? -newSensitivity : newSensitivity;
                foundY = true;
                Debug.Log($"Set Y gain to {c.Input.Gain} (inverted: {isYInverted})");
            }
        }

        if (!foundX || !foundY)
        {
            Debug.LogWarning($"Some controllers not found. X found: {foundX}, Y found: {foundY}");
        }

        // Сохраняем значение
        PlayerPrefs.SetFloat("CameraSensitivity", newSensitivity);
        PlayerPrefs.Save();
    }

    public void ApplyInvertY(bool isInverted)
    {
        if (axisController == null)
        {
            Debug.LogError("axisController is null");
            return;
        }

        float sensitivityValue = PlayerPrefs.GetFloat("CameraSensitivity", 1f);
        bool found = false;

        foreach (var c in axisController.Controllers)
        {
            if (c.Name == "Look Y (Tilt)")
            {
                c.Input.Gain = isInverted ? -sensitivityValue : sensitivityValue;
                found = true;
                Debug.Log($"Y axis inverted: {isInverted}, set gain to {c.Input.Gain}");
            }
        }

        if (!found)
        {
            Debug.LogWarning("Y controller not found when applying invert");
        }
    }
}