using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class AsyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenuScreen;

    [Header("Loading Screen Slider")]
    [SerializeField] private Slider loadingSlider;

    [SerializeField] AudioSource AudioSource;

    public void LoadSceneAsync(string sceneName)
    {
        AudioSource.Play();
        mainMenuScreen.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }


    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            yield return null;
        }
    }

    public void Quit()
    {
        AudioSource.Play();
        Application.Quit();
    }
}
