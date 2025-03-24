using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
public class MenuController : MonoBehaviour 
{
    [SerializeField]
    public GameObject layoutSettings;
    public GameObject layoutMenuSettings;
    public Slider SliderMusic;
    public AudioSource AudioSourceMusic;
    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        AudioSourceMusic.volume = musicVolume;
        SliderMusic.value = musicVolume; 
        SliderMusic.onValueChanged.AddListener(OnSlider);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClickPlay()
    {
        Debug.Log("����� Play");
        SceneManager.LoadScene("Level");
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
    public void OnSlider(float value)
    {
        AudioSourceMusic.volume = value;
    }
    

}
