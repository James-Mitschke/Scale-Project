using Assets.Scripts.Classes;
using Assets.Scripts.Services.RandomGeneratorService;
using System;
using System.Collections;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerScript : MonoBehaviour
{
    private readonly TimeSpan maxLoadTimeSeconds = TimeSpan.FromSeconds(120);
    private float gameLoadProgress;
    public TextMeshProUGUI funFactsText;
    public TextMeshProUGUI loaderText;
    public Slider loaderSlider;
    public GameObject buttonsObject;
    public GameObject loadingObject;
    private IRandomGenerator _randomGenerator;

    // Start is called before the first frame update
    void Start()
    {
        _randomGenerator = new RandomGenerator();

        gameLoadProgress = 0.0f;

        if (funFactsText == null)
        {
            var foo = GameObject.FindGameObjectWithTag(GameTagsEnum.FunFacts.ToString());
            var funFactsTextObject = foo.GetFirstChildObjectWithTag(GameTagsEnum.Text.ToString());

            if (funFactsTextObject == null)
            {
                throw new NullReferenceException("Unable to find the fun facts text for the loading menu");
            }

            funFactsText = funFactsTextObject.GetComponent<TextMeshProUGUI>();
        }

        if (loaderText == null || loaderSlider == null)
        {
            var loaderObject = GameObject.FindGameObjectWithTag(GameTagsEnum.Loader.ToString());

            if (loaderText == null)
            {
                var loaderTextObject = loaderObject.GetFirstChildObjectWithTag(GameTagsEnum.Text.ToString());

                if (loaderTextObject == null)
                {
                    throw new NullReferenceException("Unable to find the loader text for the loading menu");
                }

                loaderText = loaderTextObject.GetComponent<TextMeshProUGUI>();
            }

            if (loaderSlider == null)
            {
                var loaderSliderObject = loaderObject.GetFirstChildObjectWithTag(GameTagsEnum.Slider.ToString());

                if (loaderSliderObject == null)
                {
                    throw new NullReferenceException("Unable to find the loader bar for the loading menu");
                }

                loaderSlider = loaderSliderObject.GetComponent<Slider>();
            }
        }

        if (buttonsObject == null)
        {
            buttonsObject = GameObject.FindGameObjectWithTag(GameTagsEnum.UIButtons.ToString());

            if (buttonsObject == null)
            {
                throw new NullReferenceException("Unable to find the main menu buttons object");
            }
        }

        if (loadingObject == null)
        {
            loadingObject = GameObject.FindGameObjectWithTag(GameTagsEnum.UILoading.ToString());

            if (loadingObject == null)
            {
                throw new NullReferenceException("Unable to find the main menu loading pop up object");
            }
            else
            {
                loadingObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates the displayed objects on the main menu and starts loading the main game.
    /// </summary>
    public void LoadGame()
    {
        loadingObject.SetActive(true);
        buttonsObject.SetActive(false);

        StartCoroutine(LoadMainGame());
    }

    /// <summary>
    /// Placeholder method for changing the menu to the leaderboard menu.
    /// </summary>
    void LoadLeaderboard()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Placeholder method for changing the menu to the settings menu.
    /// </summary>
    void LoadSettings()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Loads the main game with a loading bar and percentage, also includes random facts.
    /// Unfortunately the game loads so quickly most users will never experience this, at least the framework is here for future projects.
    /// </summary>
    private IEnumerator LoadMainGame()
    {
        int funFactIterations = 0;
        Stopwatch timeTracker = Stopwatch.StartNew();
        AsyncOperation sceneLoad = SceneManager.LoadSceneAsync("MainGameScene");

        yield return new WaitForSeconds(5);

        while (!sceneLoad.isDone && timeTracker.Elapsed < maxLoadTimeSeconds)
        {
            if (sceneLoad.progress > gameLoadProgress)
            {
                gameLoadProgress = sceneLoad.progress;
                loaderSlider.value = gameLoadProgress;
                loaderText.text = $"{(int)gameLoadProgress}%";
            }

            if (timeTracker.Elapsed.Seconds / 5 > funFactIterations)
            {
                ++funFactIterations;
                funFactsText.text = GetNewRandomFunFact();
            }

            yield return null;
        }

        if (timeTracker.Elapsed >= maxLoadTimeSeconds)
        {
            throw new TimeoutException("Game loading has timed out, please try again. If you continue to get this issue then contact the developer.");
        }
    }

    /// <summary>
    /// Gets a random fact from the dictionary in the constants file based on how many fact names are stored in the FunFactsEnum.
    /// </summary>
    /// <returns>A <see cref="string"/> containing a random fun fact.</returns>
    private string GetNewRandomFunFact()
    {
        var funFactNames = Enum.GetNames(typeof(FunFactsEnum));
        int factIndex = _randomGenerator.GetRangedRandomInt(0, funFactNames.Length);
        var factName = funFactNames[factIndex];

        return Constants.FunFacts[factName];
    }
}
