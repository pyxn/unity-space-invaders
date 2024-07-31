using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Loading,
        Game
    }

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    [SerializeField] private string mainMenuSceneName = "MainMenuScene";
    [SerializeField] private string loadingSceneName = "LoadingScene";
    [SerializeField] private string gameSceneName = "GameScene";

    private AsyncOperation gameSceneLoadOperation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        OnStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.MainMenu:
                StartCoroutine(LoadSceneAsync(mainMenuSceneName));
                break;
            case GameState.Loading:
                StartCoroutine(LoadLoadingScene());
                break;
            case GameState.Game:
                StartCoroutine(TransitionToGameScene());
                break;
            default:
                Debug.LogError($"Unhandled game state: {newState}");
                break;
        }
    }

    private IEnumerator LoadLoadingScene()
    {
        // Load the loading scene
        yield return StartCoroutine(LoadSceneAsync(loadingSceneName));
        Debug.Log("Loading scene is now active");

        // Start preloading the game scene
        gameSceneLoadOperation = SceneManager.LoadSceneAsync(gameSceneName);
        gameSceneLoadOperation.allowSceneActivation = false;

        // Wait for 1.618 seconds or until the scene is almost loaded, whichever is longer
        float elapsedTime = 0f;
        while (elapsedTime < 1.618f || gameSceneLoadOperation.progress < 0.9f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Switch to the game scene
        ChangeState(GameState.Game);
    }

    private IEnumerator TransitionToGameScene()
    {
        if (gameSceneLoadOperation != null && gameSceneLoadOperation.progress >= 0.9f)
        {
            gameSceneLoadOperation.allowSceneActivation = true;
            while (!gameSceneLoadOperation.isDone)
            {
                yield return null;
            }
        }
        else
        {
            // Fallback in case the game scene hasn't been preloaded
            yield return StartCoroutine(LoadSceneAsync(gameSceneName));
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void StartLoading()
    {
        ChangeState(GameState.Loading);
    }

    public void ReturnToMainMenu()
    {
        ChangeState(GameState.MainMenu);
    }
}