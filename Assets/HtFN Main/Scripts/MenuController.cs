using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
public class MenuController : MonoBehaviour 
{
    [SerializeField]
    public GameObject layoutSettings;
    public GameObject layoutMenuSettings;
    public Slider SliderMusic;
    public AudioSource AudioSourceMusic;
    public LoadScene _loadScene;
    
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = new SceneLoader();
    }
    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        AudioSourceMusic.volume = musicVolume; // ������������� ����������� ��������
        SliderMusic.value = musicVolume;
        SliderMusic.onValueChanged.AddListener(OnSlider);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClickPlay()
    {
        Debug.Log("����� Play");
        StartCoroutine(ProcessSwitchScene());
    }
    public void OnClickExit()
    {
        Debug.Log("����� Exit");
        Application.Quit();
    }
    public void OnClickSettings()
    {
        Debug.Log("����� Exit");

        layoutSettings.gameObject.SetActive(true);
        layoutMenuSettings.gameObject.SetActive(false);
    }
    public void OnClickReturn()
    {
        layoutMenuSettings.gameObject.SetActive(true);
        layoutSettings.gameObject.SetActive(false);
        Debug.Log("����� Exit");
    }
    private void OnSlider(float value)
    {
        AudioSourceMusic.volume = value; // ��������� ��������� ��� ��������� ��������
        PlayerPrefs.SetFloat("MusicVolume", value); // ��������� ��������
        PlayerPrefs.Save();
    }

    private IEnumerator ProcessSwitchScene()
    {
        Debug.Log("Start");
        layoutMenuSettings.gameObject.SetActive(false);
        _loadScene.Show();
        _loadScene.ShowMessage("Loading...");

        // ��������� ����������� �������� ����� � ����������� �������������� ����������
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("Level");
        asyncOp.allowSceneActivation = false;

        // ���, ���� �������� �� ���������� (asyncOp.progress ��������� 0.9)
        while (asyncOp.progress < 0.9f)
        {
            yield return null;
        }

        // ����� �������� ���������, ���������� ��������� � ��� ������� ������� F
        _loadScene.ShowMessage("Loading done. Press F.");
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

        // ��������� ��������� ����� �����
        asyncOp.allowSceneActivation = true;
        _loadScene.Hide();
        // ����������� ����� ��������� ���������� ���������


       
    }


}
