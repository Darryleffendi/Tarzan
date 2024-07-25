using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject loadingScreen;
    [SerializeField]
    private Image loadingBarFill;
    [SerializeField]
    private TextMeshProUGUI loadingText;
    [SerializeField]
    private Image bgImg0, bgImg1, fadeImg;
    
    private bool fadeIn, fadeOut;
    private string sceneToChange;

    public static SceneChanger Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        // Fade transition
        if (fadeIn)
        {
            fadeImg.gameObject.SetActive(true);

            var colorFade = fadeImg.color;
            colorFade.a = Mathf.Lerp(colorFade.a, 1f, 1.5f * Time.deltaTime);
            fadeImg.color = colorFade;
            if (colorFade.a > 0.9)
            {
                fadeIn = false;
                StartCoroutine(LoadSceneAsync(sceneToChange));
            }
        }

        // Fade out transition
        if (fadeOut)
        {
            var colorFade = fadeImg.color;
            colorFade.a = Mathf.Lerp(colorFade.a, 0f, 1.5f * Time.deltaTime);
            fadeImg.color = colorFade;

            if (colorFade.a < 0.1)
            {
                fadeOut = false;
                fadeImg.gameObject.SetActive(false);
                UIController ui = UIController.Instance;
                if (ui != null) ui.ShowPlayerCanvas(true);
            }
        }
    }

    public void GameScene()
    {
        fadeIn = true;
        sceneToChange = "MainScene";
    }

    public void MenuScene()
    {
        StartCoroutine(LoadSceneAsync("MainMenu"));
    }
    public void EndScene()
    {
        StartCoroutine(LoadSceneAsync("EndMenu"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        while (fadeIn) { }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        // Scene Loading
        while (!operation.isDone)
        {
            // Fade in first image
            var color0 = bgImg0.color;
            color0.a += 0.05f;
            Mathf.Clamp01(color0.a);
            bgImg0.color = color0;

            // Loading bar
            float progressValue = Mathf.Clamp01(operation.progress);
            loadingBarFill.fillAmount = progressValue;
            loadingText.text = ((int)(operation.progress * 100)).ToString() + " %";

            // Fade in second image
            if (operation.progress > 0.5f)
            {
                var color1 = bgImg1.color;
                color1.a += 0.05f;
                Mathf.Clamp01(color1.a);
                bgImg1.color = color1;
            }
            yield return null;
        }

        // Fade out images
        var colorOut = bgImg0.color;
        while (colorOut.a > 0)
        {
            colorOut.a -= 0.05f;
            bgImg0.color = colorOut;
            bgImg1.color = colorOut;
            yield return null;
        }
        loadingScreen.SetActive(false);

        fadeOut = true;
    }
}
