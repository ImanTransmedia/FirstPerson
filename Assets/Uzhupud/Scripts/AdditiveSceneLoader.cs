using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoader : MonoBehaviour
{
    public string[] scenesToLoad;
    public bool loadOnStart = true;
    void Start()
    {
        if (!loadOnStart || scenesToLoad == null || scenesToLoad.Length == 0)
            return;

        foreach (var sceneName in scenesToLoad)
        {
            if (string.IsNullOrEmpty(sceneName))
                continue;

            if (SceneManager.GetSceneByName(sceneName).isLoaded)
                continue;

            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}