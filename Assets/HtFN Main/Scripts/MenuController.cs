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
        AudioSourceMusic.volume = musicVolume; // Устанавливаем загруженное значение
        SliderMusic.value = musicVolume;
        SliderMusic.onValueChanged.AddListener(OnSlider);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClickPlay()
    {
        Debug.Log("Нажал Play");
        StartCoroutine(ProcessSwitchScene());
    }
    public void OnClickExit()
    {
        Debug.Log("Нажал Exit");
        Application.Quit();
    }
    public void OnClickSettings()
    {
        Debug.Log("Нажал Exit");

        layoutSettings.gameObject.SetActive(true);
        layoutMenuSettings.gameObject.SetActive(false);
    }
    public void OnClickReturn()
    {
        layoutMenuSettings.gameObject.SetActive(true);
        layoutSettings.gameObject.SetActive(false);
        Debug.Log("Нажал Exit");
    }
    private void OnSlider(float value)
    {
        AudioSourceMusic.volume = value; // Обновляем громкость при изменении слайдера
        PlayerPrefs.SetFloat("MusicVolume", value); // Сохраняем значение
        PlayerPrefs.Save();
    }

    private IEnumerator ProcessSwitchScene()
    {
        Debug.Log("Start");
        layoutMenuSettings.gameObject.SetActive(false);
        _loadScene.Show();
        _loadScene.ShowMessage("Loading...");

        // Запускаем асинхронную загрузку сцены с отключенной автоматической активацией
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync("Level");
        asyncOp.allowSceneActivation = false;

        // Ждём, пока загрузка не завершится (asyncOp.progress достигнет 0.9)
        while (asyncOp.progress < 0.9f)
        {
            yield return null;
        }

        // Когда загрузка завершена, показываем сообщение и ждём нажатия клавиши F
        _loadScene.ShowMessage("Loading done. Press F.");
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));

        // Разрешаем активацию новой сцены
        asyncOp.allowSceneActivation = true;
        _loadScene.Hide();
        // Опционально можно подождать завершения активации


       
    }


}
