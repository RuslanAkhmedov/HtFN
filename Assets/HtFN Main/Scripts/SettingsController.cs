using System;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class SettingsController : MonoBehaviour
{
    public GameObject pauseCanvas;
    public GameObject settingsCanvas;
    public GameObject gamePanel;
    public GameObject graphicsPanel;
    public GameObject controllerPanel;

    public CameraController cameraController;

    public Slider sliderFOV;
    public Slider sliderSens;
    public Toggle invertYToggle;
    public Toggle VSyncToggle;

    public TMP_Dropdown fpsDropDown;
    public TMP_InputField fpsInputField;
    public CinemachineCamera virtualCamera;
    [HideInInspector]
    public bool isPaused = false; 


    void Start()
    {
        pauseCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

        // Настройка FOV
        float savedFOV = PlayerPrefs.GetFloat("FOV", 60f);
        sliderFOV.value = savedFOV;
        sliderFOV.onValueChanged.AddListener(OnFovUpdate);

        // Настройка чувствительности
        float savedSens = PlayerPrefs.GetFloat("CameraSensitivity", 1f);
        sliderSens.value = savedSens;
        sliderSens.onValueChanged.AddListener(OnSensUpdate);

        // Настройка инвертирования Y
        float invertY = PlayerPrefs.GetFloat("InvertY", 0f);
        invertYToggle.isOn = invertY > 0f;
        invertYToggle.onValueChanged.AddListener(OnInvertYChanged);

        // Применяем инвертирование при запуске
        cameraController.ApplyInvertY(invertYToggle.isOn);

        // Настройка VSync
        int VSyncbool = PlayerPrefs.GetInt("VSync", 0);
        VSyncToggle.isOn = VSyncbool > 0;
        VSyncToggle.onValueChanged.AddListener(OnVSyncChanged);

        // fps 

        int savedFPSinput = PlayerPrefs.GetInt("FPSInput", 60);
        fpsInputField.text = savedFPSinput.ToString();

        int caseFpsDropDown = PlayerPrefs.GetInt("FPSDropDown", 0);
        OnDropdownValueChanged(caseFpsDropDown);
        fpsDropDown.value = caseFpsDropDown;



        fpsDropDown.onValueChanged.AddListener(OnDropdownValueChanged);
        fpsInputField.onValueChanged.AddListener(OnInputChanged);
        fpsInputField.onEndEdit.AddListener(OnInputEnd);


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !settingsCanvas.activeSelf)
        {
            isPaused = !isPaused;
            pauseCanvas.SetActive(isPaused);

            if (isPaused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && settingsCanvas.activeSelf)
        {
            pauseCanvas.SetActive(true);
            settingsCanvas.SetActive(false);
        }
    }

    public void OnResumeClick()
    {
        pauseCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }

    public void OnExitClick()
    {
        SceneManager.LoadScene("1.MainMenu");
    }

    public void OnSettingsClick()
    {
        gamePanel.SetActive(true);
        graphicsPanel.SetActive(false);
        controllerPanel.SetActive(false);

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
        controllerPanel.SetActive(false);
    }

    public void OnClickGraphicsSettings()
    {
        gamePanel.SetActive(false);
        graphicsPanel.SetActive(true);
        controllerPanel.SetActive(false);
    }
    public void OnClickControllerSettings()
    {
        gamePanel.SetActive(false);
        graphicsPanel.SetActive(false);
        controllerPanel.SetActive(true);
    }

    public void OnInvertYChanged(bool isInverted)
    {
        // Сохраняем в PlayerPrefs
        PlayerPrefs.SetFloat("InvertY", isInverted ? 1f : 0f);
        PlayerPrefs.Save();

        // Передаём точно такое же значение в камеру
        cameraController.ApplyInvertY(isInverted);
    }

    public void OnVSyncChanged(bool isVSync)
    {
        PlayerPrefs.SetInt("VSync", isVSync ? 1 : 0);
        PlayerPrefs.Save();

        QualitySettings.vSyncCount = isVSync ? 1 : 0;
        Debug.Log($"VSync изменен на: {isVSync}, установлено значение: {QualitySettings.vSyncCount}");
    }

    void OnDropdownValueChanged(int index)
    {
        fpsInputField.gameObject.SetActive(index == 3); // Показываем только при index == 3

        PlayerPrefs.SetInt("FPSDropDown", index);
        PlayerPrefs.Save();

        switch (index)
        {
            case 0: Application.targetFrameRate = -1; break; // Безлимит
            case 1: Application.targetFrameRate = 60; break;
            case 2: Application.targetFrameRate = 90; break;
            case 3: OnInputEnd(fpsInputField.text); break;
        }
    }


    void OnInputChanged(string _input)
    {
        int fps = Convert.ToInt32(_input);

    }


    void OnInputEnd(string _input)
    {
        if (int.TryParse(_input, out int fps))
        {
            fps = Mathf.Max(10, fps); // Минимум 10 FPS
            fpsInputField.text = fps.ToString();
            Application.targetFrameRate = fps;
            PlayerPrefs.SetInt("FPSInput", fps);
            PlayerPrefs.Save();
        }
        else
        {
            fpsInputField.text = "10"; // Сброс на стандартное значение
        }
    }

}


