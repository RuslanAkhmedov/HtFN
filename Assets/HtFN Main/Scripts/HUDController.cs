using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public MovementController moveController;
    public TextMeshProUGUI textMeshPro;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text = "speed: " + moveController.moveSpeed;
    }
}
