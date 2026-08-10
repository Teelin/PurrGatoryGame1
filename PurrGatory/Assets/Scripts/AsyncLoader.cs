using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using NUnit.Framework.Internal.Commands;

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
    [SerializeField] string startText;
    [SerializeField] TextMeshProUGUI infoTextUI, startTextUI;
    [SerializeField] GameObject backButton;
    int textIndex = 0;
    bool  textTyping = false;

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
            backButton.SetActive(true);
        }
        else
        {
            backButton.SetActive(false);
        }

        //infoTextUI.text = infoText[textIndex];
    }

    public void ShowInstructions()
    {
        AudioSource.Play();
        mainMenuScreen.SetActive(false);
        instructionScreen.SetActive(true);
        infoTextUI.text = "";
        StartCoroutine(PrintTextPerChar(infoText[textIndex]));
        textTyping = true;
    }

    public void LoadSceneAsync(string sceneName)
    {
        AudioSource.Play();
        mainMenuScreen.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(PrintStartText(startText, sceneName));
        
        
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

        if(textIndex == 0)
        {
            AudioSource.Play();
            instructionScreen.SetActive(false);
            mainMenuScreen.SetActive(true);
        }
        
        AudioSource.Play();
        StopAllCoroutines();
        if (!textTyping)
        {
            textIndex--;
            infoTextUI.text = "";
            StartCoroutine(PrintTextPerChar(infoText[textIndex]));
            textTyping = true;
        }
        else
        {
            StopAllCoroutines();
            infoTextUI.text = infoText[textIndex];
            textTyping = false;
        }
        
    }
     public void RightText()
    {

        AudioSource.Play();
        if (textIndex == infoText.Length - 1&& !textTyping)
        {
            return;
        }
        
        if(!textTyping)
        {
            textIndex++;
            infoTextUI.text = "";
            StartCoroutine(PrintTextPerChar(infoText[textIndex]));
            textTyping = true;
        }
        else
        {
            StopAllCoroutines();
            infoTextUI.text = infoText[textIndex];
            textTyping = false;
        }
        

        
    }

    IEnumerator PrintTextPerChar(string text)
    {
        foreach (char c in text)
        {
            infoTextUI.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        textTyping = false;
    }
    IEnumerator PrintStartText(string text, string sceneName)
    {
        foreach (char c in text)
        {
            startTextUI.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    public void BackButtenPress()
    {
        AudioSource.Play();
        instructionScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
    

}
