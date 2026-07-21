using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class AsyncLoader : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject instructionScreen;

    [Header("Loading Screen Slider")]
    [SerializeField] private Slider loadingSlider;

    [SerializeField] AudioSource AudioSource;

    [SerializeField] string[] infoText;
    [SerializeField] TextMeshProUGUI infoTextUI;
    [SerializeField] GameObject playButton;
    int textIndex = 0;

    private void Start()
    {
        infoTextUI.text = infoText[textIndex];
    }
    private void Update()
    {
        if(textIndex < 0)
        {
            textIndex = 0;
        }
        else if (textIndex >= infoText.Length)
        {
            textIndex = infoText.Length-1;
        }

        if(textIndex == infoText.Length - 1)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }

        infoTextUI.text = infoText[textIndex];
    }

    public void ShowInstructions()
    {
        AudioSource.Play();
        mainMenuScreen.SetActive(false);
        instructionScreen.SetActive(true);
    }
    public void LoadSceneAsync(string sceneName)
    {
        AudioSource.Play();
        instructionScreen.SetActive(false);
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

    public void LeftText()
    {
        AudioSource.Play();
        textIndex--;
    }
     public void RightText()
    {
        AudioSource.Play();
        textIndex++;
    }

}
