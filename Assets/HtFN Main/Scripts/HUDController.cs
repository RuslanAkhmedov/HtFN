using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public PlayerController playerController;

    public MovementController moveController;

    public TextMeshProUGUI speedHUD;
    public TextMeshProUGUI fpsHUD;
    public TextMeshProUGUI stamineHUD;


    private float deltaTime = 0.0f;
    private float fps;


    void Update()
    {

        deltaTime += (Time.deltaTime - deltaTime) * 0.1f; // Сглаживание значения FPS
        fps = 1.0f / deltaTime;
    }

    private void FixedUpdate()
    {
        speedHUD.text = "speed: " + moveController.moveSpeed.ToString("F2");

        fpsHUD.text = "fps: " + fps.ToString("F2");

        stamineHUD.text = "stamine: " + playerController.stamine.ToString("F2");

    }
}
