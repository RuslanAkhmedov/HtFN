using System.Globalization;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuController : MonoBehaviour 
{
    [SerializeField]
    public GameObject layoutSettings;
    public GameObject layoutMenuSettings;
    public Slider SliderMusic;
    public AudioSource AudioSourceMusic;
    private void Start()
    {
        SliderMusic.onValueChanged.AddListener(OnSlider);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnClickPlay()
    {
        Debug.Log("Нажал Play");
        SceneManager.LoadScene("Level");
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
    public void OnSlider(float value)
    {
        AudioSourceMusic.volume = value;
    }
    

}
