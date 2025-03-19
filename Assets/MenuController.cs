using System.Globalization;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour 
{
    [SerializeField]
    public GameObject layoutSettings;
    public GameObject layoutMenuSettings;
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

}
