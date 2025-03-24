using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject settingsCanvas;
    public GameObject gamePanel;
    public GameObject graphicsPanel;

    public CameraController cameraController;

    public Slider sliderFOV;
    public Slider sliderSens;
    public Toggle invertYToggle;
    public Toggle VSyncToggle;
    public CinemachineCamera virtualCamera;

    private bool isPaused = false;

    void Start()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

        // ��������� FOV
        float savedFOV = PlayerPrefs.GetFloat("FOV", 60f);
        sliderFOV.value = savedFOV;
        sliderFOV.onValueChanged.AddListener(OnFovUpdate);

        // ��������� ����������������
        float savedSens = PlayerPrefs.GetFloat("CameraSensitivity", 1f);
        sliderSens.value = savedSens;
        sliderSens.onValueChanged.AddListener(OnSensUpdate);

        // ��������� �������������� Y
        float invertY = PlayerPrefs.GetFloat("InvertY", 0f);
        invertYToggle.isOn = invertY > 0f;
        invertYToggle.onValueChanged.AddListener(OnInvertYChanged);

        // ��������� �������������� ��� �������
        cameraController.ApplyInvertY(invertYToggle.isOn);


        int VSyncbool = PlayerPrefs.GetInt("VSync", 0);
        VSyncToggle.isOn = VSyncbool > 0;
        VSyncToggle.onValueChanged.AddListener(OnVSyncChanged);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsCanvas.activeSelf)
        {
            isPaused = !isPaused;
            pauseCanvas.SetActive(isPaused);

            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None; // ������������ ������
                Cursor.visible = true; // ������ ��� �������
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked; // ��������� ������
                Cursor.visible = false; // �������� ������
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && settingsCanvas.activeSelf)
        {
            pauseCanvas.SetActive(true);
            settingsCanvas.SetActive(false);
        }

    }

    public void OnResumeClick ()
    {
        pauseCanvas.SetActive(false);
        Cursor.visible = false;

    }

    public void OnSettingsClick ()
    {
        // panels
        gamePanel.SetActive(true);
        graphicsPanel.SetActive(false);


        settingsCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
    }

    public void OnFovUpdate(float newFOV)
    {
        virtualCamera.Lens.FieldOfView = newFOV;
        PlayerPrefs.SetFloat("FOV", newFOV);
        PlayerPrefs.Save();
    }

    public void OnSensUpdate(float newSens)
    {
        cameraController.SetSensitivity(newSens);
        PlayerPrefs.SetFloat("CameraSensitivity", newSens);
        PlayerPrefs.Save();
    }

    public void OnReturnToMenuClick()
    {
        pauseCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void OnClickGameSettings()
    {
        gamePanel.SetActive(true);
        graphicsPanel.SetActive(false);
    }

    public void OnClickGraphicsSettings()
    {
        gamePanel.SetActive(false);
        graphicsPanel.SetActive(true);
    }


    public void OnInvertYChanged(bool isInverted)
    {
        PlayerPrefs.SetFloat("InvertY", isInverted ? 1f : 0f);
        PlayerPrefs.Save();

        cameraController.ApplyInvertY(!isInverted);
    }
    public void OnVSyncChanged(bool isVSync)
    {
        // ��������� ��������� � PlayerPrefs
        PlayerPrefs.SetInt("VSync", isVSync ? 1 : 0);
        PlayerPrefs.Save();

        // ��������� ��������� VSync
        QualitySettings.vSyncCount = isVSync ? 1 : 0;

        Debug.Log($"VSync ������� ��: {isVSync}, ����������� ��������: {QualitySettings.vSyncCount}");
    }

}
