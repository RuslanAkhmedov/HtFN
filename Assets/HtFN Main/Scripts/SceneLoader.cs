using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public IEnumerator LoadAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        AsyncOperation waitLoading = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
        yield return new WaitUntil(() => waitLoading.isDone); // Ожидание завершения загрузки
    }
}
