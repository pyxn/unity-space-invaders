using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    private int score;

    public int Score => score;

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
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged += HandleGameStateChanged;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleGameStateChanged;
        }
    }

    private bool canIncrementScore = true;

    private void HandleGameStateChanged(GameManager.GameState newState)
    {
        canIncrementScore = newState != GameManager.GameState.GameOver;
    }

    public void IncrementScore()
    {
        if (canIncrementScore)
        {
            score++;
            OnScoreChanged?.Invoke(score);
        }
    }

    public event System.Action<int> OnScoreChanged;
}