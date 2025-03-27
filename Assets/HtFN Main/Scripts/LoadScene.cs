using TMPro;
using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField] private TMP_Text _messageText;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // <-- Исправлено
        Hide();
    }


    // Update is called once per frame
    public void Show() => gameObject.SetActive(true);

    public void Hide () => gameObject.SetActive(false);

    public void ShowMessage(string message) => _messageText.text = message;

}
